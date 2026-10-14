using FloraBackend.Data;
using FloraBackend.Repositories;
using FloraBackend.Repositories.IRepository;
using FloraBackend.Services;
using FloraBackend.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// Databas
builder.Services.AddDbContext<FloraDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositori
builder.Services.AddScoped<IPlantRepository, PlantRepository>();

// Service
builder.Services.AddScoped<IPlantService, PlantService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();