using CgvMate.Api.Entities;

namespace CgvMate.Api.Data.Interfaces;

public interface IBoardRepo
{
    Task<IEnumerable<Board>> GetAllBoardsAsync();
    Task<Board?> GetBoardByIdAsync(int id);
    Task AddBoardAsync(Board board);
    Task UpdateBoardAsync(Board board);
    Task DeleteBoardAsync(int id);
}
