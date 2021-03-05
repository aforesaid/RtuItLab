#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 15672
EXPOSE 5672

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RabbitMQ/RabbitMQ.csproj", "RabbitMQ/"]
COPY ["Services/Identity/Identity.Domain/Identity.Domain.csproj", "Services/Identity/Identity.Domain/"]
COPY ["Services/Identity/Identity.DAL/Identity.DAL.csproj", "Services/Identity/Identity.DAL/"]
COPY ["RtuItLab.Infrastructure/RtuItLab.Infrastructure.csproj", "RtuItLab.Infrastructure/"]
COPY ["Services/Purchases/Purchases.Domain/Purchases.Domain.csproj", "Services/Purchases/Purchases.Domain/"]
COPY ["Services/Purchases/Purchases.DAL/Purchases.DAL.csproj", "Services/Purchases/Purchases.DAL/"]
COPY ["Services/Shops/Shops.Domain/Shops.Domain.csproj", "Services/Shops/Shops.Domain/"]
COPY ["Services/Shops/Shops.DAL/Shops.DAL.csproj", "Services/Shops/Shops.DAL/"]
COPY ["Services/Factories/Factories.Domain/Factories.Domain.csproj", "Services/Factories/Factories.Domain/"]
COPY ["Services/Factories/Factories.DAL/Factories.DAL.csproj", "Services/Factories/Factories.DAL/"]
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