-- Kreiranje tabele Sektor
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
