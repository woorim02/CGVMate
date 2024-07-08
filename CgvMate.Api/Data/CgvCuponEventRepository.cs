using CgvMate.Data.Entities.Cgv;
using CgvMate.Services.Repos;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api.Data;

public class CgvCuponEventRepository : ICgvCuponEventRepository
{
    private readonly AppDbContext _context;

    public CgvCuponEventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddCuponEventsAsync(IEnumerable<CuponEvent> cuponEvents)
    {
        await _context.CgvCuponEvents.AddRangeAsync(cuponEvents);
        await _context.SaveChangesAsync();
    }

    public async Task<CuponEvent?> GetCuponEventAsync(string id)
    {
        return await _context.CgvCuponEvents.FindAsync(id);
    }

    public async Task<IEnumerable<CuponEvent>> GetCuponEventsAsync()
    {
        return await _context.CgvCuponEvents.ToListAsync();
    }
}