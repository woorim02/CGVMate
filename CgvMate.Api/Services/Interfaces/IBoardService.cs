using CgvMate.Api.Entities;

namespace CgvMate.Api.Services.Interfaces;

public interface IBoardService
{
    Task<IEnumerable<Board>> GetAllBoardsAsync();
}
