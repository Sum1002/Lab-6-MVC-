CREATE TABLE "BookShops" (
    "BookShopId" INTEGER NOT NULL CONSTRAINT "PK_BookShops" PRIMARY KEY AUTOINCREMENT,
    "BookId" INTEGER NOT NULL,
    "ShopId" INTEGER NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "ShopPrice" decimal(18,2) NULL,
    "Notes" TEXT NULL,
    CONSTRAINT "FK_BookShops_Books_BookId" FOREIGN KEY ("BookId") REFERENCES "Books" ("BookId") ON DELETE CASCADE,
    CONSTRAINT "FK_BookShops_Shops_ShopId" FOREIGN KEY ("ShopId") REFERENCES "Shops" ("ShopId") ON DELETE CASCADE
)

CREATE TABLE "Books" (
    "BookId" INTEGER NOT NULL CONSTRAINT "PK_Books" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "Author" TEXT NOT NULL,
    "ISBN" TEXT NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Description" TEXT NULL,
    "PublicationYear" INTEGER NULL,
    "Genre" TEXT NULL
)

CREATE TABLE "Customers" (
    "CustomerId" INTEGER NOT NULL CONSTRAINT "PK_Customers" PRIMARY KEY AUTOINCREMENT,
    "FirstName" TEXT NOT NULL,
    "LastName" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Phone" TEXT NULL,
    "Address" TEXT NULL,
    "City" TEXT NULL,
    "State" TEXT NULL,
    "PostalCode" TEXT NULL,
    "Country" TEXT NULL,
    "DateOfBirth" TEXT NULL
)

CREATE TABLE "Orders" (
    "OrderId" INTEGER NOT NULL CONSTRAINT "PK_Orders" PRIMARY KEY AUTOINCREMENT,
    "CustomerId" INTEGER NOT NULL,
    "BookShopId" INTEGER NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "OrderDate" TEXT NOT NULL,
    "TotalPrice" decimal(18,2) NOT NULL,
    "OrderStatus" TEXT NOT NULL,
    "OrderNotes" TEXT NULL,
    "ShippedDate" TEXT NULL,
    "ShippingMethod" TEXT NULL,
    "ShippingCost" decimal(18,2) NOT NULL,
    CONSTRAINT "FK_Orders_BookShops_BookShopId" FOREIGN KEY ("BookShopId") REFERENCES "BookShops" ("BookShopId") ON DELETE RESTRICT,
    CONSTRAINT "FK_Orders_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("CustomerId") ON DELETE RESTRICT
)

CREATE TABLE "Shops" (
    "ShopId" INTEGER NOT NULL CONSTRAINT "PK_Shops" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Location" TEXT NOT NULL,
    "Address" TEXT NULL,
    "Phone" TEXT NULL,
    "Email" TEXT NULL,
    "Website" TEXT NULL,
    "OpeningYear" INTEGER NULL
)

CREATE TABLE sqlite_sequence(name,seq)