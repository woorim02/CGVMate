using CgvMate.Api.Entities;

namespace CgvMate.Api.Services.Interfaces;

public interface IBoardService
{
    Task<IEnumerable<Board>> GetAllBoardsAsync();
    Task<Board?> GetBoardByIdAsync(int id);
    Task AddBoardAsync(Board board);
    Task UpdateBoardAsync(Board board);
    Task DeleteBoardAsync(int id);
}
