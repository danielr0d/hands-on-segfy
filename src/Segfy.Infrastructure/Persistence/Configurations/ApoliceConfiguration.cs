using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Segfy.Domain.Entities;

namespace Segfy.Infrastructure.Persistence.Configurations;

public class ApoliceConfiguration : IEntityTypeConfiguration<Apolice>
{
    public void Configure(EntityTypeBuilder<Apolice> builder)
    {
        builder.ToTable("Apolices");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Numero)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(a => a.Numero).IsUnique();

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne<Cliente>()
            .WithMany()
            .HasForeignKey(a => a.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Ramo>()
            .WithMany()
            .HasForeignKey(a => a.RamoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
