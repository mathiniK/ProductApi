# Product Code Management API

Product Code Management API built with **.NET 8**, **EF Core (MySQL)**, **JWT (HS256)**, and **Swagger**. Implements required endpoints with **unique ProductCode** constraint.

## Requirements
* .NET 8 SDK
* MySQL 8 (recommended) or 5.7

## Getting Started

### 1. Configure Database & JWT
Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Server=127.0.0.1;Port=3306;Database=product_db;User=root;Password=your_mysql_password;TreatTinyAsBoolean=false"
  },
  "Jwt": {
    "Key": "use-a-32+char-random-secret-here",
    "Issuer": "ProductApi",
    "Audience": "ProductApiClient",
    "ExpiresMinutes": 60
  }
}
```

⚠️ **JWT key must be ≥ 256 bits (32+ chars) for HS256.**

### 1. Install EF Tools (if needed)
```bash
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### 2. Run Migrations & Start API
```bash
dotnet ef database update
dotnet run
```

### 4. Test with Swagger
1. Open: `http://localhost:5000/swagger`
2. Call **POST** `/api/token` with:
   ```json
   { "username": "admin", "password": "admin123" }
   ```
3. Click **Authorize** (padlock), paste the **raw token string**
4. Use secured endpoints under `/api/product/*`

## API Reference

### Auth
* **POST** `/api/token` → `{ "token": "<JWT>" }`  
  Credentials: `admin / admin123`

### Products (requires `Authorization: Bearer <token>`)
* **POST** `/api/product` — Create product  
  ```json
  { "productCode": "TV-001", "productName": "Smart TV 55", "price": 1999.99 }
  ```
  Responses: `201 Created`, `400 BadRequest`, `409 Conflict` (duplicate code)

* **PUT** `/api/product/{id}` — Update by ID  
  → `204 No Content` or `409 Conflict`

* **GET** `/api/product/search?query=tv` — Search by code or name  
  → `200 OK` with product list

## Data Model
```csharp
public class Product {
  public int Id { get; set; }
  public string ProductCode { get; set; }  // UNIQUE in DB
  public string ProductName { get; set; }
  public decimal Price { get; set; }
}
```
## Troubleshooting
* **EF Tools error**: `dotnet add package Microsoft.EntityFrameworkCore.Design`
* **MySQL connection issues**: Ensure MySQL service is running
* **JWT error IDX10720**: Use 32+ character secret key
* **No Swagger Authorize button**: Add Bearer security definition in `AddSwaggerGen`
