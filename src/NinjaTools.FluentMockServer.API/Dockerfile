FROM  mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 2081
EXPOSE 2080

ENV ASPNETCORE_URLS="http://+:2080;http://+:2081"

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
