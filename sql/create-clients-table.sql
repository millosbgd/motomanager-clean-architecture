-- SQL skripta za kreiranje Clients tabele

USE MotoManager;
GO

-- Kreiranje Clients tabele
CREATE TABLE Clients (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Naziv NVARCHAR(200) NOT NULL,
    Adresa NVARCHAR(200) NOT NULL,
    Grad NVARCHAR(100) NOT NULL,
    PIB NVARCHAR(20) NULL,
    Telefon NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL
);
GO

-- Opciono: dodaj test podatke
INSERT INTO Clients (Naziv, Adresa, Grad, PIB, Telefon, Email)
VALUES 
    ('Auto Servis Miloš', 'Bulevar kralja Aleksandra 123', 'Beograd', '123456789', '+381641234567', 'info@automilos.rs'),
    ('Moto Parts DOO', 'Narodnih heroja 45', 'Novi Sad', NULL, '+381213456789', 'kontakt@motoparts.rs'),
    ('Speed Shop', 'Kneza Miloša 77', 'Niš', '987654321', '+381181234567', 'speedshop@gmail.com');
GO
