using CgvMate.Data.Entities.LotteCinema;
using CgvMate.Services.Repos;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api.Data;

public class LotteGiveawayEventModelRepository : ILotteGiveawayEventModelRepository
{
    private readonly AppDbContext _context;

    public LotteGiveawayEventModelRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(LotteGiveawayEventModel model)
    {
        _context.LotteGiveawayEventModels.Add(model);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<LotteGiveawayEventModel>> GetAllAsync()
    {
        return await _context.LotteGiveawayEventModels.ToListAsync();
    }

    public async Task<LotteGiveawayEventModel?> GetAsync(string EventID)
    {
        return await _context.LotteGiveawayEventModels.FindAsync(EventID);
    }
}
