build:
	dotnet restore
	dotnet build --no-restore

run:
	dotnet run --project Rheum.WebApi/Rheum.WebApi.csproj

docker-build:
	docker compose build

up: docker-build
	docker compose up --detach

down:
	docker compose down
