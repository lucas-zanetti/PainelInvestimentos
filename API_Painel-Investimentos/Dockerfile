FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY cert.pfx cert.pfx

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API_Painel-Investimentos.csproj", "."]
RUN dotnet restore "./API_Painel-Investimentos.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./API_Painel-Investimentos.csproj" -c $BUILD_CONFIGURATION -o /app/build
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API_Painel-Investimentos.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN mkdir SQLite
RUN chmod 777 SQLite 
COPY --from=publish /app/publish .

ENV Kestrel__Certificates__Default__Path=/app/cert.pfx
ENV Kestrel__Certificates__Default__Password=1234

ENTRYPOINT ["dotnet", "API_Painel-Investimentos.dll"]