FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Segfy.Domain/Segfy.Domain.csproj", "src/Segfy.Domain/"]
COPY ["src/Segfy.Application/Segfy.Application.csproj", "src/Segfy.Application/"]
COPY ["src/Segfy.Infrastructure/Segfy.Infrastructure.csproj", "src/Segfy.Infrastructure/"]
COPY ["src/Segfy.Api/Segfy.Api.csproj", "src/Segfy.Api/"]
RUN dotnet restore "src/Segfy.Api/Segfy.Api.csproj"

COPY src/ src/
RUN dotnet publish "src/Segfy.Api/Segfy.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Segfy.Api.dll"]
