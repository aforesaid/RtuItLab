#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 7002

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Purchases/Purchases.API/Purchases.API.csproj", "Services/Purchases/Purchases.API/"]
COPY ["RtuItLab.Infrastructure/RtuItLab.Infrastructure.csproj", "RtuItLab.Infrastructure/"]
RUN dotnet restore "Services/Purchases/Purchases.API/Purchases.API.csproj"
COPY . .
WORKDIR "/src/Services/Purchases/Purchases.API"
RUN dotnet build "Purchases.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Purchases.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Purchases.API.dll"]