FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 5001

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY LagencyUser.sln ./
COPY src/LagencyUser.Web/LagencyUser.Web.csproj src/LagencyUser.Web/
COPY src/LagencyUser.Infrastructure/LagencyUser.Infrastructure.csproj src/LagencyUser.Infrastructure/
COPY src/LagencyUser.Application/LagencyUser.Application.csproj src/LagencyUser.Application/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/src/LagencyUser.Web
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LagencyUser.Web.dll"]
