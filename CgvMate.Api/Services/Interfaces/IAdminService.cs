namespace CgvMate.Api.Services.Interfaces;

public interface IAdminService
{
    public Task<IEnumerable<string>> GetLotteGiveawayEventKeywords();

    public Task AddLotteGiveawayEventKeywords(string keyword);

    public Task DeleteLotteGiveawayEventKeywords(string keyword);
}
