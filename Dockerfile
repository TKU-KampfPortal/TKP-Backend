# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TKP.Server.WebApi/TKP.Server.WebApi.csproj", "src/TKP.Server.WebApi/"]
COPY ["src/TKP.Server.Infrastructure/TKP.Server.Infrastructure.csproj", "src/TKP.Server.Infrastructure/"]
COPY ["src/TKP.Server.Application/TKP.Server.Application.csproj", "src/TKP.Server.Application/"]
COPY ["src/TKP.Server.Domain/TKP.Server.Domain.csproj", "src/TKP.Server.Domain/"]
COPY ["src/TKP.Server.Core/TKP.Server.Core.csproj", "src/TKP.Server.Core/"]
RUN dotnet restore "./src/TKP.Server.WebApi/TKP.Server.WebApi.csproj"


# Copy the remaining source code and build
COPY src/ /src/
WORKDIR "/src/TKP.Server.WebApi/"
RUN dotnet build "TKP.Server.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
RUN dotnet publish "TKP.Server.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy SSL certificate if Production
# ARG ENVIRONMENT=Development
# COPY ["src/TKP.Server.WebAPI/tkp_server.pfx", "/app/tkp_server.pfx"]
# RUN if [ "$ENVIRONMENT" = "Production" ]; then cp /app/tkp_server.pfx /app/tkp_server.pfx; fi

ENTRYPOINT ["dotnet", "TKP.Server.WebApi.dll"]
