using CgvMate.Api.Data.Interfaces;
using CgvMate.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api.Data;

public class CommentRepo : ICommentRepo
{
    private readonly AppDbContext _context;

    public CommentRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
    {
        return await _context.Comments
                             .Include(c => c.User)
                             .Include(c => c.Children)
                             .ToListAsync();
    }

    public async Task<Comment?> GetCommentByIdAsync(int id)
    {
        return await _context.Comments
                             .Include(c => c.Post)
                             .Include(c => c.User)
                             .Include(c => c.Children)
                             .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        return await _context.Comments
                             .Where(c => c.PostId == postId && !c.HasParent)
                             .Include(c => c.User)
                             .Include(c => c.Children)
                             .ToListAsync();
    }

    public async Task AddCommentAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
