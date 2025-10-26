# ShopVerse — ASP.NET Core E-Commerce (PostgreSQL)

A complete e-commerce web application built with **ASP.NET Core MVC**, **Entity Framework Core**, and **PostgreSQL**.  
Includes a **catalog, shopping cart, checkout, order history, and admin dashboard** for managing products and users.

<p align="center">
  <img src="docs/screenshots/catalog.png" width="860" alt="ShopVerse Preview">
</p>

---

## ✨ Features
- Product catalog with search and categories  
- Product detail pages with images and pricing  
- Add to cart / remove from cart  
- Checkout flow with order confirmation  
- Order history for each user  
- Admin dashboard (CRUD for products, users, and orders)  
- Authentication and roles (User / Admin)  
- PostgreSQL database with EF Core migrations  

---

## 🛠️ Tech Stack
- **ASP.NET Core MVC 8**
- **Entity Framework Core**
- **PostgreSQL (Npgsql)**
- **ASP.NET Identity**
- **Bootstrap 5**

---

## 🚀 Getting Started

### 1️⃣ Requirements
- .NET 8 SDK  
- PostgreSQL installed locally

### 2️⃣ Setup
```bash
# Clone repository
git clone https://github.com/gitprojectacc77-lab/ShopVerse.git
cd ShopVerse

# Install EF tools
dotnet tool install --global dotnet-ef

# Initialize user-secrets
dotnet user-secrets init

# Add database connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=shopverse;Username=postgres;Password=postgres"

# Set default admin credentials
dotnet user-secrets set "Seed:AdminEmail" "admin@shopverse.local"
dotnet user-secrets set "Seed:AdminPassword" "Admin123!"

# Apply migrations
dotnet ef database update

# Run the app
dotnet run

ShopVerse/
 ├── Controllers/
 ├── Data/               # DbContext, models (Product, Category, Order, etc.)
 ├── Migrations/
 ├── Views/              # MVC Views (Catalog, Cart, Orders, Admin, Shared)
 ├── wwwroot/            # Static files (css/js/images)
 └── Program.cs

🧩 Future Improvements
Product filtering and pagination

Image upload support

Order status management (New/Paid/Shipped)

Docker Compose for local PostgreSQL

Payment gateway integration
