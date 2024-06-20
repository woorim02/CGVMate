using CgvMate.Data.Entities.Megabox;

namespace CgvMate.Services;

public interface IMegaboxGiveawayEventRepository
{
    Task<IEnumerable<GiveawayEvent>> GetAllAsync();
    Task AddAsync(GiveawayEvent giveawayEvent);
    Task UpdateViewAsync(string giveawayID);
    Task DeleteAsync(string eventIndex);
    Task DeleteAsync(List<string> ids);
}
