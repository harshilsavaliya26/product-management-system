-- Run this in SQL Server Management Studio.
-- Database used by the project: pr3

IF DB_ID('pr3') IS NULL
    CREATE DATABASE pr3;
GO

USE pr3;
GO

IF OBJECT_ID('dbo.register', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.register
    (
        id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        name VARCHAR(100) NOT NULL,
        email VARCHAR(150) NOT NULL,
        password VARCHAR(100) NOT NULL
    );
END
GO

-- Optional: make email unique if your existing data has no duplicate emails.
-- Run only after removing duplicate email rows.
-- CREATE UNIQUE INDEX UX_register_email ON dbo.register(email);
GO
