#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5672
EXPOSE 15672

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RabbitMQ/RabbitMQ.csproj", "RabbitMQ/"]
COPY ["Shops.Domain/Shops.Domain.csproj", "Shops.Domain/"]
COPY ["Shops.DAL/Shops.DAL.csproj", "Shops.DAL/"]
COPY ["RtuItLab.Infrastructure/RtuItLab.Infrastructure.csproj", "RtuItLab.Infrastructure/"]
COPY ["Identity.Domain/Identity.Domain.csproj", "Identity.Domain/"]
COPY ["Identity.DAL/Identity.DAL.csproj", "Identity.DAL/"]
COPY ["Purchases.Domain/Purchases.Domain.csproj", "Purchases.Domain/"]
COPY ["Purchases.DAL/Purchases.DAL.csproj", "Purchases.DAL/"]
RUN dotnet restore "RabbitMQ/RabbitMQ.csproj"
COPY . .
WORKDIR "/src/RabbitMQ"
RUN dotnet build "RabbitMQ.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMQ.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQ.dll"]