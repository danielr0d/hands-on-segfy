using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Segfy.Domain.Entities;

namespace Segfy.Infrastructure.Persistence.Configurations;

public class RamoConfiguration : IEntityTypeConfiguration<Ramo>
{
    public void Configure(EntityTypeBuilder<Ramo> builder)
    {
        builder.ToTable("Ramos");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Nome)
            .IsRequired()
            .HasMaxLength(150);
    }
}
