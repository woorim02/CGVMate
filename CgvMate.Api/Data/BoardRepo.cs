using CgvMate.Api.Data.Interfaces;
using CgvMate.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api.Data;

public class BoardRepo : IBoardRepo
{
    private readonly AppDbContext _context;

    public BoardRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Board>> GetAllBoardsAsync()
    {
        return await _context.Boards.ToListAsync();
    }

    public async Task<Board?> GetBoardByIdAsync(int id)
    {
        return await _context.Boards.FindAsync(id);
    }

    public async Task AddBoardAsync(Board board)
    {
        await _context.Boards.AddAsync(board);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBoardAsync(Board board)
    {
        _context.Boards.Update(board);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBoardAsync(int id)
    {
        var board = await _context.Boards.FindAsync(id);
        if (board != null)
        {
            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();
        }
    }
}
