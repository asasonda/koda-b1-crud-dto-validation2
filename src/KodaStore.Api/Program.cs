using System.Globalization;
using KodaStore.Api.Data;
using KodaStore.Api.Middlewares;
using KodaStore.Api.Repositories;
using KodaStore.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// builder.Services.AddSingleton<InMemoryProductCatalog>();
// builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
// builder.Services.AddSingleton<IProductService, ProductService>();

builder.Services.AddScoped<IProductRepository, SqlProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
