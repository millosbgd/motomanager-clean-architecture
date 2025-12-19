-- Kreiranje tabele Korisnik
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
