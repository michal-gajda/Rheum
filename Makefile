build:
	dotnet restore
	dotnet build --no-restore

run:
	dotnet run --project src/WebApi/Rheum.WebApi.csproj

image:
	docker compose build

push: image
	docker push gajdaltd/rheum:latest

up: image
	docker compose down
	docker compose up --detach

down:
	docker compose down
