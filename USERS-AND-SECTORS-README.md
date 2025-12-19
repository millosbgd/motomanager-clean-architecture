# Implementacija sistema korisnika i sektora

## Šta je urađeno

### 1. Nove tabele u bazi podataka

#### Sektor tabela
- **Id** (INT, Primary Key, Identity)
- **Naziv** (NVARCHAR(100), NOT NULL)
- **CreatedAt** (DATETIME2, NOT NULL)
- **EditedAt** (DATETIME2, NULL)

#### Korisnik tabela
- **Id** (NVARCHAR(255), Primary Key) - Auth0 User ID
- **ImePrezime** (NVARCHAR(200), NOT NULL)
- **UserName** (NVARCHAR(100), NOT NULL)
- **SektorId** (INT, NOT NULL, Foreign Key -> Sektor)
- **CreatedAt** (DATETIME2, NOT NULL)
- **EditedAt** (DATETIME2, NULL)

### 2. Proširenje postojećih tabela

Dodato polje **KorisnikId** (NVARCHAR(255), NULL, Foreign Key -> Korisnik) na:
- **ServiceOrders** - Radni nalozi
- **PurchaseInvoices** - Računi dobavljača

### 3. Backend implementacija

#### Domain layer (MotoManager.Domain)
- `Entities/Sektor.cs`
- `Entities/Korisnik.cs`
- Ažurirani `ServiceOrder.cs` i `PurchaseInvoice.cs` sa KorisnikId poljem

#### Application layer (MotoManager.Application)
- `Sektori/SektorDto.cs` - DTOs i Request objekti
- `Sektori/SektorService.cs` - Business logika
- `Korisnici/KorisnikDto.cs` - DTOs i Request objekti
- `Korisnici/KorisnikService.cs` - Business logika
- `Abstractions/ISektorRepository.cs` - Repository interface
- `Abstractions/IKorisnikRepository.cs` - Repository interface
- Ažurirani servisi za ServiceOrder i PurchaseInvoice da uključuju KorisnikId

#### Infrastructure layer (MotoManager.Infrastructure)
- `Repositories/SektorRepository.cs` - Dapper implementacija
- `Repositories/KorisnikRepository.cs` - Dapper implementacija
- Ažuriran `AppDbContext.cs` sa DbSet-ovima i konfiguracijama
- Ažurirani repositoriji za ServiceOrder i PurchaseInvoice

#### API layer (MotoManager.Api)
- `Controllers/SektoriController.cs` - CRUD operacije za sektore
- `Controllers/KorisniciController.cs` - CRUD operacije za korisnike
- Ažuriran `Program.cs` sa registracijom novih servisa

### 4. SQL skripte

Kreirane skripte za migraciju:
- `create-sektor-table.sql`
- `create-korisnik-table.sql`
- `alter-service-orders-add-korisnik-fk.sql`
- `alter-purchase-invoices-add-korisnik-fk.sql`
- `add-users-and-sectors-migration.sql` - **kompletna skripta sa provjerama**

## Kako pokrenuti migraciju

### Opcija 1: Kompletna skripta (Preporučeno)
```sql
-- Izvršiti u SQL Server Management Studio ili Azure Data Studio
-- Ova skripta proverava postojanje tabela i kolona pre kreiranja
-- Lokacija: sql/add-users-and-sectors-migration.sql
```

### Opcija 2: Pojedinačne skripte
```sql
-- Izvršiti redom:
1. sql/create-sektor-table.sql
2. sql/create-korisnik-table.sql
3. sql/alter-service-orders-add-korisnik-fk.sql
4. sql/alter-purchase-invoices-add-korisnik-fk.sql
```

## API Endpoints

### Sektori
- `GET /api/sektori` - Lista svih sektora
- `GET /api/sektori/{id}` - Detalji sektora
- `POST /api/sektori` - Kreiranje novog sektora
- `PUT /api/sektori/{id}` - Ažuriranje sektora
- `DELETE /api/sektori/{id}` - Brisanje sektora

### Korisnici
- `GET /api/korisnici` - Lista svih korisnika
- `GET /api/korisnici/{id}` - Detalji korisnika (po Auth0 ID)
- `GET /api/korisnici/by-username/{userName}` - Detalji korisnika po username-u
- `GET /api/korisnici/exists/{id}` - Provera postojanja korisnika
- `POST /api/korisnici` - Kreiranje novog korisnika
- `PUT /api/korisnici/{id}` - Ažuriranje korisnika
- `DELETE /api/korisnici/{id}` - Brisanje korisnika

## Sledeći koraci

### Frontend implementacija
1. Kreirati Angular komponente za upravljanje sektorima
2. Kreirati Angular komponente za upravljanje korisnicima
3. Integrisati Auth0 podatke sa lokalnom tabelom korisnika
4. Dodati middleware/interceptor koji automatski dodaje trenutnog korisnika na dokumente

### Auth0 integracija
1. Pri logovanju korisnika, proveriti da li korisnik postoji u lokalnoj bazi
2. Ako ne postoji, preusmeriti na stranicu za registraciju/inicijalizaciju
3. Ako postoji, učitati podatke korisnika i čuvati u session/state
4. Pri kreiranju dokumenata (radni nalozi, računi), automatski dodati `KorisnikId` trenutnog korisnika

### Middleware za automatsko dodavanje korisnika
```csharp
// Kreirati middleware koji iz JWT tokena izvlači Auth0 ID
// i automatski dodaje na sve Create/Update zahteve
```

## Struktura podataka

### Inicijalni sektori
- Mehanika
- Elektrika
- Administracija
- Vulkanizer
- Limar

Ovi sektori se automatski kreiraju tokom migracije.
