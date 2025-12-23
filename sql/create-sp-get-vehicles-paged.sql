-- Stored procedure for getting vehicles with pagination
CREATE OR ALTER PROCEDURE [dbo].[sp_GetVehiclesPaged]
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    DECLARE @TotalCount INT;
    
    SELECT @TotalCount = COUNT(*) FROM Vehicles;
    
    SELECT 
        v.Id,
        v.Plate,
        v.Model,
        v.Year,
        v.ClientId,
        c.Naziv AS ClientNaziv,
        @TotalCount AS TotalCount,
        @PageNumber AS CurrentPage,
        @PageSize AS PageSize,
        CEILING(CAST(@TotalCount AS FLOAT) / @PageSize) AS TotalPages
    FROM Vehicles v
    LEFT JOIN Clients c ON v.ClientId = c.Id
    ORDER BY v.Plate
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
