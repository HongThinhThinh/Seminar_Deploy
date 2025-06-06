# Demo2 API - 3-Layer Architecture with EF Core

Dự án API được xây dựng theo mô hình 3 lớp (Controller-Services-Repository) tuân thủ nguyên tắc SOLID và sử dụng Entity Framework Core.

## 🏗️ Kiến trúc dự án

### Cấu trúc thư mục

```
Demo2/
├── Controller/          # Presentation Layer
│   ├── Controllers/     # API Controllers
│   ├── Program.cs       # Entry point & DI configuration
│   └── appsettings.json # Configuration
├── Services/            # Business Logic Layer
│   ├── DTOs/           # Data Transfer Objects
│   ├── Interfaces/     # Service interfaces
│   ├── Implementations/# Service implementations
│   └── Mappings/       # AutoMapper profiles
└── Repositories/        # Data Access Layer
    ├── Models/         # Domain entities
    ├── Interfaces/     # Repository interfaces
    ├── Implementations/# Repository implementations
    └── Data/           # DbContext
```

### Nguyên tắc SOLID được áp dụng

1. **Single Responsibility Principle (SRP)**

   - Mỗi class có một trách nhiệm duy nhất
   - Controllers chỉ xử lý HTTP requests
   - Services xử lý business logic
   - Repositories xử lý data access

2. **Open/Closed Principle (OCP)**

   - Generic Repository cho phép mở rộng mà không cần sửa đổi
   - Interface-based design

3. **Liskov Substitution Principle (LSP)**

   - Các implementation có thể thay thế interface mà không ảnh hưởng

4. **Interface Segregation Principle (ISP)**

   - Interfaces nhỏ và chuyên biệt
   - Không ép buộc implement những method không cần thiết

5. **Dependency Inversion Principle (DIP)**
   - Phụ thuộc vào abstractions thay vì concrete classes
   - Dependency Injection được sử dụng toàn bộ

## 🚀 Công nghệ sử dụng

- **.NET 8.0**
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **AutoMapper** - Object mapping
- **Swagger/OpenAPI** - API documentation
- **Generic Repository Pattern** - Data access abstraction
- **Unit of Work Pattern** - Transaction management

## 📊 Database Schema

### Entities

**Category**

- Id (int, Primary Key)
- Name (string, Required, Max 100 chars)
- Description (string, Optional, Max 500 chars)
- CreatedAt, UpdatedAt, IsDeleted (Audit fields)

**Product**

- Id (int, Primary Key)
- Name (string, Required, Max 200 chars)
- Description (string, Optional, Max 1000 chars)
- Price (decimal, Required)
- Stock (int, Required)
- CategoryId (int, Foreign Key)
- CreatedAt, UpdatedAt, IsDeleted (Audit fields)

## 🔧 Cài đặt và chạy

### Yêu cầu hệ thống

- .NET 8.0 SDK
- SQL Server (LocalDB)
- Visual Studio 2022 hoặc VS Code

### Các bước cài đặt

1. **Clone repository**

   ```bash
   git clone [repository-url]
   cd Demo2
   ```

2. **Restore packages**

   ```bash
   dotnet restore
   ```

3. **Update connection string** (nếu cần)

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Demo2Db;Trusted_Connection=true;MultipleActiveResultSets=true"
   }
   ```

4. **Chạy migration**

   ```bash
   cd Controller
   dotnet ef database update
   ```

5. **Chạy ứng dụng**

   ```bash
   dotnet run
   ```

6. **Truy cập Swagger UI**
   ```
   https://localhost:5263/swagger
   ```

## 📖 API Endpoints

### Categories

- `GET /api/categories` - Lấy tất cả categories
- `GET /api/categories/{id}` - Lấy category theo ID
- `GET /api/categories/{id}/products` - Lấy category với products
- `POST /api/categories` - Tạo category mới
- `PUT /api/categories/{id}` - Cập nhật category
- `DELETE /api/categories/{id}` - Xóa category (soft delete)

### Products

- `GET /api/products` - Lấy tất cả products
- `GET /api/products/{id}` - Lấy product theo ID
- `GET /api/products/category/{categoryId}` - Lấy products theo category
- `GET /api/products/search?searchTerm={term}` - Tìm kiếm products
- `POST /api/products` - Tạo product mới
- `PUT /api/products/{id}` - Cập nhật product
- `DELETE /api/products/{id}` - Xóa product (soft delete)

## 🧪 Test Data

Database được seed với dữ liệu mẫu:

**Categories:**

1. Electronics - Electronic devices and gadgets
2. Books - Books and educational materials
3. Clothing - Fashion and apparel

**Products:**

1. Laptop - High-performance laptop ($999.99, Electronics)
2. Smartphone - Latest smartphone model ($699.99, Electronics)
3. Programming Book - Learn programming basics ($29.99, Books)

## 🔒 Features

- **Soft Delete** - Dữ liệu không bị xóa vật lý
- **Audit Fields** - Tracking thời gian tạo và cập nhật
- **Data Validation** - Validation attributes trên DTOs
- **Error Handling** - Global exception handling
- **Auto Mapping** - AutoMapper cho entity-DTO conversion
- **Generic Repository** - Tái sử dụng code cho CRUD operations
- **Unit of Work** - Transaction management
- **Dependency Injection** - Loose coupling

## 📝 Các Pattern được sử dụng

1. **Repository Pattern** - Abstraction layer cho data access
2. **Unit of Work Pattern** - Quản lý transactions
3. **DTO Pattern** - Data transfer between layers
4. **Dependency Injection** - Inversion of control
5. **Generic Repository** - Code reusability
6. **Soft Delete Pattern** - Data preservation

## 🎯 Best Practices

- Separation of concerns
- Interface-based programming
- Configuration management
- Error handling and logging
- Data validation
- API documentation
- Clean code principles
- SOLID principles

## 🔄 Workflow

1. **Request** → Controller nhận HTTP request
2. **Validation** → Controller validate input với DTOs
3. **Business Logic** → Controller gọi Service để xử lý
4. **Data Access** → Service sử dụng Repository để truy cập data
5. **Database** → Repository tương tác với EF Core và database
6. **Response** → Kết quả được trả về qua các layer và mapping về DTOs

## 🚀 Mở rộng trong tương lai

- Authentication & Authorization (JWT)
- Caching (Redis)
- Logging (Serilog)
- Health Checks
- Rate Limiting
- API Versioning
- Unit Testing & Integration Testing
- Docker containerization
- CI/CD pipeline
