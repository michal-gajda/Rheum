FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Domain/Rheum.Domain.csproj Domain/Rheum.Domain.csproj
COPY src/Application/Rheum.Application.csproj Application/Rheum.Application.csproj
COPY src/Infrastructure/Rheum.Infrastructure.csproj Infrastructure/Rheum.Infrastructure.csproj
COPY src/Shared/Rheum.Shared.csproj Shared/Rheum.Shared.csproj
COPY src/WebApi/Rheum.WebApi.csproj WebApi/Rheum.WebApi.csproj
RUN dotnet new sln -n Rheum
RUN dotnet sln add WebApi/Rheum.WebApi.csproj
RUN dotnet restore

COPY src/ .
COPY Directory.Build.props .
RUN dotnet publish WebApi/Rheum.WebApi.csproj --configuration Release --no-restore --output /app/build

FROM mcr.microsoft.com/dotnet/aspnet:10.0-azurelinux3.0
WORKDIR /app

RUN tdnf install -y shadow-utils; groupadd -g 10000 dotnet; useradd -u 10000 -g 10000 -s /sbin/nologin -M dotnet; tdnf clean all;
USER dotnet:dotnet

COPY --chown=dotnet:dotnet --from=build /app/build .

ENV ASPNETCORE_HTTP_PORTS=5080
EXPOSE 5080

HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl --fail http://localhost:5080/healthz || exit 1

ENTRYPOINT ["dotnet", "Rheum.WebApi.dll"]
