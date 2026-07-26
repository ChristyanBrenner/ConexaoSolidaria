using ConexaoSolidaria.Worker.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConexaoSolidaria.Worker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Campanha> Campanhas { get; set; }
        public DbSet<Doacao> Doacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Campanha>(entity =>
            {
                entity.ToTable("CAMPANHA");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("ID");

                entity.Property(x => x.ValorArrecadado)
                    .HasColumnName("VALOR_TOTAL_ARRECADADO")
                    .HasPrecision(18, 2);
            });

            modelBuilder.Entity<Doacao>(entity =>
            {
                entity.ToTable("DOACAO");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasColumnName("ID");

                entity.Property(x => x.CampanhaId)
                    .HasColumnName("CAMPANHA_ID");

                entity.Property(x => x.ValorDoacao)
                    .HasColumnName("VALOR_DOACAO")
                    .HasPrecision(18, 2);

                entity.Property(x => x.Status)
                    .HasColumnName("STATUS")
                    .HasConversion<int>();

                entity.Property(x => x.DataProcessamento)
                    .HasColumnName("DATA_PROCESSAMENTO");
            });
        }
    }
}