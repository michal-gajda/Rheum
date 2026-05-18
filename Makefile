build:
	dotnet restore
	dotnet build --no-restore

run:
	dotnet run --project src/WebApi/Rheum.WebApi.csproj

docker:
	docker compose build

up: docker
	docker compose up --detach

down:
	docker compose down
