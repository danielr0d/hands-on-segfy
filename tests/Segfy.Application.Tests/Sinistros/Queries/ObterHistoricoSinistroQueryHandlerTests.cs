using FluentAssertions;
using Moq;
using Segfy.Application.Common.Exceptions;
using Segfy.Application.Common.Interfaces;
using Segfy.Application.Sinistros.Queries.ObterHistoricoSinistro;
using Segfy.Domain.Entities;
using Segfy.Domain.Enums;

namespace Segfy.Application.Tests.Sinistros.Queries;

public class ObterHistoricoSinistroQueryHandlerTests
{
    private readonly Mock<ISinistroRepository> _sinistroRepository = new();
    private readonly ObterHistoricoSinistroQueryHandler _handler;

    public ObterHistoricoSinistroQueryHandlerTests()
    {
        _handler = new ObterHistoricoSinistroQueryHandler(_sinistroRepository.Object);
    }

    [Fact]
    public async Task Handle_QuandoSinistroExiste_DeveRetornarHistoricoMapeado()
    {
        var sinistro = Sinistro.Abrir(new Apolice("AP-0001", Guid.NewGuid(), Guid.NewGuid()), 1000m);
        sinistro.AlterarStatus(SinistroStatus.EmAnalise);
        sinistro.AlterarStatus(SinistroStatus.Aprovado, valorAprovado: 800m);

        _sinistroRepository
            .Setup(r => r.ObterComHistoricoPorIdAsync(sinistro.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(sinistro);

        var resultado = await _handler.Handle(
            new ObterHistoricoSinistroQuery(sinistro.Id), CancellationToken.None);

        resultado.Should().HaveCount(2);
        resultado.Should().Contain(h =>
            h.StatusAnterior == SinistroStatus.Aberto && h.StatusNovo == SinistroStatus.EmAnalise);
        resultado.Should().Contain(h =>
            h.StatusAnterior == SinistroStatus.EmAnalise && h.StatusNovo == SinistroStatus.Aprovado);
    }

    [Fact]
    public async Task Handle_QuandoSinistroNaoExiste_DeveLancarNotFoundException()
    {
        var id = Guid.NewGuid();

        _sinistroRepository
            .Setup(r => r.ObterComHistoricoPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Sinistro?)null);

        var acao = () => _handler.Handle(new ObterHistoricoSinistroQuery(id), CancellationToken.None);

        await acao.Should().ThrowAsync<NotFoundException>();
    }
}
