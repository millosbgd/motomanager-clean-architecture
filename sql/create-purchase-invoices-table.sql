-- SQL skripta za kreiranje PurchaseInvoices tabele

USE MotoManager;
GO

-- Kreiranje PurchaseInvoices tabele
CREATE TABLE PurchaseInvoices (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BrojRacuna NVARCHAR(100) NOT NULL,
    Datum DATETIME NOT NULL,
    DobavljacId INT NOT NULL,
    VoziloId INT NULL,
    IznosNeto DECIMAL(18,2) NOT NULL,
    IznosPDV DECIMAL(18,2) NOT NULL,
    IznosBruto DECIMAL(18,2) NOT NULL,
    
    CONSTRAINT FK_PurchaseInvoices_Clients FOREIGN KEY (DobavljacId) 
        REFERENCES Clients(Id),
    CONSTRAINT FK_PurchaseInvoices_Vehicles FOREIGN KEY (VoziloId) 
        REFERENCES Vehicles(Id)
);
GO

-- Kreiranje indeksa za bolje performanse
CREATE INDEX IX_PurchaseInvoices_DobavljacId ON PurchaseInvoices(DobavljacId);
CREATE INDEX IX_PurchaseInvoices_VoziloId ON PurchaseInvoices(VoziloId);
CREATE INDEX IX_PurchaseInvoices_Datum ON PurchaseInvoices(Datum);
GO
