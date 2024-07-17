using CgvMate.Api.Data;
using CgvMate.Api.Entities;
using CgvMate.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api.Services;

public class BoardService : IBoardService
{
    public BoardService(AppDbContext context)
    {
        _context = context;
    }
    AppDbContext _context;

    public async Task<IEnumerable<Board>> GetAllBoardsAsync()
    {
        return await _context.Boards.ToListAsync();
    }
}