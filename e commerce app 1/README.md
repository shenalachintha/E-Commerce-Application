# E-Commerce API - Clean Architecture Backend

ASP.NET Core Web API (.NET 8) E-Commerce backend using Clean Architecture, Entity Framework Core, SQL Server, and JWT Authentication.

## Project Structure

```
src/
├── Ecommerce.Api/           # Web API - Controllers, Program.cs
├── Ecommerce.Application/   # Business logic - DTOs, Interfaces, Services
├── Ecommerce.Domain/        # Entities - Product, Category, User, etc.
└── Ecommerce.Infrastructure/# Data access - DbContext, Repositories, EF Configurations
```

## Entity Relationships

- **Product** → Category (many-to-one)
- **User** → Cart (one-to-one), Orders (one-to-many), Reviews, Wishlists
- **Cart** → CartItems (one-to-many) → Product
- **Order** → OrderItems (one-to-many) → Product
- **Review** → Product, User
- **Wishlist** → User, Product

## Setup

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB, Express, or full)

### Connection String

Update `src/Ecommerce.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EcommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

### Migrations

```bash
# Create migration
dotnet ef migrations add YourMigrationName --project src/Ecommerce.Infrastructure --startup-project src/Ecommerce.Api

# Update database
dotnet ef database update --project src/Ecommerce.Infrastructure --startup-project src/Ecommerce.Api
```

### Run

```bash
dotnet run --project src/Ecommerce.Api
```

API: https://localhost:7xxx (or http://localhost:5xxx)  
Swagger: https://localhost:7xxx/swagger

## API Endpoints

### Auth (no auth required)

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register new user |
| POST | `/api/auth/login` | Login, returns JWT |

### Products (public read)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | List products (paginated, search, category filter) |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create product (Admin) |
| PUT | `/api/products/{id}` | Update product (Admin) |
| DELETE | `/api/products/{id}` | Delete product (Admin) |

### Categories

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/categories` | List all categories |
| GET | `/api/categories/{id}` | Get category by ID |
| POST | `/api/categories` | Create category (Admin) |
| PUT | `/api/categories/{id}` | Update category (Admin) |
| DELETE | `/api/categories/{id}` | Delete category (Admin) |

### Cart (requires auth)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/cart` | Get current user's cart |
| POST | `/api/cart` | Add item to cart |
| PUT | `/api/cart` | Update cart item quantity |
| DELETE | `/api/cart/{productId}` | Remove item from cart |
| DELETE | `/api/cart` | Clear cart |

### Orders (requires auth)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/orders/history` | User order history (paginated) |
| GET | `/api/orders/{id}` | Get order by ID |
| GET | `/api/orders/number/{orderNumber}` | Get order by order number |
| POST | `/api/orders` | Create order (checkout from cart) |
| PUT | `/api/orders/{id}/status` | Update order status |

## Example API Requests/Responses

### Register

**Request:** `POST /api/auth/register`
```json
{
  "email": "user@example.com",
  "password": "Password123",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Response:** `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "role": "Customer",
  "expiresAt": "2026-03-20T12:00:00Z"
}
```

### Login

**Request:** `POST /api/auth/login`
```json
{
  "email": "user@example.com",
  "password": "Password123"
}
```

### Get Products (paginated, search, filter)

**Request:** `GET /api/products?page=1&pageSize=10&search=wireless&categoryId=1`

**Response:** `200 OK`
```json
{
  "items": [
    {
      "id": 1,
      "name": "Wireless Bluetooth Headphones",
      "description": "High-quality noise-cancelling headphones",
      "price": 79.99,
      "stockQuantity": 100,
      "categoryId": 1,
      "categoryName": "Electronics",
      "imageUrl": "https://example.com/headphones.jpg",
      "createdAt": "2026-03-13T00:00:00Z",
      "isActive": true
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

### Create Order (Checkout)

**Request:** `POST /api/orders` (with Bearer token)
```json
{
  "shippingAddress": "123 Main St, City, Country",
  "billingAddress": "123 Main St, City, Country",
  "notes": "Leave at door"
}
```

**Response:** `201 Created`
```json
{
  "id": 1,
  "userId": 1,
  "orderNumber": "ORD-20260313-ABC12345",
  "status": "Pending",
  "subTotal": 159.98,
  "tax": 15.998,
  "shippingCost": 0,
  "total": 175.978,
  "shippingAddress": "123 Main St",
  "orderDate": "2026-03-13T12:00:00Z",
  "orderItems": [
    {
      "id": 1,
      "productId": 1,
      "productName": "Wireless Bluetooth Headphones",
      "quantity": 2,
      "unitPrice": 79.99
    }
  ]
}
```

## First Run

1. Apply migrations:  
   `dotnet ef database update --project src/Ecommerce.Infrastructure --startup-project src/Ecommerce.Api`
2. Run the API:  
   `dotnet run --project src/Ecommerce.Api`

The app will also run migrations automatically on startup. Seed data is applied when the database is empty.

## Seed Data

On first run (or when database is empty), the database is seeded with:
- **5 Categories:** Electronics, Clothing, Home & Garden, Books, Sports
- **11 Sample Products** across those categories

## Authentication

- **Register** and **Login** return a JWT token
- Include token in requests: `Authorization: Bearer <token>`
- Admin operations require user with `Role = "Admin"` (create a user and update DB, or add seed admin)
- Password hashing uses BCrypt
