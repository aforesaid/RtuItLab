#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 7004

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Factories.API/Factories.API.csproj", "Factories.API/"]
COPY ["RtuItLab.Infrastructure/RtuItLab.Infrastructure.csproj", "RtuItLab.Infrastructure/"]
RUN dotnet restore "Factories.API/Factories.API.csproj"
COPY . .
WORKDIR "/src/Factories.API"
RUN dotnet build "Factories.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Factories.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Factories.API.dll"]