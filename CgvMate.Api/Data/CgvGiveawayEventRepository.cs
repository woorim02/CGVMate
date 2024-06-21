using CgvMate.Data.Entities.Cgv;
using CgvMate.Services.Repos;
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
            await _context.CgvGiveawayEvents.AddAsync(cgvEvent);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<GiveawayEvent> cgvEvent)
        {
            await _context.CgvGiveawayEvents.AddRangeAsync(cgvEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string eventIndex)
        {
            var cgvEvent = await _context.CgvGiveawayEvents.FindAsync(eventIndex);
            if (cgvEvent != null)
            {
                _context.CgvGiveawayEvents.Remove(cgvEvent);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(IEnumerable<GiveawayEvent> cgvEvent)
        {
            _context.CgvGiveawayEvents.RemoveRange(cgvEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GiveawayEvent>> GetAllAsync()
        {
            return await _context.CgvGiveawayEvents.ToListAsync();
        }

        public async Task<GiveawayEvent?> GetByIdAsync(string eventIndex)
        {
            return await _context.CgvGiveawayEvents.FindAsync(eventIndex);
        }

        public async Task<Dictionary<string, int>> GetViewsAsync(IEnumerable<string> eventIndexs)
        {
            var views = await _context.CgvGiveawayEvents
                                      .Where(e => eventIndexs.Contains(e.EventIndex))
                                      .ToDictionaryAsync(e => e.EventIndex, e => e.Views);
            return views;
        }

        public async Task UpdateAsync(GiveawayEvent cgvEvent)
        {
            _context.CgvGiveawayEvents.Update(cgvEvent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<GiveawayEvent> cgvEvent)
        {
            _context.CgvGiveawayEvents.UpdateRange(cgvEvent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateViewAsync(string eventIndex)
        {
            var cgvEvent = await _context.CgvGiveawayEvents.SingleOrDefaultAsync(e => e.EventIndex == eventIndex);
            if (cgvEvent != null)
            {
                cgvEvent.Views++;
                _context.CgvGiveawayEvents.Update(cgvEvent);
                await _context.SaveChangesAsync();
            }
        }
    }
}
