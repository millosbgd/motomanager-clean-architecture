-- Dodavanje kolone KorisnikId u tabelu ServiceOrders
ALTER TABLE ServiceOrders
ADD KorisnikId NVARCHAR(255) NULL;

-- Dodavanje Foreign Key constrainta
ALTER TABLE ServiceOrders
ADD CONSTRAINT FK_ServiceOrders_Korisnik
FOREIGN KEY (KorisnikId) REFERENCES Korisnik(Id);

-- Kreiranje indeksa za brže pretraživanje
CREATE INDEX IX_ServiceOrders_KorisnikId ON ServiceOrders(KorisnikId);
