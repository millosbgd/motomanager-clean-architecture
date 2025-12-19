-- Dodavanje kolone KorisnikId u tabelu PurchaseInvoices
ALTER TABLE PurchaseInvoices
ADD KorisnikId NVARCHAR(255) NULL;

-- Dodavanje Foreign Key constrainta
ALTER TABLE PurchaseInvoices
ADD CONSTRAINT FK_PurchaseInvoices_Korisnik
FOREIGN KEY (KorisnikId) REFERENCES Korisnik(Id);

-- Kreiranje indeksa za brže pretraživanje
CREATE INDEX IX_PurchaseInvoices_KorisnikId ON PurchaseInvoices(KorisnikId);
