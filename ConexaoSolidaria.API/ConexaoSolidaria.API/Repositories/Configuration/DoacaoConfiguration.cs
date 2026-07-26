using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configuration
{
    public class DoacaoConfiguration : IEntityTypeConfiguration<Doacao>
    {
        public void Configure(EntityTypeBuilder<Doacao> builder)
        {
            builder.ToTable("DOACAO");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.UsuarioId)
                .HasColumnName("USUARIO_ID")
                .IsRequired();

            builder.Property(x => x.CampanhaId)
                .HasColumnName("CAMPANHA_ID")
                .IsRequired();

            builder.Property(x => x.ValorDoacao)
                .HasColumnName("VALOR_DOACAO")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("STATUS")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();

            builder.Property(x => x.DataProcessamento)
                .HasColumnName("DATA_PROCESSAMENTO");

            builder.HasOne(x => x.Usuario)
                .WithMany(x => x.Doacoes)
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Campanha)
                .WithMany(x => x.Doacoes)
                .HasForeignKey(x => x.CampanhaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}