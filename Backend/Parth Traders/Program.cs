using Microsoft.AspNetCore.Identity;
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
    });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

builder.Services.AddDbContext<ParthTradersContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionString").Value);
});

var allowedOrigin = builder.Configuration.GetSection("AllowedOrigin").Value;

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(builder =>
{
    builder.WithOrigins(allowedOrigin).AllowAnyHeader();
    builder.WithOrigins(allowedOrigin).AllowAnyMethod();
});

app.UseHttpsRedirection();

//app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.UseSwagger();
/*app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Parth Traders API v1");
    s.SwaggerEndpoint("/swagger/v2/swagger.json", "Parth Traders API v2");
});*/

app.Run();
