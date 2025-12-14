-- Kreiranje tabele ServiceOrderLabors
CREATE TABLE ServiceOrderLabors (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ServiceOrderId INT NOT NULL,
    OpisRadova NVARCHAR(500) NOT NULL,
    UkupnoVreme DECIMAL(18, 2) NOT NULL,
    Cena DECIMAL(18, 2) NOT NULL,
    
    -- Foreign key constraint
    CONSTRAINT FK_ServiceOrderLabors_ServiceOrders FOREIGN KEY (ServiceOrderId) 
        REFERENCES ServiceOrders(Id) ON DELETE CASCADE
);

-- Kreiranje indeksa
CREATE INDEX IX_ServiceOrderLabors_ServiceOrderId ON ServiceOrderLabors(ServiceOrderId);
