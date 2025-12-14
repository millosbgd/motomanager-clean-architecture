-- Add ClientId foreign key to Vehicles
-- Make sure Clients table exists first!

-- IMPORTANT: This will DELETE all existing data from ServiceOrders, ServiceOrderLabors, ServiceOrderMaterials, and Vehicles
-- If you need to keep the data, you must manually set ClientId first

PRINT 'Deleting existing ServiceOrderMaterials data...';
DELETE FROM ServiceOrderMaterials;

PRINT 'Deleting existing ServiceOrderLabors data...';
DELETE FROM ServiceOrderLabors;

PRINT 'Deleting existing ServiceOrders data...';
DELETE FROM ServiceOrders;

PRINT 'Deleting existing Vehicles data...';
DELETE FROM Vehicles;

-- Add ClientId column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Vehicles') AND name = 'ClientId')
BEGIN
    PRINT 'Adding ClientId column...';
    ALTER TABLE Vehicles
    ADD ClientId INT NOT NULL;
END

-- Add foreign key constraint if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Vehicles_Clients')
BEGIN
    PRINT 'Adding foreign key constraint...';
    ALTER TABLE Vehicles
    ADD CONSTRAINT FK_Vehicles_Clients 
        FOREIGN KEY (ClientId) 
        REFERENCES Clients(Id)
        ON DELETE NO ACTION;
END

-- Create index if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Vehicles_ClientId' AND object_id = OBJECT_ID('Vehicles'))
BEGIN
    PRINT 'Creating index...';
    CREATE INDEX IX_Vehicles_ClientId 
    ON Vehicles(ClientId);
END

PRINT 'Migration completed successfully!';
