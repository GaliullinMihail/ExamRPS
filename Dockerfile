FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RPS.API/RPS.API.csproj", "RPS.API/"]
COPY ["RPS.Infrastructure/RPS.Infrastructure.csproj", "RPS.Infrastructure/"]
COPY ["RPS.Domain/RPS.Domain.csproj", "RPS.Domain/"]
COPY ["RPS.Application/RPS.Application.csproj", "RPS.Application/"]
RUN dotnet restore "RPS.API/RPS.API.csproj"
COPY . .
WORKDIR "/src/RPS.API"
RUN dotnet build "RPS.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RPS.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RPS.API.dll"]
