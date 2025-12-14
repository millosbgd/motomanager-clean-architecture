-- Kreiranje tabele Materials (šifarnik materijala)
CREATE TABLE Materials (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Naziv NVARCHAR(200) NOT NULL,
    JedinicnaCena DECIMAL(18, 2) NOT NULL
);

-- Kreiranje indeksa
CREATE INDEX IX_Materials_Naziv ON Materials(Naziv);
