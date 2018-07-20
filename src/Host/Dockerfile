FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 5010

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY IdentityServer4.MongoDB.sln ./
COPY src/Host/Host.csproj src/Host/
COPY src/IdentityServer4.MongoDB/IdentityServer4.MongoDB.csproj src/IdentityServer4.MongoDB/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/src/Host
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Host.dll"]
