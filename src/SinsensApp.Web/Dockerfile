#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
#COPY ["nuget.config", "."]
COPY ["NuGet.Config", "."]
COPY ["src/SinsensApp.Web/SinsensApp.Web.csproj", "src/SinsensApp.Web/"]
COPY ["src/SinsensApp.Application/SinsensApp.Application.csproj", "src/SinsensApp.Application/"]
COPY ["src/SinsensApp.EntityFrameworkCore/SinsensApp.EntityFrameworkCore.csproj", "src/SinsensApp.EntityFrameworkCore/"]
COPY ["src/SinsensApp.Domain/SinsensApp.Domain.csproj", "src/SinsensApp.Domain/"]
COPY ["src/SinsensApp.Domain.Shared/SinsensApp.Domain.Shared.csproj", "src/SinsensApp.Domain.Shared/"]
COPY ["src/SinsensApp.Application.Contracts/SinsensApp.Application.Contracts.csproj", "src/SinsensApp.Application.Contracts/"]
COPY ["src/SinsensApp.EntityFrameworkCore.DbMigrations/SinsensApp.EntityFrameworkCore.DbMigrations.csproj", "src/SinsensApp.EntityFrameworkCore.DbMigrations/"]
COPY ["src/SinsensApp.HttpApi/SinsensApp.HttpApi.csproj", "src/SinsensApp.HttpApi/"]
RUN dotnet restore "src/SinsensApp.Web/SinsensApp.Web.csproj"
COPY . .
WORKDIR "/src/src/SinsensApp.Web"
RUN dotnet build "SinsensApp.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SinsensApp.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SinsensApp.Web.dll"]