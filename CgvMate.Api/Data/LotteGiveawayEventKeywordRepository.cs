using CgvMate.Data.Entities.LotteCinema;
using CgvMate.Services.Repos;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api.Data;

public class LotteGiveawayEventKeywordRepository : ILotteGiveawayEventKeywordRepository
{
    private readonly AppDbContext _context;

    public LotteGiveawayEventKeywordRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddKeywordAsync(string keyword)
    {
        var obj = new GiveawayEventKeyword { Keyword = keyword };
        await _context.LotteGiveawayEventKeywords.AddAsync(obj);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteKeywordAsync(string keyword)
    {
        var obj = await _context.LotteGiveawayEventKeywords.FirstOrDefaultAsync(x => x.Keyword == keyword);
        if (obj == null)
        {
            return;
        }
        _context.LotteGiveawayEventKeywords.Remove(obj);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exists(string keyword)
    {
        return await _context.LotteGiveawayEventKeywords.AnyAsync(k => k.Keyword == keyword);
    }

    public async Task<IEnumerable<string>> GetAllKeywordsAsync()
    {
        return await _context.LotteGiveawayEventKeywords.Select(x => x.Keyword).ToListAsync();
    }
}
