MotoManager - Clean Architecture Skeleton (backend)

Projekti:
- MotoManager.Domain         -> entiteti (npr. Vehicle)
- MotoManager.Application    -> interfejsi, DTO-i, poslovna logika (VehicleService)
- MotoManager.Infrastructure -> EF Core, DbContext, repozitorijumi
- MotoManager.Api            -> Web API (kontroleri, Program.cs)

Kako da pokreneš:

1) Otvori solution:
   - U Visual Studio: otvori MotoManager.sln
   - Ili u terminalu idi u ovaj folder.

2) Podesi konekcioni string u MotoManager.Api/appsettings.json:

   "ConnectionStrings": {
     "DefaultConnection": "Server=MILOS-LAPTOP;Database=MotoManager;Trusted_Connection=True;TrustServerCertificate=True;"
   }

   Ili stavi svoj SQL login.

3) Napravi bazu i tabelu (ako još nije):

   U SQL Server Management Studio:

   CREATE DATABASE MotoManager;
   GO
   USE MotoManager;
   GO
   CREATE TABLE Vehicles (
       Id INT IDENTITY(1,1) PRIMARY KEY,
       Model NVARCHAR(100) NOT NULL,
       Plate NVARCHAR(20) NOT NULL
   );

   (Ili koristi EF migracije iz MotoManager.Infrastructure projekta.)

4) Pokreni API:

   U terminalu:

   dotnet build
   dotnet run --project MotoManager.Api

   API će raditi na http://localhost:5000 (ili slično).

5) Frontend (Angular) možeš da povežeš tako da gađa:
   http://localhost:5000/api/vehicles
