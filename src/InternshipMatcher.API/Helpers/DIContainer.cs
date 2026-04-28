using FluentValidation;
using InternshipMatcher.Application.Behaviours;
using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Infra.DbContext;
using InternshipMatcher.Infra.Services;
using MediatR;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Formatting.Compact;
using System.IO.Compression;
using System.Text;
using System.Threading.RateLimiting;
using InternshipMatcher.Application;
using Microsoft.OpenApi.Models;

namespace InternshipMatcher.API.Helpers;

public static class DIContainer
{
    public static IServiceCollection AddAuthAndAuthorizationWithJWT(this IServiceCollection Services , IConfiguration Configuration)
    {

        // 7. Authentication & Authorization
        Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]!)),
              ValidateIssuer = true,
              ValidIssuer = Configuration["Jwt:Issuer"],
              ValidateAudience = true,
              ValidAudience = Configuration["Jwt:Audience"],
              ValidateLifetime = true,
              ClockSkew = TimeSpan.Zero
          };
      });
        Services.AddAuthorization();
        Services.AddScoped<IJWTService, JWTService>();
        Services.AddScoped<IPasswordHasher, PasswordHasherService>();
        Services.AddScoped<IEmailHasher  , EmailHasherService>();
        Services.AddScoped<IUserService  ,  UserService>();

        return Services;

    }
    public static IServiceCollection AddResponseCompressionEnc(this IServiceCollection services)
    {
       ///Compression
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.MimeTypes = new[] { "application/json", "text/plain", "text/html", "application/xml" };
            options.Providers.Add<GzipCompressionProvider>();
            options.Providers.Add<BrotliCompressionProvider>();
        });
       services.Configure<GzipCompressionProviderOptions>(o => o.Level = CompressionLevel.Optimal);
       services.Configure<BrotliCompressionProviderOptions>(o => o.Level = CompressionLevel.Optimal);
        return services;
    }
    public static IServiceCollection AddRequestErrorDetails(this IServiceCollection Services) 
    {
        Services.AddProblemDetails(opt =>
        {
            opt.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.Add("RequestID", context.HttpContext.TraceIdentifier);
            };
        });

        return Services;
    }
    public static IServiceCollection AddPipelineBehaviour(this IServiceCollection Services)
    {

       Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(BehaviourValidation<,>));
        Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        return Services;
    }
    public static IServiceCollection MediateR(this IServiceCollection Services)
    {
        Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ApplicationMarker).Assembly));
        return Services;
    }
    public static IServiceCollection RegisterDbConnection(this IServiceCollection Services , IConfiguration configuration)
    {
        Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        return Services;
    }

    public static IServiceCollection SwaggerRegistration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "InternshipMatcher API",
                Version = "v1"
            });

            var bearerScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter your JWT token here. Example: Bearer eyJhbGci...",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };

            options.AddSecurityDefinition("Bearer", bearerScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference 
                        { Type = ReferenceType.SecurityScheme, 
                            
                         Id = "Bearer" }
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }
    public static IServiceCollection AddRateLimit(this IServiceCollection Services)
    {
        Services.AddRateLimiter(opt =>
        {
            opt.AddConcurrencyLimiter("ConcurrencyPolicy", o =>
            {
                o.PermitLimit = 50;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.QueueLimit = 2;
            });
            opt.AddTokenBucketLimiter("TokenPolicy", o =>
            {
                o.TokenLimit = 10;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.QueueLimit = 2;
                o.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
                o.TokensPerPeriod = 10;
                o.AutoReplenishment = true;
            });
            opt.AddFixedWindowLimiter("FixedPolicy", o =>
            {
                o.Window = TimeSpan.FromMinutes(1);
                o.PermitLimit = 10;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.QueueLimit = 2;
            });
            opt.AddSlidingWindowLimiter("DefaultPolicy", o =>
            {
                o.Window = TimeSpan.FromMinutes(1);
                o.PermitLimit = 10;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.SegmentsPerWindow = 2;
                o.AutoReplenishment = true;
                o.QueueLimit = 2;
            });
        });
        return Services;
    }
    public static IHostBuilder Serilog(this IHostBuilder host, IConfiguration configuration)
    {
        host.UseSerilog((ctx, cfg) =>
            cfg.ReadFrom.Configuration(configuration)
               .Enrich.FromLogContext()
               .Enrich.WithMachineName()
        );

        return host;
    }
    public static IServiceCollection AddCORS(this IServiceCollection services)
    {
        services.AddCors(o => o.AddDefaultPolicy(p =>
        p.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod()));


        return services;
    }
        
}
