using Microsoft.Data.SqlClient;
using MotoManager.Domain.Entities;
using MotoManager.Application.Abstractions;
using Dapper;

namespace MotoManager.Infrastructure.Repositories;

public class SektorRepository : ISektorRepository
{
    private readonly string _connectionString;

    public SektorRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Sektor>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        var query = "SELECT * FROM Sektor ORDER BY Naziv";
        return await connection.QueryAsync<Sektor>(query);
    }

    public async Task<Sektor?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = "SELECT * FROM Sektor WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<Sektor>(query, new { Id = id });
    }

    public async Task<Sektor> CreateAsync(Sektor sektor)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = @"
            INSERT INTO Sektor (Naziv, CreatedAt)
            VALUES (@Naziv, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int);";
        
        sektor.CreatedAt = DateTime.UtcNow;
        var id = await connection.ExecuteScalarAsync<int>(query, sektor);
        sektor.Id = id;
        return sektor;
    }

    public async Task<Sektor?> UpdateAsync(Sektor sektor)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = @"
            UPDATE Sektor
            SET Naziv = @Naziv,
                EditedAt = @EditedAt
            WHERE Id = @Id";
        
        sektor.EditedAt = DateTime.UtcNow;
        var rowsAffected = await connection.ExecuteAsync(query, sektor);
        
        return rowsAffected > 0 ? sektor : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = "DELETE FROM Sektor WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
        return rowsAffected > 0;
    }
}
