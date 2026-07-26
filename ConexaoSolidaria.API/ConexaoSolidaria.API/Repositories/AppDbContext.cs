using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Campanha> Campanha { get; set; }
        public DbSet<Doacao> Doacao { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
        }
    }
}