using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json.Serialization;
using Parth_Traders.Data;
using Parth_Traders.Data.Repositories.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.Admin;
using Parth_Traders.Domain.RepositoryInterfaces.AdminInterfaces;
using Parth_Traders.Service;
using Parth_Traders.Service.Admin;
using Parth_Traders.Service.Services.Admin;
using Parth_Traders.Service.Services.Admin.AdminInterfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(setupAction => {
        setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        setupAction.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IProductHelperService, ProductHelperService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ParthTradersContext>();


builder.Services.AddDbContext<ParthTradersContext>(options => {
    options.UseSqlServer(
        @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = Parth Traders");
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
