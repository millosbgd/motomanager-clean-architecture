-- Stored procedure for getting purchase invoices with filtering and pagination
CREATE OR ALTER PROCEDURE [dbo].[sp_GetPurchaseInvoicesPaged]
    @DatumOd DATE = NULL,
    @DatumDo DATE = NULL,
    @DobavljacId INT = NULL,
    @VoziloId INT = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Calculate offset for pagination
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    -- Get total count
    DECLARE @TotalCount INT;
    
    SELECT @TotalCount = COUNT(*)
    FROM PurchaseInvoices pi
    WHERE (@DatumOd IS NULL OR pi.Datum >= @DatumOd)
        AND (@DatumDo IS NULL OR pi.Datum <= @DatumDo)
        AND (@DobavljacId IS NULL OR pi.DobavljacId = @DobavljacId)
        AND (@VoziloId IS NULL OR pi.VoziloId = @VoziloId);
    
    -- Get paged results with sorting (newest first)
    SELECT 
        pi.Id,
        pi.BrojRacuna,
        pi.Datum,
        pi.DobavljacId,
        pi.VoziloId,
        pi.IznosNeto,
        pi.IznosPDV,
        pi.IznosBruto,
        pi.KorisnikId,
        pi.SektorId,
        d.Naziv AS DobavljacNaziv,
        v.RegistarskaOznaka AS VoziloRegistarskaOznaka,
        k.Ime AS KorisnikIme,
        k.Prezime AS KorisnikPrezime,
        s.Naziv AS SektorNaziv,
        @TotalCount AS TotalCount,
        @PageNumber AS CurrentPage,
        @PageSize AS PageSize,
        CEILING(CAST(@TotalCount AS FLOAT) / @PageSize) AS TotalPages
    FROM PurchaseInvoices pi
    LEFT JOIN Clients d ON pi.DobavljacId = d.Id
    LEFT JOIN Vehicles v ON pi.VoziloId = v.Id
    LEFT JOIN Korisnik k ON pi.KorisnikId = k.Id
    LEFT JOIN Sektor s ON pi.SektorId = s.Id
    WHERE (@DatumOd IS NULL OR pi.Datum >= @DatumOd)
        AND (@DatumDo IS NULL OR pi.Datum <= @DatumDo)
        AND (@DobavljacId IS NULL OR pi.DobavljacId = @DobavljacId)
        AND (@VoziloId IS NULL OR pi.VoziloId = @VoziloId)
    ORDER BY pi.Datum DESC, pi.Id DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
