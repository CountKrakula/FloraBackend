using FloraBackend.Data;
using FloraBackend.Models;
using FloraBackend.Repositories;
using FloraBackend.Repositories.IRepository;
using FloraBackend.Services;
using FloraBackend.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            builder.Services.AddOpenApi();

            // Repositories
            builder.Services.AddScoped<IPlantRepository, PlantRepository>();
            builder.Services.AddScoped<ICareTaskRepository, CareTaskRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            builder.Services.AddScoped<IPlantHealthLogRepository, PlantHealthLogRepository>();

            // Services
            builder.Services.AddScoped<IPlantService, PlantService>();
            builder.Services.AddScoped<ICareTaskService, CareTaskService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IPlantHealthLogService, PlantHealthLogService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy.WithOrigins(builder.Configuration["Frontend_Domain"])
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            builder.Services.AddHttpClient<PlantSpeciesService>(client =>
            {
                client.BaseAddress = new Uri("https://perenual.com/api/v2/");
            });

            var app = builder.Build();

            app.UseCors("Frontend");

            app.UseAuthentication();
            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            var api = app.MapGroup("/api");
            app.MapIdentityApi<User>();
            
            // MapIdentityApi doesn't provide a /logout endpoint out of the box,
            // so this manually signs the user out and clears their auth cookie
            app.MapPost("/logout", async (SignInManager<User> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            }).RequireAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}