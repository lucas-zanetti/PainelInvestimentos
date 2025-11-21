using API_Painel_Investimentos.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API_Painel_Investimentos.Data.Contexts;

public class DbUsuarioContext(DbContextOptions<DbUsuarioContext> options) : IdentityDbContext<UsuarioEntity>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UsuarioEntity>(b =>
        {
            b.Property(u => u.Role).IsRequired();
        });
    }
}
