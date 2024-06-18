using CgvMate.Data.Entities.Cgv;
using CgvMate.Services;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api.Data
{
    internal class CgvGiveawayEventRepository : ICgvGiveawayEventRepository
    {
        private readonly AppDbContext _context;

        public CgvGiveawayEventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(GiveawayEvent cgvEvent)
        {
            await _context.GiveawayEvents.AddAsync(cgvEvent);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<GiveawayEvent> cgvEvent)
        {
            await _context.GiveawayEvents.AddRangeAsync(cgvEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string eventIndex)
        {
            var cgvEvent = await _context.GiveawayEvents.FindAsync(eventIndex);
            if (cgvEvent != null)
            {
                _context.GiveawayEvents.Remove(cgvEvent);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(IEnumerable<GiveawayEvent> cgvEvent)
        {
            _context.GiveawayEvents.RemoveRange(cgvEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GiveawayEvent>> GetAllAsync()
        {
            return await _context.GiveawayEvents.ToListAsync();
        }

        public async Task<GiveawayEvent?> GetByIdAsync(string eventIndex)
        {
            return await _context.GiveawayEvents.FindAsync(eventIndex);
        }

        public async Task<Dictionary<string, int>> GetViewsAsync(IEnumerable<string> eventIndexs)
        {
            var views = await _context.GiveawayEvents
                                      .Where(e => eventIndexs.Contains(e.EventIndex))
                                      .ToDictionaryAsync(e => e.EventIndex, e => e.Views);
            return views;
        }

        public async Task UpdateAsync(GiveawayEvent cgvEvent)
        {
            _context.GiveawayEvents.Update(cgvEvent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<GiveawayEvent> cgvEvent)
        {
            _context.GiveawayEvents.UpdateRange(cgvEvent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateViewAsync(string eventIndex)
        {
            var cgvEvent = await _context.GiveawayEvents.SingleOrDefaultAsync(e => e.EventIndex == eventIndex);
            if (cgvEvent != null)
            {
                cgvEvent.Views++;
                _context.GiveawayEvents.Update(cgvEvent);
                await _context.SaveChangesAsync();
            }
        }
    }
}
