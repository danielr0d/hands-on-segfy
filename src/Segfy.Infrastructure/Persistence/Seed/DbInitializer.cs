using Microsoft.EntityFrameworkCore;
using Segfy.Domain.Entities;

namespace Segfy.Infrastructure.Persistence.Seed;

public static class DbInitializer
{
    public static async Task SeedAsync(SegfyDbContext context)
    {
        if (await context.Clientes.AnyAsync())
            return;

        var clientes = new[]
        {
            new Cliente("Ana Beatriz Souza"),
            new Cliente("Carlos Eduardo Lima"),
            new Cliente("Fernanda Torres Costa")
        };

        var ramos = new[]
        {
            new Ramo("Automóvel"),
            new Ramo("Residencial"),
            new Ramo("Saúde")
        };

        await context.Clientes.AddRangeAsync(clientes);
        await context.Ramos.AddRangeAsync(ramos);

        var apolices = new[]
        {
            new Apolice("AP-0001", clientes[0].Id, ramos[0].Id),
            new Apolice("AP-0002", clientes[1].Id, ramos[1].Id),
            new Apolice("AP-0003", clientes[2].Id, ramos[2].Id)
        };

        apolices[2].Inativar();

        await context.Apolices.AddRangeAsync(apolices);

        await context.SaveChangesAsync();
    }
}
