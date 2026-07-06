using FluentAssertions;
using Moq;
using Segfy.Application.Common.Interfaces;
using Segfy.Application.Sinistros.Queries.ListarSinistros;
using Segfy.Domain.Entities;
using Segfy.Domain.Enums;

namespace Segfy.Application.Tests.Sinistros.Queries;

public class ListarSinistrosQueryHandlerTests
{
    private readonly Mock<ISinistroRepository> _sinistroRepository = new();
    private readonly ListarSinistrosQueryHandler _handler;

    public ListarSinistrosQueryHandlerTests()
    {
        _handler = new ListarSinistrosQueryHandler(_sinistroRepository.Object);
    }

    [Fact]
    public async Task Handle_DeveRepassarFiltrosEMapearResultadoPaginado()
    {
        var itens = new List<Sinistro>
        {
            Sinistro.Abrir(new Apolice("AP-0001", Guid.NewGuid(), Guid.NewGuid()), 1000m),
            Sinistro.Abrir(new Apolice("AP-0002", Guid.NewGuid(), Guid.NewGuid()), 2000m)
        };

        var query = new ListarSinistrosQuery(SinistroStatus.Aberto, DateTime.UtcNow.AddDays(-30), DateTime.UtcNow, 2, 10);

        _sinistroRepository
            .Setup(r => r.ListarAsync(
                query.Status, query.DataInicio, query.DataFim, query.Pagina, query.TamanhoPagina,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((itens, TotalRegistros: 25));

        var resultado = await _handler.Handle(query, CancellationToken.None);

        resultado.Itens.Should().HaveCount(2);
        resultado.TotalRegistros.Should().Be(25);
        resultado.Pagina.Should().Be(2);
        resultado.TamanhoPagina.Should().Be(10);
        resultado.TotalPaginas.Should().Be(3);
    }
}
