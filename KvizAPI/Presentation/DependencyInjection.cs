using KvizAPI.Domain.Entities;
using KvizAPI.Infrastructure.DBContexts;
using KvizAPI.Infrastructure.Import;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using KvizAPI.Application.Services;
using KvizAPI.Application.Common;
using KvizAPI.Domain.Interfaces;

namespace KvizAPI.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<QuizDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IQuestionsService, QuestionsService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IAuthService, AuthService>();
            services.Configure<AuthOptions>(configuration.GetSection("AuthOptions"));
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));

            var authOptions = configuration
                .GetSection("AuthOptions")
                .Get<AuthOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RoleClaimType = ClaimTypes.Role,
                        ValidIssuer = authOptions.Issuer,
                        ValidAudience = authOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SecretKey))
                    };
                });
            services.AddHttpContextAccessor();
            services.AddMemoryCache();

            return services;
        }
     }
}
