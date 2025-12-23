-- Stored procedure for getting materials with pagination
CREATE OR ALTER PROCEDURE [dbo].[sp_GetMaterialsPaged]
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    DECLARE @TotalCount INT;
    
    SELECT @TotalCount = COUNT(*) FROM Materials;
    
    SELECT 
        Id,
        Naziv,
        --JedinicaMere,
        JedinicnaCena,    -- promenio naziv polja
        @TotalCount AS TotalCount,
        @PageNumber AS CurrentPage,
        @PageSize AS PageSize,
        CEILING(CAST(@TotalCount AS FLOAT) / @PageSize) AS TotalPages
    FROM Materials
    ORDER BY Naziv
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
