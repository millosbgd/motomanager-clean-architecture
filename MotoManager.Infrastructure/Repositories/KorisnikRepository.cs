using Microsoft.Data.SqlClient;
using MotoManager.Domain.Entities;
using MotoManager.Application.Abstractions;
using Dapper;

namespace MotoManager.Infrastructure.Repositories;

public class KorisnikRepository : IKorisnikRepository
{
    private readonly string _connectionString;

    public KorisnikRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Korisnik>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        var query = @"
            SELECT k.*, s.Naziv as SektorNaziv
            FROM Korisnik k
            LEFT JOIN Sektor s ON k.SektorId = s.Id
            ORDER BY k.ImePrezime";
        
        var korisnici = await connection.QueryAsync<Korisnik, string, Korisnik>(
            query,
            (korisnik, sektorNaziv) =>
            {
                if (korisnik.Sektor == null)
                    korisnik.Sektor = new Sektor();
                korisnik.Sektor.Naziv = sektorNaziv ?? string.Empty;
                return korisnik;
            },
            splitOn: "SektorNaziv"
        );
        
        return korisnici;
    }

    public async Task<Korisnik?> GetByIdAsync(string id)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = @"
            SELECT k.*, s.Naziv as SektorNaziv
            FROM Korisnik k
            LEFT JOIN Sektor s ON k.SektorId = s.Id
            WHERE k.Id = @Id";
        
        var korisnici = await connection.QueryAsync<Korisnik, string, Korisnik>(
            query,
            (korisnik, sektorNaziv) =>
            {
                if (korisnik.Sektor == null)
                    korisnik.Sektor = new Sektor();
                korisnik.Sektor.Naziv = sektorNaziv ?? string.Empty;
                return korisnik;
            },
            new { Id = id },
            splitOn: "SektorNaziv"
        );
        
        return korisnici.FirstOrDefault();
    }

    public async Task<Korisnik?> GetByUserNameAsync(string userName)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = @"
            SELECT k.*, s.Naziv as SektorNaziv
            FROM Korisnik k
            LEFT JOIN Sektor s ON k.SektorId = s.Id
            WHERE k.UserName = @UserName";
        
        var korisnici = await connection.QueryAsync<Korisnik, string, Korisnik>(
            query,
            (korisnik, sektorNaziv) =>
            {
                if (korisnik.Sektor == null)
                    korisnik.Sektor = new Sektor();
                korisnik.Sektor.Naziv = sektorNaziv ?? string.Empty;
                return korisnik;
            },
            new { UserName = userName },
            splitOn: "SektorNaziv"
        );
        
        return korisnici.FirstOrDefault();
    }

    public async Task<Korisnik> CreateAsync(Korisnik korisnik)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = @"
            INSERT INTO Korisnik (Id, ImePrezime, UserName, SektorId, CreatedAt)
            VALUES (@Id, @ImePrezime, @UserName, @SektorId, @CreatedAt)";
        
        korisnik.CreatedAt = DateTime.UtcNow;
        await connection.ExecuteAsync(query, korisnik);
        return korisnik;
    }

    public async Task<Korisnik?> UpdateAsync(Korisnik korisnik)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = @"
            UPDATE Korisnik
            SET ImePrezime = @ImePrezime,
                UserName = @UserName,
                SektorId = @SektorId,
                EditedAt = @EditedAt
            WHERE Id = @Id";
        
        korisnik.EditedAt = DateTime.UtcNow;
        var rowsAffected = await connection.ExecuteAsync(query, korisnik);
        
        return rowsAffected > 0 ? korisnik : null;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = "DELETE FROM Korisnik WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
        return rowsAffected > 0;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = "SELECT COUNT(1) FROM Korisnik WHERE Id = @Id";
        var count = await connection.ExecuteScalarAsync<int>(query, new { Id = id });
        return count > 0;
    }
}
