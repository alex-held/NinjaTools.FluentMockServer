FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

MAINTAINER Alexander Held
LABEL swaggerVersion="V1.0"
LABEL applicationVersion="0.1.0"
LABEL applicationName="MockServer"
LABEL maintainer="Alexander Held"
LABEL repo="https://github.com/alex-held/NinjaTools.FluentMockServer"
LABEL contact="contact@alexanderheld.net"
LABEL healthCheck="healthz"
LABEL swagger="swagger"


ENV MOCKSERVER__APPLICATION_NAME="MockServer"
ENV MOCKSERVER__APPLICATION_VERSION="0.1.0"
ENV MOCKSERVER__HEALTH_CHECK_PATH="healthz"
ENV MOCKSERVER__SWAGGER_PATH="swagger"
ENV MOCKSERVER__SWAGGER_VERSION="V1.0"

ENV MOCKSERVER__PATHS__CONFIG_BASE_PATH="/etc/mock-server"
ENV MOCKSERVER__PATHS__CONFIG_FILE_PATH="/etc/mock-server/config"
ENV MOCKSERVER__PATHS__LOG_PATH="/var/log/mock-server"

ENV ASPNETCORE_ENVIRONMENT="Release"
ENV ASPNETCORE_URLS="http://+:80;http://+:1080"
ENV MOCKSERVER__RUNNING_IN_CONTAINER="yes"
ENV MOCKSERVER__ADMIN_PORT="1080"
ENV MOCKSERVER__MOCK_PORT="80"

EXPOSE 80
EXPOSE 1080

RUN mkdir $MOCKSERVER__PATHS__CONFIG_BASE_PATH
RUN mkdir $MOCKSERVER__PATHS__CONFIG_FILE_PATH
RUN mkdir $MOCKSERVER__PATHS__LOG_PATH
RUN chmod -R 777 $MOCKSERVER__PATHS__CONFIG_BASE_PATH
RUN chmod -R 777 $MOCKSERVER__PATHS__LOG_PATH


# HEALTHCHECK --interval=15m --timeout=3s \
#   CMD curl -f http://localhost:${MOCKSERVER__ADMIN_PORT}/${MOCKSERVER__HEALTH_CHECK_PATH} || exit 1


FROM  mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

COPY . .

WORKDIR src/NinjaTools.FluentMockServer.API

RUN dotnet restore  -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release  -f netcoreapp3.1 -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "NinjaTools.FluentMockServer.API.dll"]
