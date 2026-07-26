using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configuration
{
    public class CampanhaConfiguration : IEntityTypeConfiguration<Campanha>
    {
        public void Configure(EntityTypeBuilder<Campanha> builder)
        {
            builder.ToTable("CAMPANHA");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Titulo)
                .HasColumnName("TITULO")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.DataInicio)
                .HasColumnName("DATA_INICIO")
                .IsRequired();

            builder.Property(x => x.DataFim)
                .HasColumnName("DATA_FIM")
                .IsRequired();

            builder.Property(x => x.MetaFinanceira)
                .HasColumnName("META_FINANCEIRA")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.ValorTotalArrecadado)
                .HasColumnName("VALOR_TOTAL_ARRECADADO")
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("STATUS")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();

            builder.HasMany(x => x.Doacoes)
                .WithOne(x => x.Campanha)
                .HasForeignKey(x => x.CampanhaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}