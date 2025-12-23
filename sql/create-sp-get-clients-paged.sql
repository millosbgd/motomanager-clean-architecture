-- Stored procedure for getting clients with pagination
CREATE OR ALTER PROCEDURE [dbo].[sp_GetClientsPaged]
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    DECLARE @TotalCount INT;
    
    SELECT @TotalCount = COUNT(*) FROM Clients;
    
    SELECT 
        Id,
        Naziv,
        Adresa,
        Telefon,
        Email,
        PIB,
        --MaticniBroj,
        @TotalCount AS TotalCount,
        @PageNumber AS CurrentPage,
        @PageSize AS PageSize,
        CEILING(CAST(@TotalCount AS FLOAT) / @PageSize) AS TotalPages
    FROM Clients
    ORDER BY Naziv
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
