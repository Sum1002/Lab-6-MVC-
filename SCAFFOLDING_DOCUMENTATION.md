# Bookshop Management System - CRUD Scaffolding Documentation

## Overview

This document provides comprehensive documentation for the Bookshop Management System with full CRUD (Create, Read, Update, Delete) operations implemented using ASP.NET Core MVC with Entity Framework Core scaffolding.

## System Architecture

The system consists of five main entities:

- **Books**: Book catalog with details like title, author, ISBN, price, etc.
- **Shops**: Physical bookstores with location and contact information
- **Customers**: Customer information and contact details
- **Orders**: Customer orders linking books, shops, and customers
- **BookShops**: Junction table managing inventory (which books are available in which shops)

## Scaffolded Controllers and Views

### 1. Books Controller (`BooksController.cs`)

**CRUD Operations:**

- **Index**: Lists all books with title, author, ISBN, price, genre, and publication year
- **Details**: Shows detailed book information including available shops
- **Create**: Form to add new books with validation
- **Edit**: Form to modify existing book information
- **Delete**: Confirmation page to remove books

**Key Features:**

- ISBN validation with regex pattern
- Price validation (must be greater than 0)
- String length validations for all text fields
- Navigation to related BookShop entries

### 2. Shops Controller (`ShopsController.cs`)

**CRUD Operations:**

- **Index**: Lists all shops with name, location, address, phone, email, and opening year
- **Details**: Shows detailed shop information including available books
- **Create**: Form to add new shops with contact validation
- **Edit**: Form to modify existing shop information
- **Delete**: Confirmation page to remove shops

**Key Features:**

- Email validation for shop contact
- Phone number validation
- URL validation for website
- Navigation to related BookShop entries

### 3. Customers Controller (`CustomersController.cs`)

**CRUD Operations:**

- **Index**: Lists all customers with name, email, phone, city, state, and date of birth
- **Details**: Shows detailed customer information including order history
- **Create**: Form to add new customers with comprehensive address fields
- **Edit**: Form to modify existing customer information
- **Delete**: Confirmation page to remove customers

**Key Features:**

- Email validation (required and unique)
- Phone number validation
- Computed property for full name display
- Navigation to related orders

### 4. Orders Controller (`OrdersController.cs`)

**CRUD Operations:**

- **Index**: Lists all orders with customer, book, shop, quantity, total price, and status
- **Details**: Shows detailed order information including all related data
- **Create**: Form to create new orders with dropdowns for customer and book-shop selection
- **Edit**: Form to modify existing orders including status updates
- **Delete**: Confirmation page to remove orders

**Key Features:**

- Dropdown selection for customers (shows full name)
- Dropdown selection for book-shop combinations (shows "Book Title - Shop Name")
- Order status management (Pending, Processing, Shipped, Completed, Cancelled)
- Automatic order date setting
- Shipping information tracking

### 5. BookShops Controller (`BookShopsController.cs`)

**CRUD Operations:**

- **Index**: Lists all book-shop inventory entries with book, shop, quantity, and price
- **Details**: Shows detailed inventory information including related orders
- **Create**: Form to add books to shop inventory
- **Edit**: Form to modify inventory quantities and prices
- **Delete**: Confirmation page to remove inventory entries

**Key Features:**

- Dropdown selection for books and shops
- Quantity tracking (must be non-negative)
- Shop-specific pricing (optional, defaults to book price)
- Notes field for additional information
- Navigation to related orders

## Data Validation

### Model Validation Attributes

All models include comprehensive data annotations for validation:

#### Book Model

```csharp
[Required(ErrorMessage = "Title is required")]
[StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
public string Title { get; set; }

[Required(ErrorMessage = "ISBN is required")]
[RegularExpression(@"^(?:ISBN(?:-1[03])?:? )?(?=[0-9X]{10}$|(?=(?:[0-9]+[- ]){3})[- 0-9X]{13}$|97[89][0-9]{10}$|(?=(?:[0-9]+[- ]){4})[- 0-9X]{17}$)(?:97[89][- ]?)?[0-9]{1,5}[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9X]$",
    ErrorMessage = "Please enter a valid ISBN")]
public string ISBN { get; set; }

[Required(ErrorMessage = "Price is required")]
[Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
[DataType(DataType.Currency)]
public decimal Price { get; set; }
```

#### Customer Model

```csharp
[Required(ErrorMessage = "Email is required")]
[EmailAddress(ErrorMessage = "Please enter a valid email address")]
[StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
public string Email { get; set; }

[Phone(ErrorMessage = "Please enter a valid phone number")]
[StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
public string? Phone { get; set; }
```

#### Order Model

```csharp
[Required(ErrorMessage = "Quantity is required")]
[Range(1, int.MaxValue, ErrorMessage = "Order quantity must be at least 1")]
public int Quantity { get; set; }

[Required(ErrorMessage = "Total price is required")]
[Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than 0")]
[DataType(DataType.Currency)]
public decimal TotalPrice { get; set; }
```

### Validation Features

1. **Client-side Validation**: All forms include client-side validation using jQuery validation
2. **Server-side Validation**: ModelState validation in all POST actions
3. **Custom Error Messages**: User-friendly error messages for all validation rules
4. **Visual Feedback**: Bootstrap styling for validation states (success, error, warning)

## Database Integration

### Entity Framework Core Configuration

The `BookshopContext` includes:

- Proper entity configurations with fluent API
- Relationship mappings (one-to-many, many-to-many)
- Unique constraints (ISBN for books, Email for customers)
- Composite unique index for BookShop (BookId, ShopId combination)
- Seed data for testing

### Database Schema

```sql
-- Books table with ISBN uniqueness
CREATE TABLE Books (
    BookId INTEGER PRIMARY KEY,
    Title TEXT NOT NULL,
    Author TEXT NOT NULL,
    ISBN TEXT NOT NULL UNIQUE,
    Price DECIMAL(18,2) NOT NULL,
    Description TEXT,
    PublicationYear INTEGER,
    Genre TEXT
);

-- Shops table
CREATE TABLE Shops (
    ShopId INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    Location TEXT NOT NULL,
    Address TEXT,
    Phone TEXT,
    Email TEXT,
    Website TEXT,
    OpeningYear INTEGER
);

-- Customers table with email uniqueness
CREATE TABLE Customers (
    CustomerId INTEGER PRIMARY KEY,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE,
    Phone TEXT,
    Address TEXT,
    City TEXT,
    State TEXT,
    PostalCode TEXT,
    Country TEXT,
    DateOfBirth DATE
);

-- BookShops junction table
CREATE TABLE BookShops (
    BookShopId INTEGER PRIMARY KEY,
    BookId INTEGER NOT NULL,
    ShopId INTEGER NOT NULL,
    Quantity INTEGER NOT NULL,
    ShopPrice DECIMAL(18,2),
    Notes TEXT,
    FOREIGN KEY (BookId) REFERENCES Books(BookId),
    FOREIGN KEY (ShopId) REFERENCES Shops(ShopId),
    UNIQUE(BookId, ShopId)
);

-- Orders table
CREATE TABLE Orders (
    OrderId INTEGER PRIMARY KEY,
    CustomerId INTEGER NOT NULL,
    BookShopId INTEGER NOT NULL,
    Quantity INTEGER NOT NULL,
    OrderDate DATE NOT NULL,
    TotalPrice DECIMAL(18,2) NOT NULL,
    OrderStatus TEXT NOT NULL,
    OrderNotes TEXT,
    ShippedDate DATE,
    ShippingMethod TEXT,
    ShippingCost DECIMAL(18,2) DEFAULT 0,
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
    FOREIGN KEY (BookShopId) REFERENCES BookShops(BookShopId)
);
```

## Sample Data

The system includes seeded data for testing:

### Books

- The Great Gatsby by F. Scott Fitzgerald (ISBN: 978-0743273565, $12.99)
- To Kill a Mockingbird by Harper Lee (ISBN: 978-0446310789, $14.99)
- 1984 by George Orwell (ISBN: 978-0451524935, $11.99)

### Shops

- Downtown Bookstore (Downtown location)
- University Bookshop (University District)
- Mall Bookstore (Shopping Mall)

### Customers

- John Doe (john.doe@email.com)
- Jane Smith (jane.smith@email.com)
- Bob Johnson (bob.johnson@email.com)

### BookShops (Inventory)

- Multiple inventory entries linking books to shops with quantities and prices

### Orders

- Sample orders with different statuses (Pending, Shipped, Completed)

## User Interface Features

### Navigation

- Bootstrap-based responsive navigation bar
- Links to all entity management pages
- Consistent styling across all views

### Views Design

- **Index Views**: Responsive tables with Bootstrap styling
- **Details Views**: Card-based layout with related data sections
- **Create/Edit Views**: Form layouts with validation feedback
- **Delete Views**: Confirmation pages with warning messages

### Form Features

- **Input Types**: Appropriate input types (email, url, number, date)
- **Placeholders**: Helpful placeholder text for all inputs
- **Validation**: Real-time validation with error messages
- **Help Text**: Information cards explaining field requirements

## How Scaffolding Simplified CRUD Development

### 1. **Automatic Code Generation**

- Controllers with all CRUD actions generated automatically
- Views with proper form layouts and validation
- Consistent naming conventions and structure

### 2. **Built-in Validation**

- Data annotations automatically translated to client-side validation
- Server-side validation with ModelState
- Consistent error handling across all entities

### 3. **Database Integration**

- Entity Framework Core integration with proper relationship handling
- Automatic SQL generation for CRUD operations
- Connection string management

### 4. **Consistent UI**

- Bootstrap styling applied consistently
- Responsive design for all screen sizes
- Standard form layouts and navigation patterns

### 5. **Relationship Management**

- Automatic handling of foreign key relationships
- Dropdown lists for related entities
- Proper Include statements for related data loading

## Validation Handling Examples

### Valid Input Example (Book Creation)

```
Title: "The Catcher in the Rye"
Author: "J.D. Salinger"
ISBN: "978-0316769174"
Price: 15.99
Genre: "Fiction"
Publication Year: 1951
Description: "A coming-of-age story..."
```

### Invalid Input Examples

#### Missing Required Fields

```
Title: "" (empty)
Author: "J.D. Salinger"
ISBN: "978-0316769174"
Price: 15.99
```

**Result**: "Title is required" error message

#### Invalid ISBN Format

```
Title: "The Catcher in the Rye"
Author: "J.D. Salinger"
ISBN: "invalid-isbn"
Price: 15.99
```

**Result**: "Please enter a valid ISBN" error message

#### Invalid Price

```
Title: "The Catcher in the Rye"
Author: "J.D. Salinger"
ISBN: "978-0316769174"
Price: -5.00
```

**Result**: "Price must be greater than 0" error message

#### Invalid Email (Customer)

```
FirstName: "John"
LastName: "Doe"
Email: "invalid-email"
Phone: "555-1234"
```

**Result**: "Please enter a valid email address" error message

## Challenges Faced During Scaffolding

### 1. **Relationship Complexity**

- **Challenge**: Managing many-to-many relationships between Books and Shops
- **Solution**: Created BookShop junction table with proper navigation properties

### 2. **Validation Requirements**

- **Challenge**: Implementing complex validation rules (ISBN format, email uniqueness)
- **Solution**: Used data annotations with custom error messages and regex patterns

### 3. **User Experience**

- **Challenge**: Making forms user-friendly with proper dropdowns and help text
- **Solution**: Enhanced views with SelectList population and information cards

### 4. **Data Consistency**

- **Challenge**: Ensuring referential integrity across related entities
- **Solution**: Proper foreign key constraints and cascade delete configurations

### 5. **View Organization**

- **Challenge**: Creating consistent and intuitive navigation
- **Solution**: Standardized view layouts and navigation structure

## Testing CRUD Operations

### Create Operations

1. **Books**: Add new books with all required fields
2. **Shops**: Create new shop locations with contact information
3. **Customers**: Register new customers with complete profiles
4. **Orders**: Place orders linking customers to book-shop combinations
5. **BookShops**: Add books to shop inventory with quantities and prices

### Read Operations

1. **Index Views**: List all entities with key information
2. **Details Views**: View complete entity information with related data
3. **Search/Filter**: Basic listing functionality (can be enhanced)

### Update Operations

1. **Edit Forms**: Modify existing entity information
2. **Validation**: Ensure updated data meets validation requirements
3. **Relationships**: Update related entity references

### Delete Operations

1. **Confirmation Pages**: Safe deletion with confirmation
2. **Cascade Handling**: Proper handling of related entity deletions
3. **Data Integrity**: Maintain referential integrity

## Database Updates Verification

### After Creating Records

- New records appear in respective tables
- Foreign key relationships are properly established
- Validation constraints are enforced

### After Editing Records

- Changes are persisted to the database
- Related data is updated appropriately
- Validation rules are maintained

### After Deleting Records

- Records are removed from the database
- Related records are handled according to cascade rules
- Data integrity is maintained

## Conclusion

The Bookshop Management System successfully implements full CRUD operations for all entities using ASP.NET Core MVC scaffolding. The system provides:

- **Complete CRUD functionality** for Books, Shops, Customers, Orders, and BookShops
- **Comprehensive validation** with both client-side and server-side validation
- **User-friendly interface** with Bootstrap styling and responsive design
- **Proper database integration** with Entity Framework Core
- **Relationship management** between all entities
- **Sample data** for testing and demonstration

The scaffolding approach significantly reduced development time while ensuring consistency and best practices across all CRUD operations. The system is ready for production use with proper error handling, validation, and user experience considerations.
