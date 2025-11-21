
using API_Painel_Investimentos.Configuration;
using API_Painel_Investimentos.Data.Contexts;
using API_Painel_Investimentos.Data.Migrations;
using API_Painel_Investimentos.Data.Repositories;
using API_Painel_Investimentos.Filters;
using API_Painel_Investimentos.Interfaces;
using API_Painel_Investimentos.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace API_Painel_Investimentos;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JwtConfiguration"));

        builder.Services.AddDbContext<DbTelemetriaContext>(
            options => options.UseSqlite(builder.Configuration.GetConnectionString("DbTelemetriaConnectionString")));
            
        builder.Services.AddDbContext<DbUsuarioContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("DbUsuarioConnectionString"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        builder.Services.AddDbContext<DbPainelInvestimentoContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("DbPainelInvestimentoConnectionString"));
        });

        builder.Services.AddScoped<ITelemetriaRepository, TelemetriaRepository>();
        builder.Services.AddScoped<TelemetriaActionFilter>();
        builder.Services.AddScoped<ITelemetriaService, TelemetriaService>();
        builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
        builder.Services.AddScoped<IPainelInvestimentoRepository, PainelInvestimentoRepository>();
        builder.Services.AddScoped<IPainelInvestimentoService, PainelInvestimentoService>();

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add(new CustomExceptionFilter());
        }).ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddScoped<SchemaValidationFilter>();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API Painel Investimentos",
                Version = "v1",
                Description = "API responsável pela simulação e controle de investimentos de clientes."
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Cabeçalho de autorização JWT usando o esquema Bearer."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        var jwtConfig = builder.Configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();
        var key = Encoding.UTF8.GetBytes(jwtConfig!.Key);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true
            };
        });

        var app = builder.Build();

        app.MigrateAndSeed<DbTelemetriaContext>();
        app.MigrateAndSeed<DbUsuarioContext>(DbInitializer.SeedUsuarioContext);
        app.MigrateAndSeed<DbPainelInvestimentoContext>(DbInitializer.SeedPainelInvestimentoContext);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
