using CgvMate.Data.Entities.Cgv;

namespace CgvMate.Services;

public interface ICgvGiveawayEventRepository
{
    Task<IEnumerable<GiveawayEvent>> GetAllAsync();
    Task<GiveawayEvent?> GetByIdAsync(string eventIndex);
    Task<Dictionary<string, int>> GetViewsAsync(IEnumerable<string> eventIndexs);
    Task AddAsync(GiveawayEvent cgvEvent);
    Task AddAsync(IEnumerable<GiveawayEvent> cgvEvent);
    Task UpdateAsync(GiveawayEvent cgvEvent);
    Task UpdateAsync(IEnumerable<GiveawayEvent> cgvEvent);
    Task UpdateViewAsync(string eventIndex);
    Task DeleteAsync(string eventIndex);
    Task DeleteAsync(IEnumerable<GiveawayEvent> cgvEvent);
}
