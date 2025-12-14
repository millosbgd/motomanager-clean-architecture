-- Kreiranje tabele ServiceOrders
CREATE TABLE ServiceOrders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    BrojNaloga NVARCHAR(50) NOT NULL,
    Datum DATETIME2 NOT NULL,
    ClientId INT NOT NULL,
    VehicleId INT NOT NULL,
    OpisRada NVARCHAR(MAX) NOT NULL,
    Kilometraza INT NOT NULL,
    
    -- Foreign key constraints
    CONSTRAINT FK_ServiceOrders_Clients FOREIGN KEY (ClientId) 
        REFERENCES Clients(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_ServiceOrders_Vehicles FOREIGN KEY (VehicleId) 
        REFERENCES Vehicles(Id) ON DELETE NO ACTION
);

-- Kreiranje indeksa za bolje performanse
CREATE INDEX IX_ServiceOrders_ClientId ON ServiceOrders(ClientId);
CREATE INDEX IX_ServiceOrders_VehicleId ON ServiceOrders(VehicleId);
CREATE INDEX IX_ServiceOrders_Datum ON ServiceOrders(Datum);
CREATE INDEX IX_ServiceOrders_BrojNaloga ON ServiceOrders(BrojNaloga);
