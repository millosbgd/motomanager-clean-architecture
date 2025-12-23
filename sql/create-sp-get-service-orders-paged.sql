-- Stored procedure for getting service orders with pagination
CREATE OR ALTER PROCEDURE [dbo].[sp_GetServiceOrdersPaged]
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    DECLARE @TotalCount INT;
    
    SELECT @TotalCount = COUNT(*) FROM ServiceOrders;
    
    SELECT 
        so.Id,
        so.BrojNaloga,
        so.Datum,
        so.ClientId,
        so.VehicleId,
        so.OpisRada,
        so.Kilometraza,
        v.Plate AS VoziloRegistarskaOznaka,
        v.Model AS VoziloModel,
        @TotalCount AS TotalCount,
        @PageNumber AS CurrentPage,
        @PageSize AS PageSize,
        CEILING(CAST(@TotalCount AS FLOAT) / @PageSize) AS TotalPages
    FROM ServiceOrders so
    LEFT JOIN Vehicles v ON so.VehicleId = v.Id
    ORDER BY so.Datum DESC, so.Id DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
