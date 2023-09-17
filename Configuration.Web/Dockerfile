#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Configuration.Web/Configuration.Web.csproj", "Configuration.Web/"]
COPY ["Configuration.Common/Configuration.Common.csproj", "Configuration.Common/"]
COPY ["Configuration.Core/Configuration.Core.csproj", "Configuration.Core/"]
COPY ["Configuration.Domain/Configuration.Domain.csproj", "Configuration.Domain/"]
COPY ["ConfigurationReader/ConfigurationReader.csproj", "ConfigurationReader/"]
RUN dotnet restore "Configuration.Web/Configuration.Web.csproj"
COPY . .
WORKDIR "/src/Configuration.Web"
RUN dotnet build "Configuration.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Configuration.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Configuration.Web.dll"]