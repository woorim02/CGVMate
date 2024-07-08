using CgvMate.Data.Entities.Cgv;

namespace CgvMate.Services.Repos;

public interface ICgvCuponEventRepository
{
    Task<CuponEvent> GetCuponEventAsync(string id);

    Task<IEnumerable<CuponEvent>> GetCuponEventsAsync();

    Task AddCuponEventsAsync(IEnumerable<CuponEvent> cuponEvents);
}
