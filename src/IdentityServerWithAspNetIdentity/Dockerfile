FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 5011

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Quickstart6_AspNetIdentity.sln ./
COPY src/IdentityServerWithAspNetIdentity/IdentityServerWithAspNetIdentity.csproj src/IdentityServerWithAspNetIdentity/
COPY src/IdentityServer4.MongoDB/IdentityServer4.MongoDB.csproj src/IdentityServer4.MongoDB/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/src/IdentityServerWithAspNetIdentity
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IdentityServerWithAspNetIdentity.dll"]
