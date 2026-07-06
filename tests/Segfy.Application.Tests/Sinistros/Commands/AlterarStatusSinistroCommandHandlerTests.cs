using FluentAssertions;
using Moq;
using Segfy.Application.Common.Exceptions;
using Segfy.Application.Common.Interfaces;
using Segfy.Application.Sinistros.Commands.AlterarStatusSinistro;
using Segfy.Domain.Entities;
using Segfy.Domain.Enums;
using Segfy.Domain.Exceptions;

namespace Segfy.Application.Tests.Sinistros.Commands;

public class AlterarStatusSinistroCommandHandlerTests
{
    private readonly Mock<ISinistroRepository> _sinistroRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly AlterarStatusSinistroCommandHandler _handler;

    public AlterarStatusSinistroCommandHandlerTests()
    {
        _handler = new AlterarStatusSinistroCommandHandler(_sinistroRepository.Object, _unitOfWork.Object);
    }

    private static Sinistro CriarSinistroAberto() =>
        Sinistro.Abrir(new Apolice("AP-0001", Guid.NewGuid(), Guid.NewGuid()), 1000m);

    [Fact]
    public async Task Handle_QuandoSinistroNaoExiste_DeveLancarNotFoundException()
    {
        var command = new AlterarStatusSinistroCommand(Guid.NewGuid(), SinistroStatus.EmAnalise, null, null);

        _sinistroRepository
            .Setup(r => r.ObterPorIdAsync(command.SinistroId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Sinistro?)null);

        var acao = () => _handler.Handle(command, CancellationToken.None);

        await acao.Should().ThrowAsync<NotFoundException>();
        _unitOfWork.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_QuandoTransicaoValida_DeveAlterarStatusEPersistirAlteracoes()
    {
        var sinistro = CriarSinistroAberto();
        var command = new AlterarStatusSinistroCommand(sinistro.Id, SinistroStatus.EmAnalise, null, null);

        _sinistroRepository
            .Setup(r => r.ObterPorIdAsync(command.SinistroId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(sinistro);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.Status.Should().Be(SinistroStatus.EmAnalise);
        _unitOfWork.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_QuandoTransicaoInvalida_DeveLancarExcecaoDeDominioENaoPersistir()
    {
        var sinistro = CriarSinistroAberto();
        var command = new AlterarStatusSinistroCommand(sinistro.Id, SinistroStatus.Aprovado, 900m, null);

        _sinistroRepository
            .Setup(r => r.ObterPorIdAsync(command.SinistroId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(sinistro);

        var acao = () => _handler.Handle(command, CancellationToken.None);

        await acao.Should().ThrowAsync<TransicaoStatusInvalidaException>();
        _unitOfWork.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ParaNegado_SemMotivo_DeveLancarMotivoNegativaObrigatorioExceptionENaoPersistir()
    {
        var sinistro = CriarSinistroAberto();
        sinistro.AlterarStatus(SinistroStatus.EmAnalise);
        var command = new AlterarStatusSinistroCommand(sinistro.Id, SinistroStatus.Negado, null, null);

        _sinistroRepository
            .Setup(r => r.ObterPorIdAsync(command.SinistroId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(sinistro);

        var acao = () => _handler.Handle(command, CancellationToken.None);

        await acao.Should().ThrowAsync<MotivoNegativaObrigatorioException>();
        _unitOfWork.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
