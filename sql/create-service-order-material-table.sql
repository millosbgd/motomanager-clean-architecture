-- Kreiranje tabele ServiceOrderMaterials
CREATE TABLE ServiceOrderMaterials (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ServiceOrderId INT NOT NULL,
    Naziv NVARCHAR(200) NOT NULL,
    Kolicina DECIMAL(18, 2) NOT NULL,
    JedinicnaCena DECIMAL(18, 2) NOT NULL,
    UkupnaCena DECIMAL(18, 2) NOT NULL,
    
    -- Foreign key constraint
    CONSTRAINT FK_ServiceOrderMaterials_ServiceOrders FOREIGN KEY (ServiceOrderId) 
        REFERENCES ServiceOrders(Id) ON DELETE CASCADE
);

-- Kreiranje indeksa
CREATE INDEX IX_ServiceOrderMaterials_ServiceOrderId ON ServiceOrderMaterials(ServiceOrderId);
