# Inventory Management System

Clean Architecture · ASP.NET Core Web API 9 · React · PostgreSQL (Supabase) · MongoDB · JWT · Docker

Sistema de gestión de inventario con operaciones CRUD para productos, categorías, proveedores, control de stock y autenticación de usuarios.

## Stack

| Tecnología | Uso |
|-----------|-----|
| **.NET 9** | API REST (ASP.NET Core, controllers) |
| **Entity Framework Core 9** | ORM |
| **PostgreSQL (Supabase)** | Base de datos principal |
| **MongoDB** | Logs de auditoría |
| **React 19 + Vite** | Frontend SPA |
| **JWT** | Autenticación stateless |
| **MediatR** | CQRS / Pipeline Behaviors |
| **FluentValidation** | Validación de comandos |
| **Swagger / Swashbuckle** | Documentación interactiva |
| **xUnit + Moq** | Pruebas unitarias (22 tests) |
| **Docker** | Contenedores |

## Arquitectura

```
┌──────────────────────────────────┐
│     API (Controllers, JWT)       │  ← Capa de presentación
├──────────────────────────────────┤
│   Application (CQRS, Behaviors)  │  ← Casos de uso, DTOs, Validación
├──────────────────────────────────┤
│  Infrastructure (EF Core, Mongo) │  ← Persistencia, Servicios externos
├──────────────────────────────────┤
│     Domain (Entities, Rules)     │  ← Reglas de negocio, Interfaces
└──────────────────────────────────┘
```

### Capas

| Capa | Proyecto | Responsabilidad |
|------|----------|----------------|
| **Domain** | `InventoryManagement.Domain` | Entidades (`Product`, `Category`, `Supplier`, `Inventory`, `StockMovement`, `User`), Value Objects (`Money`), Excepciones, Interfaces de repositorio |
| **Application** | `InventoryManagement.Application` | CQRS con MediatR (Commands/Queries/Handlers), FluentValidation, DTOs, Behaviors (Validation + AuditLog), AuthService |
| **Infrastructure** | `InventoryManagement.Infrastructure` | EF Core `ApplicationDbContext` con Fluent API, repositorios, `MongoAuditLogService` |
| **API** | `InventoryManagement.API` | Controllers REST, JWT, Swagger, Exception Handling Middleware, seed de usuarios |

## Diagrama ER

```
Users ───────────────────────── (autenticación)

Category (1) ──< (N) Product (N) >── (N) Supplier
                        │
                        │ (1)
                        │
                    Inventory (1)
                        │ (N)
                        │
                  StockMovement          MongoDB (audit logs)
```

## Requisitos

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- Node.js 18+ (para frontend)
- PostgreSQL (local o Supabase)
- MongoDB (local o Docker)
- Docker Desktop (opcional)

## Configuración

### 1. Clonar y compilar

```bash
git clone https://github.com/AngelJefferson/InventoryManagementSystem.git
cd InventoryManagementSystem
dotnet build
```

### 2. Base de datos (PostgreSQL)

Configura el connection string en `src/API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=db.xxxx.supabase.co;Database=postgres;Username=postgres;Password=tu_password;SSL Mode=Require;Trust Server Certificate=true"
  }
}
```

Si no hay connection string, el proyecto usa **InMemory** para desarrollo local.

#### Tablas requeridas

Ejecuta este SQL en tu base de datos:

```sql
CREATE TABLE "Categories" (
    "Id" uuid PRIMARY KEY, "Name" varchar(100) NOT NULL, "Description" varchar(500) NOT NULL DEFAULT '',
    "CreatedAt" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP, "UpdatedAt" timestamp
);
CREATE TABLE "Suppliers" (
    "Id" uuid PRIMARY KEY, "Name" varchar(200) NOT NULL, "ContactName" varchar(200) NOT NULL,
    "Email" varchar(200) NOT NULL, "Phone" varchar(50) NOT NULL DEFAULT '',
    "CreatedAt" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP, "UpdatedAt" timestamp
);
CREATE TABLE "Products" (
    "Id" uuid PRIMARY KEY, "Name" varchar(200) NOT NULL, "Description" varchar(2000) NOT NULL DEFAULT '',
    "SKU" varchar(50) NOT NULL, "Price" numeric(18,2) NOT NULL, "Currency" varchar(3) NOT NULL,
    "CategoryId" uuid NOT NULL REFERENCES "Categories"("Id") ON DELETE RESTRICT,
    "SupplierId" uuid REFERENCES "Suppliers"("Id") ON DELETE SET NULL,
    "IsActive" boolean NOT NULL DEFAULT true,
    "CreatedAt" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP, "UpdatedAt" timestamp,
    CONSTRAINT "AK_Products_SKU" UNIQUE ("SKU")
);
CREATE TABLE "Inventories" (
    "Id" uuid PRIMARY KEY, "ProductId" uuid NOT NULL UNIQUE REFERENCES "Products"("Id") ON DELETE CASCADE,
    "QuantityOnHand" integer NOT NULL, "MinimumStock" integer NOT NULL DEFAULT 0,
    "Location" varchar(200) NOT NULL DEFAULT 'Main Warehouse', "LastUpdated" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
);
CREATE TABLE "StockMovements" (
    "Id" uuid PRIMARY KEY, "ProductId" uuid NOT NULL REFERENCES "Products"("Id") ON DELETE NO ACTION,
    "Type" varchar(10) NOT NULL, "Quantity" integer NOT NULL, "Reference" varchar(200) NOT NULL DEFAULT '',
    "CreatedAt" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
);
CREATE TABLE "Users" (
    "Id" uuid PRIMARY KEY, "Username" varchar(50) NOT NULL, "PasswordHash" text NOT NULL,
    "Email" varchar(200) NOT NULL DEFAULT '', "Role" varchar(20) NOT NULL DEFAULT 'User',
    "IsActive" boolean NOT NULL DEFAULT true,
    "CreatedAt" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP, "UpdatedAt" timestamp,
    CONSTRAINT "AK_Users_Username" UNIQUE ("Username")
);
```

### 3. MongoDB (logs de auditoría)

```json
{
  "ConnectionStrings": {
    "MongoDb": "mongodb://localhost:27017"
  }
}
```

Si no se configura, usa `mongodb://localhost:27017` por defecto.

### 4. Ejecutar API

```bash
cd src/API
dotnet run
```

Swagger: `http://localhost:5036/swagger`

### 5. Ejecutar Frontend

```bash
cd frontend
npm install
npm run dev
```

Abrir `http://localhost:5173`

## Autenticación

Los usuarios se almacenan en la tabla `Users` con passwords hasheados (BCrypt). Al iniciar la API por primera vez, se crean automáticamente:

| Usuario | Contraseña | Rol |
|---------|-----------|-----|
| `admin` | `admin123` | Admin |
| `user` | `user123` | User |

Endpoints:
- `POST /api/v1/auth/login` — login
- `POST /api/v1/auth/register` — registrar nuevo usuario
- `GET /api/v1/auth/me` — perfil del usuario autenticado (requiere JWT)

## Endpoints

### Category
| Método | Ruta | Auth |
|--------|------|------|
| `GET` | `/api/v1/categories` | No |
| `GET` | `/api/v1/categories/{id}` | No |
| `POST` | `/api/v1/categories` | Admin |
| `PUT` | `/api/v1/categories/{id}` | Admin |
| `DELETE` | `/api/v1/categories/{id}` | Admin |

### Product
| Método | Ruta | Auth |
|--------|------|------|
| `GET` | `/api/v1/products` | No |
| `GET` | `/api/v1/products/{id}` | No |
| `POST` | `/api/v1/products` | Admin |
| `PUT` | `/api/v1/products/{id}` | Admin |
| `DELETE` | `/api/v1/products/{id}` | Admin |
| `GET` | `/api/v1/products/{id}/stock` | No |
| `GET` | `/api/v1/products/low-stock?threshold=5` | JWT |

### Supplier
| Método | Ruta | Auth |
|--------|------|------|
| `GET` | `/api/v1/suppliers` | No |
| `GET` | `/api/v1/suppliers/{id}` | No |
| `POST` | `/api/v1/suppliers` | Admin |
| `PUT` | `/api/v1/suppliers/{id}` | Admin |
| `DELETE` | `/api/v1/suppliers/{id}` | Admin |

### Inventory
| Método | Ruta | Auth |
|--------|------|------|
| `GET` | `/api/v1/inventory/{productId}` | No |
| `POST` | `/api/v1/inventory/adjust` | JWT |
| `GET` | `/api/v1/inventory/movements/{productId}` | No |

## Docker

```bash
docker-compose up -d
```

Levanta:
- **PostgreSQL** — puerto `5432` (o configurado)
- **MongoDB** — puerto `27017`
- **API** — puerto `5000`

Swagger: `http://localhost:5000/swagger`

## Pruebas

```bash
dotnet test tests/InventoryManagement.Tests
```

22 tests (Domain + Application) — todos pasan.

## Estructura del Proyecto

```
InventoryManagementSystem/
├── src/
│   ├── API/
│   │   ├── Controllers/           # Auth, Categories, Products, Inventory, Suppliers
│   │   ├── Middleware/             # ExceptionHandlingMiddleware
│   │   ├── Program.cs             # DI, JWT, Swagger, DB, seed
│   │   └── appsettings.json
│   ├── Application/
│   │   ├── Categories/            # Commands, Queries, Handlers
│   │   ├── Products/
│   │   ├── Suppliers/
│   │   ├── StockManagement/
│   │   ├── Common/                # Behaviors, Interfaces
│   │   ├── DTOs/
│   │   └── Services/              # AuthService
│   ├── Domain/
│   │   ├── Entities/              # Category, Product, Supplier, Inventory, StockMovement, User
│   │   ├── ValueObjects/          # Money
│   │   ├── Exceptions/            # InsufficientStockException
│   │   └── Interfaces/            # Repositorios
│   └── Infrastructure/
│       ├── Data/                  # ApplicationDbContext, Fluent API
│       ├── Repositories/          # UserRepository
│       └── Services/              # MongoAuditLogService
├── frontend/
│   ├── src/
│   │   ├── api/                   # axios + servicios (auth, categories, products, suppliers, inventory)
│   │   ├── components/            # Navbar, Layout, ProtectedRoute
│   │   ├── context/               # AuthContext
│   │   ├── pages/                 # Login, Dashboard, CRUD, Inventory, Movements
│   │   └── App.jsx                # React Router (12 rutas)
│   └── vite.config.js             # proxy /api → localhost:5036
├── tests/
│   └── InventoryManagement.Tests/ # xUnit + Moq (22 tests)
├── docker-compose.yml
└── Dockerfile
```
