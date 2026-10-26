using System.ComponentModel.DataAnnotations;
namespace Parth_Traders.Data.DataModel.User;
public class PaymentDraftDataModel
{
    [Key] public string PayPalOrderId { get; set; } = "";
    public string CustomerId { get; set; } = "";
    public string ItemsJson { get; set; } = "";
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? TransactionId { get; set; }
    public long? OrderId { get; set; }
}
