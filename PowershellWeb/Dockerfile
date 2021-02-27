#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
RUN apt-get update && \
    apt-get install -y \
    wget
RUN wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb 

RUN apt-get update && \
    apt-get install -y \
    powershell

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["PowershellWeb/PowershellWeb.csproj", "PowershellWeb/"]
RUN dotnet restore "PowershellWeb/PowershellWeb.csproj"
COPY . .
WORKDIR "/src/PowershellWeb"
RUN dotnet build "PowershellWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PowershellWeb.csproj" -c Release -o /app/publish

FROM base AS final
ENV ServerKey test
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PowershellWeb.dll"]
