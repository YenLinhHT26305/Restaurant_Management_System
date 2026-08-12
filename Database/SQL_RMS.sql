USE RMS;
GO

CREATE TABLE users (
    userID INT PRIMARY KEY IDENTITY,
    username VARCHAR(50) NOT NULL,
    upass VARCHAR(10) NOT NULL,
    uName VARCHAR(50) NOT NULL,
    uphone VARCHAR(20)
);

CREATE TABLE category (
    catID INT PRIMARY KEY IDENTITY,
    catName NVARCHAR(50)
);
INSERT INTO category(catName) VALUES (N'Main Dishes');
INSERT INTO category(catName) VALUES (N'Sandwiches');
INSERT INTO category(catName) VALUES (N'Light Meals');
INSERT INTO category(catName) VALUES (N'Beverages');

CREATE TABLE staff (
    staffID INT PRIMARY KEY IDENTITY,
    sName NVARCHAR(50),
    sGender NVARCHAR(50),
    sDateOfBirth DATE,
    sPhone NVARCHAR(50),
    sRole NVARCHAR(50)
);

CREATE TABLE products (
    pID INT PRIMARY KEY IDENTITY,       
    pName NVARCHAR(50),                
    pPrice INT,              
    CategoryID INT,
    pImage IMAGE,
    FOREIGN KEY (CategoryID) REFERENCES category(catID)
);


CREATE TABLE tables (
    tID INT PRIMARY KEY IDENTITY,
    tName NVARCHAR(50) NOT NULL,
    tStatus NVARCHAR(20) DEFAULT N'Available',
    tSeats INT
);

CREATE TABLE tblMain (
    MainID INT PRIMARY KEY IDENTITY,
   orderTime DATETIME,
    TableName NVARCHAR(10),
    WaiterName NVARCHAR(15),
    status VARCHAR(15),
    orderType NVARCHAR(15),
    total INT,
    received INT,
    change INT
);

Create table tblDetails(
	DetailID INT PRIMARY KEY IDENTITY,
    MainID INT,
    proID INT,
    qty INT,
    price INT,
    amount INT
);
