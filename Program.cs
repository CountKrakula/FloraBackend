using FloraBackend.Data;
using FloraBackend.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FloraBackend.Repositories;
using FloraBackend.Repositories.IRepository;
using FloraBackend.Services;
using FloraBackend.Services.IServices;
using Scalar.AspNetCore;

namespace FloraBackend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<FloraDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentityApiEndpoints<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddRoles<IdentityRole<int>>().AddEntityFrameworkStores<FloraDBContext>();

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.ConfigureApplicationCookie(options =>
                {
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });
            }
            builder.Services.AddControllers();
            // Add services to the container.
            builder.Services.AddOpenApi();
            // Repositori
            builder.Services.AddScoped<IPlantRepository, PlantRepository>();

            // Service
            builder.Services.AddScoped<IPlantService, PlantService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();



            app.Run();

        }
    }
}
