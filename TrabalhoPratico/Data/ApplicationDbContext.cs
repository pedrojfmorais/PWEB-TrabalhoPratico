using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrabalhoPratico.Models;

namespace TrabalhoPratico.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<CategoriaVeiculo> CategoriaVeiculo { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Veiculo> Veiculo { get; set; }
        public DbSet<Localizacao> Localizacao { get; set; }
        public DbSet<Reserva> Reserva { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}