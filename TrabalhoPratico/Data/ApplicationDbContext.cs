using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TrabalhoPratico.Models;

namespace TrabalhoPratico.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<CategoriaVeiculo> CategoriaVeiculo { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Localizacao> Localizacao { get; set; }
        public DbSet<Reserva> Reserva { get; set; }
        public DbSet<ReservaEstadoVeiculoLevantamento> ReservaEstadoVeiculoLevantamento { get; set; }
        public DbSet<ReservaEstadoVeiculoEntrega> ReservaEstadoVeiculoEntrega { get; set; }
        public DbSet<Veiculo> Veiculo { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TrabalhoPratico.Models.Classificacao> Classificacao { get; set; }
    }
}