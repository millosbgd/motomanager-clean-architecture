using MotoManager.Domain.Entities;

namespace MotoManager.Application.Abstractions;

public interface IKorisnikRepository
{
    Task<IEnumerable<Korisnik>> GetAllAsync();
    Task<Korisnik?> GetByIdAsync(string id);
    Task<Korisnik?> GetByUserNameAsync(string userName);
    Task<Korisnik> CreateAsync(Korisnik korisnik);
    Task<Korisnik?> UpdateAsync(Korisnik korisnik);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
}
