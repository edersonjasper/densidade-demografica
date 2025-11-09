FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app

# Exp�e as portas que a aplica��o usar�
# 8080: HTTP (porta padr�o do ASP.NET Core 8 sem HTTPS)
# 8081: HTTPS
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["src/densidade-demografica.API/densidade-demografica.API.csproj", "src/densidade-demografica.API/"]

RUN dotnet restore "src/densidade-demografica.API/densidade-demografica.API.csproj"

COPY . .

WORKDIR "/src/src/densidade-demografica.API"

RUN dotnet build "densidade-demografica.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build


FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "densidade-demografica.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


FROM base AS final

USER $APP_UID

WORKDIR /app

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "densidade-demografica.API.dll"]

# ============================================
# RESUMO DOS STAGES:
# 1. BASE: Runtime .NET 8 (aspnet)
# 2. BUILD: Compila usando SDK .NET 8
# 3. PUBLISH: Prepara arquivos otimizados
# 4. FINAL: Copia apenas bin�rios necess�rios
#
# RESULTADO: Imagem final pequena e segura!
# ============================================