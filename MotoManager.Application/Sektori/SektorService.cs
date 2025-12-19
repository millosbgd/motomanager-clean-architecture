using MotoManager.Application.Abstractions;
using MotoManager.Domain.Entities;

namespace MotoManager.Application.Sektori;

public class SektorService
{
    private readonly ISektorRepository _sektorRepository;

    public SektorService(ISektorRepository sektorRepository)
    {
        _sektorRepository = sektorRepository;
    }

    public async Task<IEnumerable<SektorDto>> GetAllSektoriAsync()
    {
        var sektori = await _sektorRepository.GetAllAsync();
        return sektori.Select(s => new SektorDto
        {
            Id = s.Id,
            Naziv = s.Naziv,
            CreatedAt = s.CreatedAt,
            EditedAt = s.EditedAt
        });
    }

    public async Task<SektorDto?> GetSektorByIdAsync(int id)
    {
        var sektor = await _sektorRepository.GetByIdAsync(id);
        if (sektor == null)
            return null;

        return new SektorDto
        {
            Id = sektor.Id,
            Naziv = sektor.Naziv,
            CreatedAt = sektor.CreatedAt,
            EditedAt = sektor.EditedAt
        };
    }

    public async Task<SektorDto> CreateSektorAsync(CreateSektorRequest request)
    {
        var sektor = new Sektor
        {
            Naziv = request.Naziv
        };

        var created = await _sektorRepository.CreateAsync(sektor);
        
        return new SektorDto
        {
            Id = created.Id,
            Naziv = created.Naziv,
            CreatedAt = created.CreatedAt,
            EditedAt = created.EditedAt
        };
    }

    public async Task<SektorDto?> UpdateSektorAsync(UpdateSektorRequest request)
    {
        var sektor = new Sektor
        {
            Id = request.Id,
            Naziv = request.Naziv
        };

        var updated = await _sektorRepository.UpdateAsync(sektor);
        if (updated == null)
            return null;

        return new SektorDto
        {
            Id = updated.Id,
            Naziv = updated.Naziv,
            CreatedAt = updated.CreatedAt,
            EditedAt = updated.EditedAt
        };
    }

    public async Task<bool> DeleteSektorAsync(int id)
    {
        return await _sektorRepository.DeleteAsync(id);
    }
}
