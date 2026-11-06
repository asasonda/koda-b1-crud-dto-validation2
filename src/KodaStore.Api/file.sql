-- SELECT name from sys.databases;

-- create DATABASE eccomercedb

-- select DB_NAME() as CurrentDatabases;

drop TABLE Product;
drop Table Customers

CREATE TABLE Product (
    ProductId int IDENTITY(1,1) PRIMARY KEY,
    ProductCode NVARCHAR(20) not NULL UNIQUE,
    Name NVARCHAR(100) not NULL,
    Price DECIMAL(18,2) not NULL,
    Stock int NOT NULL DEFAULT 0,
    Category NVARCHAR(50) not NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CustomerId int NOT NULL,

    CONSTRAINT FK_Products_Customers FOREIGN key (CustomerId) REFERENCES Customers(CustomerId) on DELETE CASCADE on UPDATE CASCADE,
    CONSTRAINT CK_Products_Price CHECK(Price >= 0),
    CONSTRAINT CK_Product_Stock CHECK(Stock >= 0)
)

CREATE TABLE Customers(
    CustomerId int IDENTITY(1,1) PRIMARY KEY,
    CustomerCode NVARCHAR(20) not NULL UNIQUE,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT null UNIQUE,
    Phone NVARCHAR(30) NOT NULL,
    Address NVARCHAR(255)
)

-- cek semua tabel di database
SELECT name as tabel from sys.tables;

INSERT into dbo.Customers(CustomerCode, Name, Email, Phone, Address) VALUES
('CS001','Andi', 'andi@gmail.com', '081344556677', 'kab.bogor')

INSERT into dbo.Product (ProductCode, Name, Price, Stock, Category, CustomerId) VALUES
('PR001', 'Keyboard', 28000, 48, 'Elektronik', 1),
('PR002', 'Mouse', 10000, 10, 'Elektronik', 1);

SELECT * FROM dbo.Product;