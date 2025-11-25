-- 1️⃣ Create the database
CREATE DATABASE ProductsApi;
GO


USE ProductsApi;
GO

CREATE TABLE products (
    id INT IDENTITY(1,1) PRIMARY KEY,
    productName NVARCHAR(100) NOT NULL,
    categorie NVARCHAR(50) CHECK (categorie IN ('Electronics', 'Clothing', 'Books')) NOT NULL,
    price DECIMAL(10,2) NOT NULL
);
GO


INSERT INTO products (productName, categorie, price) VALUES
('Smartphone Samsung Galaxy S24', 'Electronics', 899.99),
('Men''s Cotton T-Shirt', 'Clothing', 19.99),
('Deep Learning with Python', 'Books', 45.50);
GO

SELECT * FROM products;
GO
