using CgvMate.Data.Entities.Megabox;
using CgvMate.Services.Repos;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api.Data
{
    public class MegaboxCuponEventRepository : IMegaboxCuponEventRepository
    {
        private readonly AppDbContext _context;

        public MegaboxCuponEventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCuponEventsAsync(IEnumerable<CuponEvent> cuponEvents)
        {
            await _context.MegaboxCuponEvents.AddRangeAsync(cuponEvents);
            await _context.SaveChangesAsync();
        }

        public async Task<CuponEvent> GetCuponEventAsync(string id)
        {
            return await _context.MegaboxCuponEvents.FindAsync(id);
        }

        public async Task<IEnumerable<CuponEvent>> GetCuponEventsAsync()
        {
            return await _context.MegaboxCuponEvents.ToListAsync();
        }
    }
}
