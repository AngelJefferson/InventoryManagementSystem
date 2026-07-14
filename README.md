# Inventory Management System — API REST

Clean Architecture · ASP.NET Core Web API 9 · C# · SQL Server · MongoDB · JWT · Docker

## Arquitectura

```
┌──────────────────────────────────┐
│            API (Controllers)     │  ← Capa de presentación
├──────────────────────────────────┤
│         Application (CQRS)       │  ← Casos de uso, DTOs, Validación
├──────────────────────────────────┤
│         Infrastructure           │  ← EF Core, SQL Server, MongoDB
├──────────────────────────────────┤
│            Domain                │  ← Entidades, Value Objects, Interfaces
└──────────────────────────────────┘
```

### Capas

| Capa | Proyecto | Responsabilidad |
|------|----------|----------------|
| **Domain** | `InventoryManagement.Domain` | Entidades (`Product`, `Category`, `Supplier`, `Inventory`, `StockMovement`), Value Objects (`Money`), Excepciones, Interfaces de repositorio |
| **Application** | `InventoryManagement.Application` | CQRS con MediatR (Commands/Queries/Handlers), FluentValidation, DTOs, AutoMapper, Pipeline Behaviors (Validation + AuditLog) |
| **Infrastructure** | `InventoryManagement.Infrastructure` | EF Core `ApplicationDbContext` con Fluent API, repositorios, `MongoAuditLogService` |
| **API** | `InventoryManagement.API` | Controllers REST, JWT Auth, Swagger, Exception Handling Middleware |

## Diagrama ER

```
Category (1) ──< (N) Product (N) >── (N) Supplier
                        │
                        │ (1)
                        │
                    Inventory (1)
                        │ (N)
                        │
                  StockMovement
```

## Endpoints

### Auth
| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `POST` | `/api/v1/auth/login` | Genera JWT | No |
| `GET` | `/api/v1/auth/me` | Perfil del usuario autenticado | JWT |

### Category
| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `GET` | `/api/v1/categories` | Listar todas | No |
| `GET` | `/api/v1/categories/{id}` | Obtener por Id | No |
| `POST` | `/api/v1/categories` | Crear | JWT (Admin) |
| `PUT` | `/api/v1/categories/{id}` | Actualizar | JWT (Admin) |
| `DELETE` | `/api/v1/categories/{id}` | Eliminar | JWT (Admin) |

### Product
| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `GET` | `/api/v1/products` | Listar (filtro por categoría, búsqueda) | No |
| `GET` | `/api/v1/products/{id}` | Obtener por Id | No |
| `POST` | `/api/v1/products` | Crear | JWT (Admin) |
| `PUT` | `/api/v1/products/{id}` | Actualizar | JWT (Admin) |
| `DELETE` | `/api/v1/products/{id}` | Eliminar | JWT (Admin) |
| `GET` | `/api/v1/products/{id}/stock` | Stock actual del producto | No |
| `GET` | `/api/v1/products/low-stock?threshold=5` | Productos con bajo stock | JWT |

### Supplier
| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `GET` | `/api/v1/suppliers` | Listar todos | No |
| `GET` | `/api/v1/suppliers/{id}` | Obtener por Id | No |
| `POST` | `/api/v1/suppliers` | Crear | JWT (Admin) |
| `PUT` | `/api/v1/suppliers/{id}` | Actualizar | JWT (Admin) |
| `DELETE` | `/api/v1/suppliers/{id}` | Eliminar | JWT (Admin) |

### Inventory
| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `GET` | `/api/v1/inventory/{productId}` | Stock de un producto | No |
| `POST` | `/api/v1/inventory/adjust` | Ajustar stock (entrada/salida) | JWT |
| `GET` | `/api/v1/inventory/movements/{productId}` | Historial de movimientos | No |

## Stack Tecnológico

- **.NET 9** — ASP.NET Core Web API (controllers)
- **Entity Framework Core 9** — ORM para SQL Server
- **MongoDB 7** — Logs de auditoría
- **JWT** — Autenticación stateless
- **MediatR** — CQRS / Pipeline Behaviors
- **FluentValidation** — Validación de comandos
- **AutoMapper** — Mapeo DTO ↔ Entidad
- **Swagger / Swashbuckle** — Documentación interactiva
- **xUnit + Moq** — Pruebas unitarias
- **Docker** — Contenedores (SQL Server, MongoDB, API)

## Requisitos

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (local o Docker)
- MongoDB (local o Docker)
- Docker Desktop (opcional, para contenedores)

## Configuración Rápida (Local)

### 1. Clonar y compilar

```bash
git clone <repo-url>
cd InventoryManagementSystem
dotnet build
```

### 2. Base de datos

El proyecto usa InMemory por defecto para desarrollo. Para usar SQL Server real, configure el connection string en `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=InventoryDb;User Id=sa;Password=Inventory_Admin_123!;TrustServerCertificate=True;"
  }
}
```

### 3. MongoDB (logs de auditoría)

```json
{
  "ConnectionStrings": {
    "MongoDb": "mongodb://localhost:27017"
  }
}
```

Si no se configura, se usa `mongodb://localhost:27017` por defecto.

### 4. Ejecutar

```bash
cd src/API
dotnet run --launch-profile https
```

Swagger disponible en: `https://localhost:7224/swagger`

### 5. Usuarios de prueba (JWT)

| Usuario | Contraseña | Rol |
|---------|-----------|-----|
| `admin` | `admin123` | Admin |
| `user` | `user123` | User |

`POST /api/v1/auth/login` con `{ "username": "admin", "password": "admin123" }` devuelve un JWT.

## Docker

### Requisitos

- Docker Desktop (Windows/Mac) o Docker Engine (Linux)

### Ejecutar todo con docker-compose

```bash
docker-compose up -d
```

Esto levanta:
- **SQL Server 2022 Express** — puerto `1433`
- **MongoDB 7** — puerto `27017`
- **API** — puerto `5000`

Swagger disponible en: `http://localhost:5000/swagger`

### Detener

```bash
docker-compose down
```

Para eliminar volúmenes (borra datos persistentes):
```bash
docker-compose down -v
```

## Pruebas

```bash
dotnet test tests/InventoryManagement.Tests
```

Actualmente **22 tests** (Domain + Application) — todos pasan.

## Estructura del Proyecto

```
InventoryManagementSystem/
├── src/
│   ├── API/                          # ASP.NET Web API
│   │   ├── Controllers/              # Auth, Categories, Products, Inventory, Suppliers
│   │   ├── Middleware/               # ExceptionHandlingMiddleware
│   │   ├── Program.cs                # Configuración (DI, JWT, Swagger, DB)
│   │   └── appsettings*.json
│   ├── Application/                  # CQRS + Validación
│   │   ├── Categories/              # Commands, Queries, Handlers
│   │   ├── Products/
│   │   ├── Suppliers/
│   │   ├── StockManagement/
│   │   ├── Common/                  # Behaviors (Validation, AuditLog)
│   │   ├── DTOs/
│   │   └── DependencyInjection.cs
│   ├── Domain/                       # Entidades y reglas de negocio
│   │   ├── Entities/                # Product, Category, Supplier, Inventory, StockMovement
│   │   ├── ValueObjects/            # Money
│   │   ├── Exceptions/              # InsufficientStockException
│   │   └── Interfaces/             # IRepository<T>, IAuditLogService
│   └── Infrastructure/              # Persistencia
│       ├── Data/                    # ApplicationDbContext, Repositorios
│       ├── Services/                # MongoAuditLogService
│       └── DependencyInjection.cs
├── tests/
│   └── InventoryManagement.Tests/   # xUnit + Moq
└── docker-compose.yml               # SQL Server + MongoDB + API
```

## Variables de Entorno (Docker)

| Variable | Descripción | Default |
|----------|-------------|---------|
| `ConnectionStrings__DefaultConnection` | Cadena de conexión SQL Server | — |
| `ConnectionStrings__MongoDb` | Cadena de conexión MongoDB | `mongodb://localhost:27017` |
| `Jwt__Key` | Clave secreta para firmar JWT | — |
| `Jwt__Issuer` | Emisor del JWT | `InventoryManagementAPI` |
| `Jwt__Audience` | Audiencia del JWT | `InventoryManagementClient` |
