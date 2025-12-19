using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;

namespace MotoManager.Application.Korisnici;

public class KorisnikService
{
    private readonly IKorisnikRepository _korisnikRepository;

    public KorisnikService(IKorisnikRepository korisnikRepository)
    {
        _korisnikRepository = korisnikRepository;
    }

    public async Task<IEnumerable<KorisnikDto>> GetAllKorisniciAsync()
    {
        var korisnici = await _korisnikRepository.GetAllAsync();
        return korisnici.Select(k => new KorisnikDto
        {
            Id = k.Id,
            ImePrezime = k.ImePrezime,
            UserName = k.UserName,
            SektorId = k.SektorId,
            SektorNaziv = k.Sektor?.Naziv ?? string.Empty,
            CreatedAt = k.CreatedAt,
            EditedAt = k.EditedAt
        });
    }

    public async Task<KorisnikDto?> GetKorisnikByIdAsync(string id)
    {
        var korisnik = await _korisnikRepository.GetByIdAsync(id);
        if (korisnik == null)
            return null;

        return new KorisnikDto
        {
            Id = korisnik.Id,
            ImePrezime = korisnik.ImePrezime,
            UserName = korisnik.UserName,
            SektorId = korisnik.SektorId,
            SektorNaziv = korisnik.Sektor?.Naziv ?? string.Empty,
            CreatedAt = korisnik.CreatedAt,
            EditedAt = korisnik.EditedAt
        };
    }

    public async Task<KorisnikDto?> GetKorisnikByUserNameAsync(string userName)
    {
        var korisnik = await _korisnikRepository.GetByUserNameAsync(userName);
        if (korisnik == null)
            return null;

        return new KorisnikDto
        {
            Id = korisnik.Id,
            ImePrezime = korisnik.ImePrezime,
            UserName = korisnik.UserName,
            SektorId = korisnik.SektorId,
            SektorNaziv = korisnik.Sektor?.Naziv ?? string.Empty,
            CreatedAt = korisnik.CreatedAt,
            EditedAt = korisnik.EditedAt
        };
    }

    public async Task<KorisnikDto> CreateKorisnikAsync(CreateKorisnikRequest request)
    {
        var korisnik = new Korisnik
        {
            Id = request.Id,
            ImePrezime = request.ImePrezime,
            UserName = request.UserName,
            SektorId = request.SektorId
        };

        var created = await _korisnikRepository.CreateAsync(korisnik);
        
        return new KorisnikDto
        {
            Id = created.Id,
            ImePrezime = created.ImePrezime,
            UserName = created.UserName,
            SektorId = created.SektorId,
            SektorNaziv = string.Empty,
            CreatedAt = created.CreatedAt,
            EditedAt = created.EditedAt
        };
    }

    public async Task<KorisnikDto?> UpdateKorisnikAsync(UpdateKorisnikRequest request)
    {
        var korisnik = new Korisnik
        {
            Id = request.Id,
            ImePrezime = request.ImePrezime,
            UserName = request.UserName,
            SektorId = request.SektorId
        };

        var updated = await _korisnikRepository.UpdateAsync(korisnik);
        if (updated == null)
            return null;

        return new KorisnikDto
        {
            Id = updated.Id,
            ImePrezime = updated.ImePrezime,
            UserName = updated.UserName,
            SektorId = updated.SektorId,
            SektorNaziv = string.Empty,
            CreatedAt = updated.CreatedAt,
            EditedAt = updated.EditedAt
        };
    }

    public async Task<bool> DeleteKorisnikAsync(string id)
    {
        return await _korisnikRepository.DeleteAsync(id);
    }

    public async Task<bool> KorisnikExistsAsync(string id)
    {
        return await _korisnikRepository.ExistsAsync(id);
    }
}
