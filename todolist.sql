-- Create the todolist database
CREATE DATABASE todolist;
GO

-- Use the todolist database
USE todolist;
GO

-- Create the Users table
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    DateCreated DATETIME2 DEFAULT GETDATE()
);
GO

-- Create the Tasks table
CREATE TABLE Tasks (
    TaskID INT PRIMARY KEY IDENTITY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    Title NVARCHAR(MAX),
    Description NVARCHAR(MAX),
    IsCompleted BIT DEFAULT 0,
    DueDate DATETIME2,
    DateCreated DATETIME2 DEFAULT GETDATE()
);
GO
