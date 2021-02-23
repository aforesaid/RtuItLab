#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 7003
COPY . .
RUN dotnet build "Shops.API/Shops.API.csproj"
RUN dotnet publish "Shops.API.csproj" -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app
COPY --from=base /app/out .
ENTRYPOINT ["dotnet", "Shops.API.dll"]
