using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parth_Traders.Data;
using Parth_Traders.Data.DataModel.User;
using Parth_Traders.Domain.Enums;
namespace Parth_Traders.Controllers.User;

[ApiController, Route("api/payments")]
public class PaymentController : ControllerBase
{
    // Sandbox-only by construction. This integration cannot charge real accounts.
    private static readonly HttpClient Http = new() { BaseAddress = new Uri("https://api-m.sandbox.paypal.com/"), Timeout = TimeSpan.FromSeconds(25) };
    private readonly ParthTradersContext db;
    private readonly IConfiguration config;
    public PaymentController(ParthTradersContext db, IConfiguration config) { this.db = db; this.config = config; }
    [HttpGet("config")]
    public IActionResult PublicConfig() => Ok(new { enabled = Configured, clientId = config["PayPal:ClientId"], mode = "sandbox" });
    private bool Configured => !string.IsNullOrWhiteSpace(config["PayPal:ClientId"]) && !string.IsNullOrWhiteSpace(config["PayPal:ClientSecret"]);
    public sealed record CartItem(string Sku, int Quantity);
    public sealed record PricedItem(string Sku, string Name, int Quantity, decimal Price, decimal Discount);
    private static decimal NetPrice(decimal price, decimal percentage) => decimal.Round(price * (100m - percentage) / 100m, 2, MidpointRounding.AwayFromZero);
    private static string Money(decimal value) => value.ToString("0.00", CultureInfo.InvariantCulture);
    private async Task<string> AccessToken(CancellationToken ct)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "v1/oauth2/token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(config["PayPal:ClientId"] + ":" + config["PayPal:ClientSecret"])));
        request.Content = new FormUrlEncodedContent(new Dictionary<string,string> { ["grant_type"] = "client_credentials" });
        using var response = await Http.SendAsync(request, ct); response.EnsureSuccessStatusCode();
        using var data = JsonDocument.Parse(await response.Content.ReadAsStringAsync(ct)); return data.RootElement.GetProperty("access_token").GetString()!;
    }
    private static async Task<JsonElement> PayPal(HttpMethod method, string path, string token, object? body, string? id, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(method, path);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        if (id != null) request.Headers.Add("PayPal-Request-Id", id);
        if (body != null) request.Content = JsonContent.Create(body);
        using var response = await Http.SendAsync(request, ct); response.EnsureSuccessStatusCode();
        using var data = JsonDocument.Parse(await response.Content.ReadAsStringAsync(ct)); return data.RootElement.Clone();
    }
    [HttpPost("orders"), Authorize(Roles = "Customer")]
    public async Task<IActionResult> Create([FromBody] List<CartItem> cart, CancellationToken ct)
    {
        if (!Configured) return StatusCode(503, new { message = "Sandbox payment setup is not complete." });
        if (cart.Count == 0 || cart.Count > 100 || cart.Any(i => string.IsNullOrWhiteSpace(i.Sku) || i.Quantity < 1 || i.Quantity > 1000) || cart.Select(i => i.Sku).Distinct().Count() != cart.Count)
            return BadRequest(new { message = "Check the items and quantities in your cart." });
        var items = new List<PricedItem>();
        foreach (var item in cart)
        {
            var product = await db.Products.AsNoTracking().SingleOrDefaultAsync(p => p.ProductSku == item.Sku, ct);
            if (product == null || product.UnitsInStock < item.Quantity || product.Discount < 0 || product.Discount > 100 || NetPrice(product.UnitPrice, product.Discount) <= 0)
                return Conflict(new { message = "An item is unavailable or its stock changed. Review your cart." });
            items.Add(new(item.Sku, product.ProductName, item.Quantity, product.UnitPrice, product.Discount));
        }
        var total = items.Sum(i => NetPrice(i.Price, i.Discount) * i.Quantity);
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        try
        {
            var token = await AccessToken(ct);
            var result = await PayPal(HttpMethod.Post, "v2/checkout/orders", token, new {
                intent = "CAPTURE",
                purchase_units = new[] { new { custom_id = customerId,
                    amount = new { currency_code = "USD", value = Money(total), breakdown = new { item_total = new { currency_code = "USD", value = Money(total) } } },
                    items = items.Select(i => new { name = i.Name, sku = i.Sku, quantity = i.Quantity.ToString(CultureInfo.InvariantCulture), unit_amount = new { currency_code = "USD", value = Money(NetPrice(i.Price, i.Discount)) } }).ToArray()
                } }, application_context = new { shipping_preference = "NO_SHIPPING" }
            }, Guid.NewGuid().ToString(), ct);
            var id = result.GetProperty("id").GetString()!;
            db.PaymentDrafts.Add(new() { PayPalOrderId = id, CustomerId = customerId, ItemsJson = JsonSerializer.Serialize(items), Total = total, CreatedAt = DateTime.UtcNow });
            await db.SaveChangesAsync(ct); return Ok(new { id, total, mode = "sandbox" });
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { return StatusCode(503, new { message = "PayPal sandbox is unavailable. Please try again later." }); }
    }
    [HttpGet("orders/{id}"), Authorize(Roles = "Customer")]
    public async Task<IActionResult> Status(string id, CancellationToken ct)
    {
        var draft = await db.PaymentDrafts.AsNoTracking().SingleOrDefaultAsync(d => d.PayPalOrderId == id, ct);
        if (draft == null || draft.CustomerId != User.FindFirstValue(ClaimTypes.NameIdentifier)) return NotFound();
        string? remoteStatus = null;
        if (draft.OrderId == null && Configured)
        {
            try {
                var token = await AccessToken(ct);
                var remote = await PayPal(HttpMethod.Get, "v2/checkout/orders/" + Uri.EscapeDataString(id), token, null, null, ct);
                remoteStatus = remote.GetProperty("status").GetString();
            } catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { return StatusCode(503, new { message = "Sandbox status is temporarily unavailable." }); }
        }
        return Ok(new { id, total = draft.Total, transactionId = draft.TransactionId, orderId = draft.OrderId, remoteStatus, mode = "sandbox" });
    }
    [HttpPost("orders/{id}/capture"), Authorize(Roles = "Customer")]
    public async Task<IActionResult> Capture(string id, CancellationToken ct)
    {
        if (!Configured) return StatusCode(503, new { message = "Sandbox payment setup is not complete." });
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        var draft = await db.PaymentDrafts.FromSqlInterpolated($"SELECT * FROM \"PaymentDrafts\" WHERE \"PayPalOrderId\" = {id} FOR UPDATE").SingleOrDefaultAsync(ct);
        if (draft == null || draft.CustomerId != User.FindFirstValue(ClaimTypes.NameIdentifier)) return NotFound();
        if (draft.OrderId != null) return Ok(new { transactionId = draft.TransactionId, orderId = draft.OrderId, mode = "sandbox" });
        var items = JsonSerializer.Deserialize<List<PricedItem>>(draft.ItemsJson)!;
        if (items.Sum(i => NetPrice(i.Price, i.Discount) * i.Quantity) != draft.Total)
            return Conflict(new { message = "Pricing was corrected. Cancel this checkout and start a new sandbox payment." });
        var details = new List<OrderDetailDataModel>();
        foreach (var item in items.OrderBy(i => i.Sku, StringComparer.Ordinal))
        {
            var product = await db.Products.FromSqlInterpolated($"SELECT * FROM \"Products\" WHERE \"ProductSku\" = {item.Sku} FOR UPDATE").SingleOrDefaultAsync(ct);
            if (product == null || product.UnitsInStock < item.Quantity || product.UnitPrice != item.Price || product.Discount != item.Discount)
                return Conflict(new { message = "Stock or prices changed. No new payment was captured. Review your cart." });
            // Sandbox tests never consume business inventory.
            details.Add(new() { ProductId = product.ProductId, Price = item.Price, Discount = item.Discount, Quantity = item.Quantity, TotalPrice = NetPrice(item.Price, item.Discount) * item.Quantity });
        }
        try
        {
            var token = await AccessToken(ct);
            var order = await PayPal(HttpMethod.Get, "v2/checkout/orders/" + Uri.EscapeDataString(id), token, null, null, ct);
            var unit = order.GetProperty("purchase_units")[0]; var amount = unit.GetProperty("amount");
            if (unit.GetProperty("custom_id").GetString() != draft.CustomerId || amount.GetProperty("currency_code").GetString() != "USD" || decimal.Parse(amount.GetProperty("value").GetString()!, CultureInfo.InvariantCulture) != draft.Total)
                return BadRequest(new { message = "Payment verification failed. Contact Parth Traders." });
            var captured = order.GetProperty("status").GetString() == "COMPLETED" ? order : await PayPal(HttpMethod.Post, "v2/checkout/orders/" + Uri.EscapeDataString(id) + "/capture", token, new { }, "capture-" + id, ct);
            var capture = captured.GetProperty("purchase_units")[0].GetProperty("payments").GetProperty("captures")[0];
            var paidAmount = capture.GetProperty("amount");
            if (capture.GetProperty("status").GetString() != "COMPLETED" || paidAmount.GetProperty("currency_code").GetString() != "USD" || decimal.Parse(paidAmount.GetProperty("value").GetString()!, CultureInfo.InvariantCulture) != draft.Total)
                return BadRequest(new { message = "Payment is not complete. Contact Parth Traders before retrying." });
            var saved = new OrderDataModel { CustomerId = draft.CustomerId, TransactionId = capture.GetProperty("id").GetString()!, OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Completed, IsSandbox = true, PaymentType = PaymentType.General, GrandTotal = draft.Total, OrderDetails = details };
            db.Orders.Add(saved); await db.SaveChangesAsync(ct); draft.TransactionId = saved.TransactionId; draft.OrderId = saved.OrderId;
            await db.SaveChangesAsync(ct); await transaction.CommitAsync(ct);
            return Ok(new { transactionId = saved.TransactionId, orderId = saved.OrderId, mode = "sandbox" });
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { return StatusCode(503, new { message = "Could not verify the sandbox payment. Retry this checkout to check its status; do not start another payment." }); }
    }
}
