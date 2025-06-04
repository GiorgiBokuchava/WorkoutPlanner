USE [WorkoutPlanner-db];
GO

-- Create [Identity].Roles
IF OBJECT_ID('[Identity].Roles', 'U') IS NULL
BEGIN
    CREATE TABLE [Identity].Roles (
        id INT IDENTITY(1,1) PRIMARY KEY,
        name VARCHAR(50) NOT NULL UNIQUE
    );
END
GO

-- Create [Identity].UserRoles (join table)
IF OBJECT_ID('[Identity].UserRoles', 'U') IS NULL
BEGIN
    CREATE TABLE [Identity].UserRoles (
        user_id INT NOT NULL
            CONSTRAINT FK_UserRoles_User
                FOREIGN KEY REFERENCES [Identity].Users(id)
                ON DELETE CASCADE,
        role_id INT NOT NULL
            CONSTRAINT FK_UserRoles_Role
                FOREIGN KEY REFERENCES [Identity].Roles(id)
                ON DELETE CASCADE,
            CONSTRAINT PK_UserRoles PRIMARY KEY (user_id, role_id)
    );
END
GO

-- Create [Identity].RefreshTokens (bonus)
IF OBJECT_ID('[Identity].RefreshTokens', 'U') IS NULL
BEGIN
    CREATE TABLE [Identity].RefreshTokens (
        id INT IDENTITY(1,1) PRIMARY KEY,
        user_id INT NOT NULL
            CONSTRAINT FK_RefreshTokens_User
                FOREIGN KEY REFERENCES [Identity].Users(id)
                ON DELETE CASCADE,
        token_hash VARCHAR(128) NOT NULL UNIQUE,
        expires_at DATETIME2 NOT NULL,
        revoked_at DATETIME2 NULL,
        revocation_reason TINYINT NULL  -- 0=logout,1=psswrd-change,2=admin-revoked, etc
    );
END
GO

-- Seed [Identity].Roles
-- Ensure “Admin” and “User” exist once
INSERT INTO [Identity].Roles (name)
SELECT val.RoleName
FROM (VALUES ('Admin'), ('User')) AS val(RoleName)
WHERE NOT EXISTS (
    SELECT 1
    FROM [Identity].Roles r
    WHERE r.name = val.RoleName
);
GO