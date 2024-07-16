using CgvMate.Api.Data.Interfaces;
using CgvMate.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CgvMate.Api.Data;

public class PostRepo : IPostRepo
{
    private readonly AppDbContext _context;

    public PostRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        return await _context.Posts
                             .Include(p => p.Board)
                             .Include(p => p.User)
                             .Include(p => p.Comments)
                             .ToListAsync();
    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await _context.Posts
                             .Include(p => p.Board)
                             .Include(p => p.User)
                             .Include(p => p.Comments)
                             .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Post>> GetPostsByBoardIdAsync(int boardId, int pageNo, int pageSize)
    {
        return await _context.Posts
                             .Where(p => p.BoardId == boardId)
                             .Skip((pageNo - 1) * pageSize)
                             .Take(pageSize)
                             .Include(p => p.Board)
                             .Include(p => p.User)
                             .Include(p => p.Comments)
                             .ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId, int pageNo, int pageSize)
    {
        return await _context.Posts
                             .Where(p => p.UserId == userId)
                             .Skip((pageNo - 1) * pageSize)
                             .Take(pageSize)
                             .Include(p => p.Board)
                             .Include(p => p.User)
                             .Include(p => p.Comments)
                             .ToListAsync();
    }

    public async Task AddPostAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePostAsync(Post post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePostAsync(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<PostSummary>> GetPostSummarysByBoardIdAsync(int boardId, int pageNo, int pageSize)
    {
        return await _context.Set<Post>()
            .Where(p => p.BoardId == boardId)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.Board)
            .Include(p => p.User)
            .Include(p => p.Comments)
            .Select(p => new PostSummary
            {
                Id = p.Id,
                BoardId = p.BoardId,
                Board = p.Board,
                WriterIP = p.WriterIP,
                WriterName = p.WriterName,
                UserId = p.UserId,
                User = p.User,
                Title = p.Title,
                DateCreated = p.DateCreated,
                ViewCount = p.ViewCount,
                CommentCount = p.Comments.Count,
                Upvote = p.Upvote,
                Downvote = p.Downvote
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PostSummary>> GetPostSummarysByUserIdAsync(int userId, int pageNo, int pageSize)
    {
        return await _context.Set<Post>()
            .Where(p => p.UserId == userId)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.Board)
            .Include(p => p.User)
            .Include(p => p.Comments)
            .Select(p => new PostSummary
            {
                Id = p.Id,
                BoardId = p.BoardId,
                Board = p.Board,
                WriterIP = p.WriterIP,
                WriterName = p.WriterName,
                UserId = p.UserId,
                User = p.User,
                Title = p.Title,
                DateCreated = p.DateCreated,
                ViewCount = p.ViewCount,
                CommentCount = p.Comments.Count,
                Upvote = p.Upvote,
                Downvote = p.Downvote
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PostSummary>> GetPostSummariesByBoardIdAndUserId(int boardId, int userId, int pageNo, int pageSize)
    {
        Expression<Func<Post, bool>> func;
        if (boardId == -1 && userId != -1)
        {
            func = p => p.UserId == userId;
        }
        else if (boardId != -1 && userId == -1)
        {
            func = p => p.BoardId == userId;
        }
        else
        {
            func = p => p.UserId == userId && p.BoardId == boardId;
        }

        return await _context.Set<Post>()
            .Where(func)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.Board)
            .Include(p => p.User)
            .Include(p => p.Comments)
            .Select(p => new PostSummary
            {
                Id = p.Id,
                BoardId = p.BoardId,
                Board = p.Board,
                WriterIP = p.WriterIP,
                WriterName = p.WriterName,
                UserId = p.UserId,
                User = p.User,
                Title = p.Title,
                DateCreated = p.DateCreated,
                ViewCount = p.ViewCount,
                CommentCount = p.Comments.Count,
                Upvote = p.Upvote,
                Downvote = p.Downvote
            })
            .ToListAsync();
    }
}
