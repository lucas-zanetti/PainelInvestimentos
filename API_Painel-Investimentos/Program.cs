
using API_Painel_Investimentos.Configuration;
using API_Painel_Investimentos.Data.Contexts;
using API_Painel_Investimentos.Data.Migrations;
using API_Painel_Investimentos.Data.Repositories;
using API_Painel_Investimentos.Interfaces;
using API_Painel_Investimentos.Services;
using Microsoft.EntityFrameworkCore;

namespace API_Painel_Investimentos;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JwtConfiguration"));

        builder.Services.AddDbContext<DbUsuarioContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("DbUsuarioConnectionString"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
        });

        var app = builder.Build();

        app.MigrateDatabases<DbUsuarioContext>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
