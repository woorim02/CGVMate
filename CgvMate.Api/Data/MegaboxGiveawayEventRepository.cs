using CgvMate.Data.Entities.Megabox;
using CgvMate.Services.Repos;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api.Data;

public class MegaboxGiveawayEventRepository : IMegaboxGiveawayEventRepository
{
    public MegaboxGiveawayEventRepository(AppDbContext context)
    {
        _context = context;
    }

    private readonly AppDbContext _context;

    public async Task AddAsync(GiveawayEvent giveawayEvent)
    {
        await _context.MegaboxGiveawayEvents.AddAsync(giveawayEvent);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string eventIndex)
    {
        var eventToDelete = await _context.MegaboxGiveawayEvents.FindAsync(eventIndex);
        if (eventToDelete != null)
        {
            _context.MegaboxGiveawayEvents.Remove(eventToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(List<string> ids)
    {
        var eventsToDelete = await _context.MegaboxGiveawayEvents
                                           .Where(e => ids.Contains(e.ID))
                                           .ToListAsync();
        _context.MegaboxGiveawayEvents.RemoveRange(eventsToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<GiveawayEvent>> GetAllAsync()
    {
        return await _context.MegaboxGiveawayEvents.ToListAsync();
    }

    public async Task UpdateViewAsync(string giveawayID)
    {
        var eventToUpdate = await _context.MegaboxGiveawayEvents.FindAsync(giveawayID);
        if (eventToUpdate != null)
        {
            eventToUpdate.ViewCount++;
            _context.MegaboxGiveawayEvents.Update(eventToUpdate);
            await _context.SaveChangesAsync();
        }
    }
}
