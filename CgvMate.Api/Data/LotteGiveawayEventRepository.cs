using CgvMate.Data.Entities.LotteCinema;
using CgvMate.Services.Repos;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api.Data;

internal class LotteGiveawayEventRepository : ILotteGiveawayEventRepository
{
    private readonly AppDbContext _context;

    public LotteGiveawayEventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Event lotteEvent)
    {
        await _context.LotteEvents.AddAsync(lotteEvent);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(IEnumerable<Event> lotteEvent)
    {
        await _context.LotteEvents.AddRangeAsync(lotteEvent);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var giveawayEvent = await _context.CgvGiveawayEvents.FindAsync(id);
        if (giveawayEvent != null)
        {
            _context.CgvGiveawayEvents.Remove(giveawayEvent);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(IEnumerable<Event> lotteEvent)
    {
        _context.LotteEvents.RemoveRange(lotteEvent);
        await _context.SaveChangesAsync();
    }

    public Task DeleteAsync(string eventID)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.LotteEvents.ToListAsync();
    }

    public async Task<Event> GetByIdAsync(string eventID)
    {
        return await _context.LotteEvents.SingleOrDefaultAsync(e => e.EventID == eventID);
    }

    public async Task<Dictionary<string, int>> GetViewsAsync(IEnumerable<string> eventIDs)
    {
        var views = await _context.LotteEvents
                                  .Where(e => eventIDs.Contains(e.EventID))
                                  .ToDictionaryAsync(e => e.EventID, e => e.Views);
        return views;
    }

    public async Task UpdateAsync(Event lotteEvent)
    {
        _context.LotteEvents.Update(lotteEvent);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(IEnumerable<Event> lotteEvent)
    {
        _context.LotteEvents.UpdateRange(lotteEvent);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateViewAsync(string eventID)
    {
        var giveawayEvent = await _context.LotteEvents.SingleOrDefaultAsync(e => e.EventID == eventID);
        if (giveawayEvent != null)
        {
            giveawayEvent.Views++;
            _context.LotteEvents.Update(giveawayEvent);
            await _context.SaveChangesAsync();
        }
    }
}