# Rheum

```powershell
dotnet new webapi --framework net10.0 --no-https --use-program-main --output src/WebApi --use-controllers --name Rheum.WebApi
dotnet new classlib --framework net10.0 --output src/Infrastructure --name Rheum.Infrastructure
dotnet add src/WebApi reference src/Infrastructure
dotnet new classlib --framework net10.0 --output src/Application --name Rheum.Application
dotnet add src/Infrastructure reference src/Application
dotnet new classlib --framework net10.0 --output src/Domain --name Rheum.Domain
dotnet add src/Application reference src/Domain
dotnet sln add src/WebApi
```

```powershell
dotnet add src/Application package Microsoft.Extensions.Logging.Abstractions
dotnet add src/Application package Rebus.ServiceProvider
```
