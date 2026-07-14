FROM node:20-alpine AS frontend-build
WORKDIR /frontend
COPY frontend/package*.json ./
RUN npm ci
COPY frontend/ ./
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/API/InventoryManagement.API.csproj", "src/API/"]
COPY ["src/Application/InventoryManagement.Application.csproj", "src/Application/"]
COPY ["src/Domain/InventoryManagement.Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/InventoryManagement.Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/API/InventoryManagement.API.csproj"

COPY . .
WORKDIR "/src/src/API"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .
COPY --from=frontend-build /frontend/dist ./wwwroot

ENTRYPOINT ["dotnet", "InventoryManagement.API.dll"]
