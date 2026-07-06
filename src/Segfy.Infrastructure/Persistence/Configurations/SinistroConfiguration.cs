using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Segfy.Domain.Entities;

namespace Segfy.Infrastructure.Persistence.Configurations;

public class SinistroConfiguration : IEntityTypeConfiguration<Sinistro>
{
    public void Configure(EntityTypeBuilder<Sinistro> builder)
    {
        builder.ToTable("Sinistros");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.ApoliceId).IsRequired();
        builder.Property(s => s.DataAbertura).IsRequired();

        builder.Property(s => s.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(s => s.MotivoNegativa).HasMaxLength(500);

        builder.OwnsOne(s => s.ValorEstimado, valor =>
        {
            valor.Property(v => v.Valor)
                .HasColumnName("ValorEstimado")
                .HasColumnType("numeric(18,2)")
                .IsRequired();
        });

        builder.OwnsOne(s => s.ValorAprovado, valor =>
        {
            valor.Property(v => v.Valor)
                .HasColumnName("ValorAprovado")
                .HasColumnType("numeric(18,2)");
        });

        builder.HasOne<Apolice>()
            .WithMany()
            .HasForeignKey(s => s.ApoliceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(s => s.Historico).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(s => s.Historico, historico =>
        {
            historico.ToTable("HistoricoSinistros");

            historico.WithOwner().HasForeignKey(h => h.SinistroId);

            historico.HasKey(h => h.Id);

            historico.Property(h => h.DataAlteracao).IsRequired();

            historico.Property(h => h.StatusAnterior)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            historico.Property(h => h.StatusNovo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
        });
    }
}
