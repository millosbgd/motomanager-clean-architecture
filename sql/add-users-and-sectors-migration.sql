-- ==========================================
-- SKRIPTA ZA DODAVANJE KORISNIKA I SEKTORA
-- ==========================================

-- 1. Kreiranje tabele Sektor
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sektor]') AND type in (N'U'))
BEGIN
    CREATE TABLE Sektor (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Naziv NVARCHAR(100) NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        EditedAt DATETIME2 NULL
    );

    -- Indeks za brže pretraživanje po nazivu
    CREATE INDEX IX_Sektor_Naziv ON Sektor(Naziv);

    -- Primer inicijalnih podataka
    INSERT INTO Sektor (Naziv) VALUES 
    ('Mehanika'),
    ('Elektrika'),
    ('Administracija'),
    ('Vulkanizer'),
    ('Limar');
    
    PRINT 'Tabela Sektor kreirana uspešno.';
END
ELSE
BEGIN
    PRINT 'Tabela Sektor već postoji.';
END
GO

-- 2. Kreiranje tabele Korisnik
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Korisnik]') AND type in (N'U'))
BEGIN
    CREATE TABLE Korisnik (
        Id NVARCHAR(255) PRIMARY KEY, -- Auth0 User ID
        ImePrezime NVARCHAR(200) NOT NULL,
        UserName NVARCHAR(100) NOT NULL,
        SektorId INT NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        EditedAt DATETIME2 NULL,
        CONSTRAINT FK_Korisnik_Sektor FOREIGN KEY (SektorId) REFERENCES Sektor(Id)
    );

    -- Indeksi za brže pretraživanje
    CREATE INDEX IX_Korisnik_UserName ON Korisnik(UserName);
    CREATE INDEX IX_Korisnik_SektorId ON Korisnik(SektorId);
    
    PRINT 'Tabela Korisnik kreirana uspešno.';
END
ELSE
BEGIN
    PRINT 'Tabela Korisnik već postoji.';
END
GO

-- 3. Dodavanje kolone KorisnikId u tabelu ServiceOrders
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[ServiceOrders]') 
    AND name = 'KorisnikId'
)
BEGIN
    ALTER TABLE ServiceOrders
    ADD KorisnikId NVARCHAR(255) NULL;

    -- Dodavanje Foreign Key constrainta
    ALTER TABLE ServiceOrders
    ADD CONSTRAINT FK_ServiceOrders_Korisnik
    FOREIGN KEY (KorisnikId) REFERENCES Korisnik(Id);

    -- Kreiranje indeksa za brže pretraživanje
    CREATE INDEX IX_ServiceOrders_KorisnikId ON ServiceOrders(KorisnikId);
    
    PRINT 'Kolona KorisnikId dodata u tabelu ServiceOrders.';
END
ELSE
BEGIN
    PRINT 'Kolona KorisnikId već postoji u tabeli ServiceOrders.';
END
GO

-- 4. Dodavanje kolone KorisnikId u tabelu PurchaseInvoices
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[PurchaseInvoices]') 
    AND name = 'KorisnikId'
)
BEGIN
    ALTER TABLE PurchaseInvoices
    ADD KorisnikId NVARCHAR(255) NULL;

    -- Dodavanje Foreign Key constrainta
    ALTER TABLE PurchaseInvoices
    ADD CONSTRAINT FK_PurchaseInvoices_Korisnik
    FOREIGN KEY (KorisnikId) REFERENCES Korisnik(Id);

    -- Kreiranje indeksa za brže pretraživanje
    CREATE INDEX IX_PurchaseInvoices_KorisnikId ON PurchaseInvoices(KorisnikId);
    
    PRINT 'Kolona KorisnikId dodata u tabelu PurchaseInvoices.';
END
ELSE
BEGIN
    PRINT 'Kolona KorisnikId već postoji u tabeli PurchaseInvoices.';
END
GO

PRINT '==========================================';
PRINT 'Migracija za korisnike i sektore završena!';
PRINT '==========================================';
