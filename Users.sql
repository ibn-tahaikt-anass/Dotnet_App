-- Create the Users table
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,       -- Auto-increment unique ID
    UserName NVARCHAR(100) NOT NULL,        -- User name
    Email NVARCHAR(255) NOT NULL UNIQUE,    -- Unique email
    Password NVARCHAR(255) NOT NULL,        -- User password (hashed in real apps)
    Role NVARCHAR(50) CHECK (Role IN ('Employee', 'Admin')) NOT NULL  -- Restrict to valid roles
);

-- Insert an Admin user
INSERT INTO Users (UserName, Email, Password, Role)
VALUES ('admin_user', 'admin@example.com', 'Admin@123', 'Admin');

-- Insert an Employee user
INSERT INTO Users (UserName, Email, Password, Role)
VALUES ('employee_user', 'employee@example.com', 'Employee@123', 'Employee');



EXEC sp_rename 'Users.Password', 'PasswordHash', 'COLUMN';

ALTER TABLE Users
ADD CONSTRAINT DF_Users_Role DEFAULT 'Employee' FOR Role;

-- View all users
SELECT * FROM Users;