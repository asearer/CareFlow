FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/CareFlow.Domain/CareFlow.Domain.csproj src/CareFlow.Domain/
COPY src/CareFlow.Application/CareFlow.Application.csproj src/CareFlow.Application/
COPY src/CareFlow.Infrastructure/CareFlow.Infrastructure.csproj src/CareFlow.Infrastructure/
COPY src/CareFlow.Api/CareFlow.Api.csproj src/CareFlow.Api/
RUN dotnet restore src/CareFlow.Api/CareFlow.Api.csproj

# Copy everything else and build
COPY . .
WORKDIR /app/src/CareFlow.Api
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CareFlow.Api.dll"]
