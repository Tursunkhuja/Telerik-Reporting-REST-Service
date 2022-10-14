#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TelerikReportingRestService.csproj", "."]
RUN dotnet restore "./TelerikReportingRestService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TelerikReportingRestService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelerikReportingRestService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
# install System.Drawing native dependencies
WORKDIR /app

COPY --from=publish /app/publish .
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*

ENTRYPOINT ["dotnet", "TelerikReportingRestService.dll"]
