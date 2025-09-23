-- Feature: OMS-2 
-- Author: Adalid
-- Purpose: Create a new database
CREATE DATABASE Operation
GO

USE Operation;
CREATE TABLE Operation.dbo.Users (
    UserID UNIQUEIDENTIFIER PRIMARY KEY, -- UserID, must be UNIQUEIDENTIFIER
    UserName NVARCHAR(50) UNIQUE,         -- User name, must be UNIQUE
    PasswordHash NVARCHAR(256) NOT NULL,          -- PasswordHash, required
    isDeleted BIT DEFAULT 0 -- isDeleted, DEFAULT 0 => isDeleted = false
);

CREATE TABLE Operation.dbo.SessionLog (
    SessionID UNIQUEIDENTIFIER PRIMARY KEY, 
    UserID UNIQUEIDENTIFIER,         -- Clave foranea
    InitSession DATETIME NOT NULL,          
    EndSession DATETIME 
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE Operation.dbo.OperationLog (
    OperationID INT IDENTITY(1,1) PRIMARY KEY, 
    SessionID UNIQUEIDENTIFIER,      -- Clave foranea   
    OperationDate DATETIME,         
    Request NVARCHAR,          
    Response NVARCHAR
    FOREIGN KEY (SessionID) REFERENCES SessionLog(SessionID)
);

CREATE TABLE Operation.dbo.Transactions (
    TransactionID INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing primary key
    OperationID INT,                 -- Clave foranea
    UserID UNIQUEIDENTIFIER,         -- Clave foranea
    TransactionName NVARCHAR(50) NOT NULL,         -- Transaction name, required
    Amount DECIMAL(10, 2) NOT NULL,          -- Amount, required
    TransactionDate DATETIME NOT NULL,                  -- Transaction date, required
    isDeleted BIT DEFAULT 0 -- isDeleted, DEFAULT 0 => isDeleted = false
    FOREIGN KEY (OperationID) REFERENCES OperationLog(OperationID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);