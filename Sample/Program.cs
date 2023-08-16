using Sample.Core.Filters;
using Sample.Infrastructure.Common;
using Sample.Infrastructure.Helpers.Configuration;
using Sample.Infrastructure.Helpers.Utility;
using Sample.Infrastructure.Middleware;
using Sample.Sercurity.JWT;
using Sample.WebAPI.Config;
using Sample.WebAPI.Core.Filters;
using Sample.WebAPI.Policy;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Configuration;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.SetupDb(builder.Configuration);
        builder.Services.RegisterAssembly(builder.Configuration);

        ConfigurationHelper.Initialize(builder.Configuration);

        builder.Services.AddControllers();

        builder.Services.AddSwaggerGenNewtonsoftSupport();

        // JWT Configuration from appsettings.json
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        // JWT authentication configuration
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), // Convert the secret key from base64
                ValidateIssuer = true,
                ValidIssuer = issuer, // Replace with the issuer used during token generation
                ValidateAudience = true,
                ValidAudience = audience, // Replace with the audience used during token generation
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Optional: Adjust the clock skew to accommodate small time differences between server and client
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // Remove the "Bearer " prefix if present in the token
                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                    context.Request.Headers["Authorization"] = $"Bearer {token}";
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    // Log the authentication failure details for debugging
                    Console.WriteLine(context.Exception);
                    return Task.CompletedTask;
                }
            };
        });

        // Add Serilog and configure logging
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(dispose: true);
        });

        // Set the default language for FluentValidation error messages (e.g., en-US)
        ValidatorOptions.Global.LanguageManager.Enabled = true;
        ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("en-EN");

        // Swagger configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sample-WebAPI", Version = "v1" });
            c.EnableAnnotations();
            c.DescribeAllParametersInCamelCase();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseSwagger();

        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

}