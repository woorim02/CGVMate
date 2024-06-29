using CgvMate.Data.Entities.LotteCinema;

namespace CgvMate.Services.Repos;

public interface ILotteGiveawayEventModelRepository
{
    Task<IEnumerable<LotteGiveawayEventModel>> GetAllAsync();
    Task<LotteGiveawayEventModel?> GetAsync(string EventID);
    Task AddAsync(LotteGiveawayEventModel model);
}
