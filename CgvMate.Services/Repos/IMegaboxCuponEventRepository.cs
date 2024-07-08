using CgvMate.Data.Entities.Megabox;

namespace CgvMate.Services.Repos;

public interface IMegaboxCuponEventRepository
{
    Task<CuponEvent> GetCuponEventAsync(string id);

    Task<IEnumerable<CuponEvent>> GetCuponEventsAsync();

    Task AddCuponEventsAsync(IEnumerable<CuponEvent> cuponEvents);
}
