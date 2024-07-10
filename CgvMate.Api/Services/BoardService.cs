using CgvMate.Api.Data.Interfaces;
using CgvMate.Api.Entities;
using CgvMate.Api.Services.Interfaces;

namespace CgvMate.Api.Services;

public class BoardService : IBoardService
{
    private readonly IBoardRepo _boardRepo;

    public BoardService(IBoardRepo boardRepo)
    {
        _boardRepo = boardRepo;
    }

    public Task<IEnumerable<Board>> GetAllBoardsAsync()
    {
        return _boardRepo.GetAllBoardsAsync();
    }

    public Task<Board?> GetBoardByIdAsync(int id)
    {
        return _boardRepo.GetBoardByIdAsync(id);
    }

    public Task AddBoardAsync(Board board)
    {
        return _boardRepo.AddBoardAsync(board);
    }

    public Task UpdateBoardAsync(Board board)
    {
        return _boardRepo.UpdateBoardAsync(board);
    }

    public Task DeleteBoardAsync(int id)
    {
        return _boardRepo.DeleteBoardAsync(id);
    }
}