using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MotoManager.Application.Abstractions;
using MotoManager.Application.Vehicles;
using MotoManager.Application.Clients;
using MotoManager.Application.ServiceOrders;
using MotoManager.Application.ServiceOrderLabors;
using MotoManager.Application.ServiceOrderMaterials;
using MotoManager.Application.Materials;
using MotoManager.Application.PurchaseInvoices;
using MotoManager.Infrastructure.Data;
using MotoManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repozitorijum + servis
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<VehicleService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
builder.Services.AddScoped<ServiceOrderService>();
builder.Services.AddScoped<IServiceOrderLaborRepository, ServiceOrderLaborRepository>();
builder.Services.AddScoped<ServiceOrderLaborService>();
builder.Services.AddScoped<IServiceOrderMaterialRepository, ServiceOrderMaterialRepository>();
builder.Services.AddScoped<ServiceOrderMaterialService>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<MaterialService>();
builder.Services.AddScoped<IPurchaseInvoiceRepository, PurchaseInvoiceRepository>();
builder.Services.AddScoped<PurchaseInvoiceService>();

// Auth0 JWT Authentication
var domain = builder.Configuration["Auth0:Domain"];
var audience = builder.Configuration["Auth0:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = $"https://{domain}/";
    options.Audience = audience;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = $"https://{domain}/",
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true
    };
});

// CORS za lokalni Angular
const string AllowLocalAngular = "AllowLocalAngular";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowLocalAngular,
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:4200",
                "https://motomanager.azurewebsites.net",
                "https://lively-dune-0073f1003.3.azurestaticapps.net"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// CORS must be before Authentication/Authorization
app.UseCors(AllowLocalAngular);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
// Force restart
