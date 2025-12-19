-- Dodavanje SektorId kolone u purchase_invoices tabelu
ALTER TABLE purchase_invoices
ADD SektorId INT NULL;

-- Dodavanje Foreign Key constraint
ALTER TABLE purchase_invoices
ADD CONSTRAINT FK_PurchaseInvoices_Sektor_SektorId
FOREIGN KEY (SektorId) REFERENCES Sektor(Id);

-- Kreiranje indexa za bolju performance
CREATE INDEX IX_PurchaseInvoices_SektorId ON purchase_invoices(SektorId);
