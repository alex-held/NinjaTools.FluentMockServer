FROM  mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 1080

ENV ASPNETCORE_URLS="http://+:80;http://+:1080"

FROM  mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

COPY . .

WORKDIR /src/NinjaTools.FluentMockServer.API

RUN dotnet restore  -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NinjaTools.FluentMockServer.API.dll"]
