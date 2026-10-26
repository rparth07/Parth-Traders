using Microsoft.AspNetCore.Identity;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Parth_Traders.Data;
using Parth_Traders.Data.DataModel.Admin;
using Parth_Traders.Data.DataModel.User;
using Parth_Traders.Data.Repositories.Admin;
using Parth_Traders.Data.Repositories.User;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.AdminInterfaces;
using Parth_Traders.Domain.RepositoryInterfaces.User;
using Parth_Traders.Extensions;
using Parth_Traders.Service.Admin;
using Parth_Traders.Service.Filter;
using Parth_Traders.Service.Services.Admin;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;
using Parth_Traders.Service.Services.User;
using Parth_Traders.Service.Services.User.UserInterface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureLoggerService();

builder.Services.AddControllers()
    .AddNewtonsoftJson(setupAction =>
    {
        setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        setupAction.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        setupAction.SerializerSettings.MaxDepth = 32;
    });

builder.Services.AddAutoMapper(config => config.Internal().ForAllMaps((_, map) => map.MaxDepth(16)), AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductHelperService, ProductHelperService>();

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IOrderHelperService, OrderHelperService>();

builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
builder.Services.AddScoped<ICustomerAuthManager, CustomerAuthManager>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwagger();

builder.Services.AddIdentityCore<CustomerDataModel>().AddEntityFrameworkStores<ParthTradersContext>();

builder.Services.AddIdentity<AdminDataModel, IdentityRole>()
    .AddEntityFrameworkStores<ParthTradersContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

//Filter to handle exception
builder.Services.AddMvc(options =>
{
    options.Filters.Add(new UserFriendlyExceptionFilterAttribute());
});

var databaseUrl = builder.Configuration["DATABASE_URL"];
if (string.IsNullOrWhiteSpace(databaseUrl)) throw new InvalidOperationException("DATABASE_URL is required.");
var signingKey = builder.Configuration["JwtSettings:signingKey"];
if (string.IsNullOrWhiteSpace(signingKey) || signingKey.Length < 32) throw new InvalidOperationException("A private JWT signing key of at least 32 characters is required.");
builder.Services.AddDbContext<ParthTradersContext>(options => options.UseNpgsql(DatabaseConnection.Parse(databaseUrl)));
var origins = (builder.Configuration["AllowedOrigin"] ?? "https://rparth07-parth-traders.netlify.app").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod()));
var port = builder.Configuration["PORT"];
if (!string.IsNullOrEmpty(port)) builder.WebHost.UseUrls("http://0.0.0.0:" + port);
var app = builder.Build();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.UseSwagger();
/*app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Parth Traders API v1");
    s.SwaggerEndpoint("/swagger/v2/swagger.json", "Parth Traders API v2");
});*/

if (args.Contains("--migrate"))
{
    using var scope = app.Services.CreateScope();
    var migrationUrl = builder.Configuration["MIGRATION_DATABASE_URL"] ?? databaseUrl;
    using var migrationDb = new ParthTradersContext(new DbContextOptionsBuilder<ParthTradersContext>()
        .UseNpgsql(DatabaseConnection.Parse(migrationUrl)).Options);
    await migrationDb.Database.MigrateAsync();
    if (args.Contains("--import-products")) await Parth_Traders.Recovery.ProductRecovery.Import(migrationDb);
    await Parth_Traders.Recovery.ProductRecovery.BootstrapAdmin(scope.ServiceProvider, builder.Configuration);
    return;
}
app.MapGet("/health/live", () => Results.Ok(new { status = "alive" }));
app.MapGet("/health/ready", async (ParthTradersContext db) => {
    try { return await db.Database.CanConnectAsync() ? Results.Ok(new { status = "ready" }) : Results.StatusCode(503); }
    catch { return Results.StatusCode(503); }
});
app.Run();
