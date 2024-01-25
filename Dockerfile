FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RPC.API/RPC.API.csproj", "RPC.API/"]
COPY ["RPC.Infrastructure/RPC.Infrastructure.csproj", "RPC.Infrastructure/"]
COPY ["RPC.Domain/RPC.Domain.csproj", "RPC.Domain/"]
COPY ["RPC.Application/RPC.Application.csproj", "RPC.Application/"]
RUN dotnet restore "RPC.API/RPC.API.csproj"
COPY . .
WORKDIR "/src/RPC.API"
RUN dotnet build "RPC.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RPC.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RPC.API.dll"]
