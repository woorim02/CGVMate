using CgvMate.Data.Entities.LotteCinema;

namespace CgvMate.Services.Repos;

public interface ILotteGiveawayEventRepository
{
    Task<IEnumerable<Event>> GetAllAsync();
    Task<Event> GetByIdAsync(string eventID);
    Task<Dictionary<string, int>> GetViewsAsync(IEnumerable<string> eventIDs);
    Task AddAsync(Event lotteEvent);
    Task AddAsync(IEnumerable<Event> lotteEvent);
    Task UpdateAsync(Event lotteEvent);
    Task UpdateAsync(IEnumerable<Event> lotteEvent);
    Task UpdateViewAsync(string eventID);
    Task DeleteAsync(string eventID);
    Task DeleteAsync(IEnumerable<Event> lotteEvent);
}
