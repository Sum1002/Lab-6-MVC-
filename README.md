# Bookshop Management System

A comprehensive ASP.NET Core MVC web application for managing books, shops, customers, and orders in a bookshop environment.

## Features

- **Book Management**: Add, edit, delete, and view books with ISBN validation
- **Shop Management**: Manage multiple bookshop locations
- **Customer Management**: Store customer information with contact details
- **Order Management**: Track orders with status updates
- **Inventory Tracking**: Monitor book quantities across different shops
- **Data Validation**: Comprehensive input validation using data annotations
- **Responsive UI**: Modern Bootstrap-based interface

## Entity Relationship Diagram (ERD)

```
┌─────────────┐    ┌──────────────┐    ┌─────────────┐
│    Books    │    │   BookShop   │    │    Shops    │
├─────────────┤    ├──────────────┤    ├─────────────┤
│ BookId (PK) │◄───┤ BookShopId   │───►│ ShopId (PK) │
│ Title       │    │ (PK)         │    │ Name        │
│ Author      │    │ BookId (FK)  │    │ Location    │
│ ISBN        │    │ ShopId (FK)  │    │ Address     │
│ Price       │    │ Quantity     │    │ Phone       │
│ Description │    │ ShopPrice    │    │ Email       │
│ PubYear     │    │ Notes        │    │ Website     │
│ Genre       │    └──────────────┘    │ OpenYear    │
└─────────────┘           │            └─────────────┘
                          │
                          ▼
                   ┌─────────────┐    ┌──────────────┐
                   │   Orders    │    │   Customers  │
                   ├─────────────┤    ├──────────────┤
                   │ OrderId(PK) │    │ CustomerId   │
                   │ CustomerId  │◄───┤ (PK)         │
                   │ BookShopId  │    │ FirstName    │
                   │ Quantity    │    │ LastName     │
                   │ OrderDate   │    │ Email        │
                   │ TotalPrice  │    │ Phone        │
                   │ OrderStatus │    │ Address      │
                   │ OrderNotes  │    │ City         │
                   │ ShippedDate │    │ State        │
                   │ ShipMethod  │    │ PostalCode   │
                   │ ShipCost    │    │ Country      │
                   └─────────────┘    │ DateOfBirth  │
                                      └──────────────┘
```

## Database Schema

### Books Table
- `BookId` (Primary Key, Auto-increment)
- `Title` (Required, Max 200 characters)
- `Author` (Required, Max 100 characters)
- `ISBN` (Required, Unique, Valid ISBN format)
- `Price` (Required, Positive decimal)
- `Description` (Optional, Max 1000 characters)
- `PublicationYear` (Optional, Non-negative integer)
- `Genre` (Optional, Max 50 characters)

### Shops Table
- `ShopId` (Primary Key, Auto-increment)
- `Name` (Required, Max 100 characters)
- `Location` (Required, Max 200 characters)
- `Address` (Optional, Max 500 characters)
- `Phone` (Optional, Valid phone format)
- `Email` (Optional, Valid email format)
- `Website` (Optional, Valid URL format)
- `OpeningYear` (Optional, Non-negative integer)

### Customers Table
- `CustomerId` (Primary Key, Auto-increment)
- `FirstName` (Required, Max 50 characters)
- `LastName` (Required, Max 50 characters)
- `Email` (Required, Unique, Valid email format)
- `Phone` (Optional, Valid phone format)
- `Address` (Optional, Max 200 characters)
- `City` (Optional, Max 100 characters)
- `State` (Optional, Max 50 characters)
- `PostalCode` (Optional, Max 20 characters)
- `Country` (Optional, Max 50 characters)
- `DateOfBirth` (Optional, Valid date)

### BookShop Table (Junction Table)
- `BookShopId` (Primary Key, Auto-increment)
- `BookId` (Foreign Key to Books)
- `ShopId` (Foreign Key to Shops)
- `Quantity` (Required, Non-negative integer)
- `ShopPrice` (Optional, Positive decimal)
- `Notes` (Optional, Max 500 characters)

### Orders Table
- `OrderId` (Primary Key, Auto-increment)
- `CustomerId` (Foreign Key to Customers)
- `BookShopId` (Foreign Key to BookShop)
- `Quantity` (Required, Minimum 1)
- `OrderDate` (Required, Current date by default)
- `TotalPrice` (Required, Positive decimal)
- `OrderStatus` (Required, Default "Pending")
- `OrderNotes` (Optional, Max 500 characters)
- `ShippedDate` (Optional, Valid date)
- `ShippingMethod` (Optional, Max 100 characters)
- `ShippingCost` (Optional, Non-negative decimal)

## Relationships

1. **Books ↔ Shops**: Many-to-Many relationship via BookShop junction table
2. **Customers → Orders**: One-to-Many relationship
3. **BookShop → Orders**: One-to-Many relationship (via BookShopId)

## Constraints

- **Unique Constraints**: ISBN (Books), Email (Customers)
- **Composite Unique**: BookId + ShopId (BookShop table)
- **Referential Integrity**: Foreign key constraints with appropriate delete behaviors
- **Data Validation**: Range constraints for quantities, prices, and dates
- **Format Validation**: Email, phone, URL, and ISBN formats

## Technology Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: Entity Framework Core with SQLite (can be configured for SQL Server)
- **ORM**: Entity Framework Core
- **UI Framework**: Bootstrap 5
- **Validation**: Data Annotations
- **Language**: C#

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- SQLite (included) or SQL Server

### Installation

1. Clone the repository
2. Navigate to the project directory
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
4. Run the application:
   ```bash
   dotnet run
   ```

### Database Setup

The application uses Entity Framework Core with SQLite by default. The database will be created automatically when you first run the application.

To use SQL Server instead:

1. Update the connection string in `appsettings.json`
2. Modify `Program.cs` to use SQL Server:
   ```csharp
   options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"))
   ```
3. Run migrations:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

### Sample Data

The application includes sample data for:
- 3 sample books
- 3 sample shops
- 3 sample customers
- 6 book-shop inventory records
- 3 sample orders

## Project Structure

```
BookshopManagement/
├── Controllers/          # MVC Controllers
├── Data/                # Entity Framework Context
├── Models/              # Entity Models
├── Views/               # Razor Views
├── wwwroot/             # Static Files (CSS, JS)
├── Program.cs           # Application Entry Point
├── appsettings.json     # Configuration
└── BookshopManagement.csproj
```

## Features Implemented

- ✅ Complete Entity Models with Data Annotations
- ✅ DbContext Configuration with Relationships
- ✅ Database Seeding with Sample Data
- ✅ Basic MVC Structure
- ✅ Home Controller with Dashboard
- ✅ Books Controller with CRUD Operations
- ✅ Navigation and Layout
- ✅ Responsive Bootstrap UI
- ✅ Data Validation
- ✅ Entity Relationships

## Future Enhancements

- User Authentication and Authorization
- Advanced Search and Filtering
- Reporting and Analytics
- API Endpoints
- Real-time Inventory Updates
- Email Notifications
- Payment Integration

## License

This project is licensed under the MIT License.
