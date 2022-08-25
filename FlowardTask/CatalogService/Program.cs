using CatalogService.Interfaces.IRepositories;
using CatalogService.Interfaces.IServices;
using CatalogService.Migrations;
using CatalogService.DbModels;
using CatalogService.Repositories;
using CatalogService.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CatalogContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// add dependency injections
builder.Services.AddScoped<CatalogContext, CatalogContext>();
builder.Services.AddScoped<IRepository<ProductDbModel>, ProductsRepository>();
builder.Services.AddScoped<IProductsService, ProductsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
