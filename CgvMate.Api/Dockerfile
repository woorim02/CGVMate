#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ARG DEBIAN_FRONTEND=noninteractive
RUN apt-get install -y tzdata

RUN ln -sf /usr/share/zoneinfo/Asia/Seoul /etc/localtime

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CgvMate.Api/CgvMate.Api.csproj", "CgvMate.Api/"]
RUN dotnet restore "./CgvMate.Api/CgvMate.Api.csproj"
COPY . .
WORKDIR "/src/CgvMate.Api"
RUN dotnet build "./CgvMate.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CgvMate.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CgvMate.Api.dll"]
