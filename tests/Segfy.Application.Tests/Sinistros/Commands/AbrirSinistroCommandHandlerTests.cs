using FluentAssertions;
using Moq;
using Segfy.Application.Common.Exceptions;
using Segfy.Application.Common.Interfaces;
using Segfy.Application.Sinistros.Commands.AbrirSinistro;
using Segfy.Domain.Entities;
using Segfy.Domain.Enums;
using Segfy.Domain.Exceptions;

namespace Segfy.Application.Tests.Sinistros.Commands;

public class AbrirSinistroCommandHandlerTests
{
    private readonly Mock<IApoliceRepository> _apoliceRepository = new();
    private readonly Mock<ISinistroRepository> _sinistroRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly AbrirSinistroCommandHandler _handler;

    public AbrirSinistroCommandHandlerTests()
    {
        _handler = new AbrirSinistroCommandHandler(
            _apoliceRepository.Object, _sinistroRepository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_QuandoApoliceNaoExiste_DeveLancarNotFoundException()
    {
        var command = new AbrirSinistroCommand(Guid.NewGuid(), 1000m);

        _apoliceRepository
            .Setup(r => r.ObterPorIdAsync(command.ApoliceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Apolice?)null);

        var acao = () => _handler.Handle(command, CancellationToken.None);

        await acao.Should().ThrowAsync<NotFoundException>();
        _sinistroRepository.Verify(
            r => r.AdicionarAsync(It.IsAny<Sinistro>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_QuandoApoliceNaoEstaAtiva_DeveLancarApoliceInativaException()
    {
        var apolice = new Apolice("AP-0001", Guid.NewGuid(), Guid.NewGuid());
        apolice.Inativar();

        var command = new AbrirSinistroCommand(apolice.Id, 1000m);

        _apoliceRepository
            .Setup(r => r.ObterPorIdAsync(command.ApoliceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apolice);

        var acao = () => _handler.Handle(command, CancellationToken.None);

        await acao.Should().ThrowAsync<ApoliceInativaException>();
        _unitOfWork.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_QuandoApoliceAtiva_DeveCriarSinistroEPersistirAlteracoes()
    {
        var apolice = new Apolice("AP-0001", Guid.NewGuid(), Guid.NewGuid());
        var command = new AbrirSinistroCommand(apolice.Id, 2500m);

        _apoliceRepository
            .Setup(r => r.ObterPorIdAsync(command.ApoliceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apolice);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        resultado.ApoliceId.Should().Be(apolice.Id);
        resultado.Status.Should().Be(SinistroStatus.Aberto);
        resultado.ValorEstimado.Should().Be(2500m);

        _sinistroRepository.Verify(
            r => r.AdicionarAsync(It.Is<Sinistro>(s => s.ApoliceId == apolice.Id), It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWork.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
