using Microsoft.EntityFrameworkCore;
using Segfy.Domain.Entities;

namespace Segfy.Infrastructure.Persistence;

public class SegfyDbContext : DbContext
{
    public SegfyDbContext(DbContextOptions<SegfyDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Ramo> Ramos => Set<Ramo>();
    public DbSet<Apolice> Apolices => Set<Apolice>();
    public DbSet<Sinistro> Sinistros => Set<Sinistro>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SegfyDbContext).Assembly);
    }
}
