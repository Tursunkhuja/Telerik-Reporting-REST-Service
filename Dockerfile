#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
# Depenencies for Telerik Reporting on Linux
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG TELERIK_USERNAME=%TELERIK_USERNAME%
ARG TELERIK_PASSWORD=%TELERIK_PASSWORD%
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["TelerikReportingRestService.csproj", "."]
RUN dotnet restore "./TelerikReportingRestService.csproj" --configfile "./NuGet.Config"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TelerikReportingRestService.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "TelerikReportingRestService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
# install System.Drawing native dependencies
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelerikReportingRestService.dll"]
