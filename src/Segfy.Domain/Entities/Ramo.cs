using Segfy.Domain.Common;
using Segfy.Domain.Exceptions;

namespace Segfy.Domain.Entities;

public class Ramo : BaseEntity
{
    public string Nome { get; private set; } = null!;

    protected Ramo()
    {
    }

    public Ramo(string nome)
    {
        SetNome(nome);
    }

    public void AlterarNome(string nome) => SetNome(nome);

    private void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new CampoObrigatorioException(nameof(Nome));

        Nome = nome;
    }
}
