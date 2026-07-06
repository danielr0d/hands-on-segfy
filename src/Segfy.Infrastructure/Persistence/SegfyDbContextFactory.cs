using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Segfy.Infrastructure.Persistence;

// Design-time only: lets `dotnet ef migrations` build the model without an API host project; runtime uses DependencyInjection's configuration-based connection string instead.
public class SegfyDbContextFactory : IDesignTimeDbContextFactory<SegfyDbContext>
{
    public SegfyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SegfyDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=segfy;Username=postgres;Password=postgres");

        return new SegfyDbContext(optionsBuilder.Options);
    }
}
