using CgvMate.Api.Services.Repos;
using CgvMate.Services.Repos;

namespace CgvMate.Api.Services;

public class AdminService : IAdminService
{
    private readonly ILotteGiveawayEventKeywordRepository _lotteGiveawayEventKeywordRepo;

    public AdminService(ILotteGiveawayEventKeywordRepository lotteGiveawayEventKeywordRepo)
    {
        _lotteGiveawayEventKeywordRepo = lotteGiveawayEventKeywordRepo;
    }

    public async Task AddLotteGiveawayEventKeywords(string keyword)
    {
        await _lotteGiveawayEventKeywordRepo.AddKeywordAsync(keyword);
    }

    public async Task DeleteLotteGiveawayEventKeywords(string keyword)
    {
        await _lotteGiveawayEventKeywordRepo.DeleteKeywordAsync(keyword);
    }

    public Task<IEnumerable<string>> GetLotteGiveawayEventKeywords()
    {
        return _lotteGiveawayEventKeywordRepo.GetAllKeywordsAsync();
    }
}
