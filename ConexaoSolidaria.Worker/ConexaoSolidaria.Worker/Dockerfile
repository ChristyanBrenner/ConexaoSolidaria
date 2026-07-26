FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["ConexaoSolidaria.Worker/ConexaoSolidaria.Worker.csproj", "ConexaoSolidaria.Worker/"]

RUN dotnet restore "ConexaoSolidaria.Worker/ConexaoSolidaria.Worker.csproj"

COPY . .

WORKDIR "/src/ConexaoSolidaria.Worker"

RUN dotnet build "ConexaoSolidaria.Worker.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "ConexaoSolidaria.Worker.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ConexaoSolidaria.Worker.dll"]