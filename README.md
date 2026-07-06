# Segfy - API de Gestão de Sinistros

---

## Tecnologias utilizadas

* **Framework:** C# / .NET 9.
* **Banco de dados:** PostgreSQL.
* **Arquitetura:** Clean Architecture, CQRS (MediatR), seguindo os princípios SOLID.
* **Testes:** xUnit, Moq e FluentAssertions.
* **Containerização:** Docker e Docker Compose pra facilitar a execução da aplicação com o banco de dados.
---

## Como executar projeto via CLI

No dir raiz do projeto, execute:

```bash
dotnet run --project src/Segfy.Api
```

## Como executar (via Docker)

No dir raiz do projeto, execute:

```bash
docker compose up -d --build
```

## Como executar os testes

```bash
dotnet test
```

