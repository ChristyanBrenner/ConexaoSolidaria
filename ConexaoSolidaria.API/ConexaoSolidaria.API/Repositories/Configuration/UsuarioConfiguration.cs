using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("USUARIO");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Nome)
                .HasColumnName("NOME_COMPLETO")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnName("EMAIL")
                .HasMaxLength(150)
                .IsRequired();

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.Cpf)
                .HasColumnName("CPF")
                .HasMaxLength(11)
                .IsRequired();

            builder.HasIndex(x => x.Cpf)
                .IsUnique();

            builder.Property(x => x.SenhaHash)
                .HasColumnName("SENHA_HASH")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Role)
                .HasColumnName("ROLE")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .IsRequired();
        }
    }
}
