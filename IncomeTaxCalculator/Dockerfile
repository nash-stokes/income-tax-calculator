FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IncomeTaxCalculator/IncomeTaxCalculator.csproj", "IncomeTaxCalculator/"]
RUN dotnet restore "IncomeTaxCalculator/IncomeTaxCalculator.csproj"
COPY . .
WORKDIR "/src/IncomeTaxCalculator"
RUN dotnet build "./IncomeTaxCalculator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IncomeTaxCalculator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IncomeTaxCalculator.dll"]
