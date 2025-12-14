-- Add MaterialId foreign key to ServiceOrderMaterials
-- Make sure to run create-materials-table.sql first!

-- IMPORTANT: This will DELETE all existing data from ServiceOrderMaterials
-- If you need to keep the data, you must manually map Naziv to MaterialId first

PRINT 'Deleting existing ServiceOrderMaterials data...';
DELETE FROM ServiceOrderMaterials;

-- Drop the Naziv column if it exists
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ServiceOrderMaterials') AND name = 'Naziv')
BEGIN
    PRINT 'Dropping Naziv column...';
    ALTER TABLE ServiceOrderMaterials
    DROP COLUMN Naziv;
END

-- Add MaterialId column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ServiceOrderMaterials') AND name = 'MaterialId')
BEGIN
    PRINT 'Adding MaterialId column...';
    ALTER TABLE ServiceOrderMaterials
    ADD MaterialId INT NOT NULL;
END

-- Add foreign key constraint if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ServiceOrderMaterials_Materials')
BEGIN
    PRINT 'Adding foreign key constraint...';
    ALTER TABLE ServiceOrderMaterials
    ADD CONSTRAINT FK_ServiceOrderMaterials_Materials 
        FOREIGN KEY (MaterialId) 
        REFERENCES Materials(Id)
        ON DELETE NO ACTION;
END

-- Create index if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ServiceOrderMaterials_MaterialId' AND object_id = OBJECT_ID('ServiceOrderMaterials'))
BEGIN
    PRINT 'Creating index...';
    CREATE INDEX IX_ServiceOrderMaterials_MaterialId 
    ON ServiceOrderMaterials(MaterialId);
END

PRINT 'Migration completed successfully!';
