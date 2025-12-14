-- =====================================================
-- Azure SQL Database - Complete Migration Script
-- MotoManager Database
-- =====================================================

-- Drop existing tables if they exist (in correct order due to foreign keys)
IF OBJECT_ID('ServiceOrderMaterials', 'U') IS NOT NULL
    DROP TABLE ServiceOrderMaterials;

IF OBJECT_ID('ServiceOrderLabors', 'U') IS NOT NULL
    DROP TABLE ServiceOrderLabors;

IF OBJECT_ID('ServiceOrders', 'U') IS NOT NULL
    DROP TABLE ServiceOrders;

IF OBJECT_ID('Vehicles', 'U') IS NOT NULL
    DROP TABLE Vehicles;

IF OBJECT_ID('Clients', 'U') IS NOT NULL
    DROP TABLE Clients;

IF OBJECT_ID('Materials', 'U') IS NOT NULL
    DROP TABLE Materials;

GO

-- =====================================================
-- Create Tables
-- =====================================================

-- Clients Table
CREATE TABLE Clients (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Naziv NVARCHAR(200) NOT NULL,
    PIB NVARCHAR(20),
    Adresa NVARCHAR(300),
    Grad NVARCHAR(100),
    Telefon NVARCHAR(50),
    Email NVARCHAR(100)
);

-- Materials Table (Master Data)
CREATE TABLE Materials (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Naziv NVARCHAR(200) NOT NULL,
    JedinicnaCena DECIMAL(18,2) NOT NULL
);

-- Vehicles Table
CREATE TABLE Vehicles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Model NVARCHAR(100) NOT NULL,
    Plate NVARCHAR(20) NOT NULL,
    ClientId INT NOT NULL,
    CONSTRAINT FK_Vehicles_Clients FOREIGN KEY (ClientId) 
        REFERENCES Clients(Id) ON DELETE NO ACTION
);

-- ServiceOrders Table
CREATE TABLE ServiceOrders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BrojNaloga NVARCHAR(50) NOT NULL,
    Datum DATETIME2 NOT NULL,
    VehicleId INT NOT NULL,
    ClientId INT NOT NULL,
    Kilometraza INT,
    OpisRada NVARCHAR(MAX),
    CONSTRAINT FK_ServiceOrders_Vehicles FOREIGN KEY (VehicleId) 
        REFERENCES Vehicles(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_ServiceOrders_Clients FOREIGN KEY (ClientId) 
        REFERENCES Clients(Id) ON DELETE NO ACTION
);

-- ServiceOrderLabors Table
CREATE TABLE ServiceOrderLabors (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ServiceOrderId INT NOT NULL,
    Naziv NVARCHAR(200) NOT NULL,
    Kolicina DECIMAL(18,2) NOT NULL,
    JedinicnaCena DECIMAL(18,2) NOT NULL,
    UkupnaCena DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_ServiceOrderLabors_ServiceOrders FOREIGN KEY (ServiceOrderId) 
        REFERENCES ServiceOrders(Id) ON DELETE CASCADE
);

-- ServiceOrderMaterials Table
CREATE TABLE ServiceOrderMaterials (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ServiceOrderId INT NOT NULL,
    MaterialId INT NOT NULL,
    Kolicina DECIMAL(18,2) NOT NULL,
    JedinicnaCena DECIMAL(18,2) NOT NULL,
    UkupnaCena DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_ServiceOrderMaterials_ServiceOrders FOREIGN KEY (ServiceOrderId) 
        REFERENCES ServiceOrders(Id) ON DELETE CASCADE,
    CONSTRAINT FK_ServiceOrderMaterials_Materials FOREIGN KEY (MaterialId) 
        REFERENCES Materials(Id) ON DELETE NO ACTION
);

GO

-- =====================================================
-- Create Indexes
-- =====================================================

CREATE INDEX IX_Clients_Naziv ON Clients(Naziv);
CREATE INDEX IX_Materials_Naziv ON Materials(Naziv);
CREATE INDEX IX_Vehicles_ClientId ON Vehicles(ClientId);
CREATE INDEX IX_Vehicles_Plate ON Vehicles(Plate);
CREATE INDEX IX_ServiceOrders_VehicleId ON ServiceOrders(VehicleId);
CREATE INDEX IX_ServiceOrders_ClientId ON ServiceOrders(ClientId);
CREATE INDEX IX_ServiceOrders_Datum ON ServiceOrders(Datum);
CREATE INDEX IX_ServiceOrderLabors_ServiceOrderId ON ServiceOrderLabors(ServiceOrderId);
CREATE INDEX IX_ServiceOrderMaterials_ServiceOrderId ON ServiceOrderMaterials(ServiceOrderId);
CREATE INDEX IX_ServiceOrderMaterials_MaterialId ON ServiceOrderMaterials(MaterialId);

GO

-- =====================================================
-- Verification Query
-- =====================================================

SELECT 
    'Tables Created' AS Status,
    COUNT(*) AS TableCount
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
    AND TABLE_NAME IN ('Clients', 'Materials', 'Vehicles', 'ServiceOrders', 'ServiceOrderLabors', 'ServiceOrderMaterials');

GO
