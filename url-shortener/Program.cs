using Microsoft.EntityFrameworkCore;
using url_shortener.Data;
using url_shortener.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

namespace url_shortener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(setupAction =>
            {
                setupAction.AddSecurityDefinition("UrlShortenerBearerAuth", new OpenApiSecurityScheme() //Esto va a permitir usar swagger con el token.
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Description = "JWT token for login."
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "UrlShortenerBearerAuth" } //Tiene que coincidir con el id seteado arriba en la definición
                            }, new List<string>() }
                });
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<UrlShortenerContext>(dbContextOptions => dbContextOptions.UseSqlite(
                builder.Configuration["ConnectionStrings:DB"]), ServiceLifetime.Singleton);

            #region Services
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<UrlService>();
            #endregion

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Authentication:Issuer"],
                        ValidAudience = builder.Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
                    }
                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
               
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}