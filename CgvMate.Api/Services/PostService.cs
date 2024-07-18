using CgvMate.Api.Data;
using CgvMate.Api.DTOs;
using CgvMate.Api.Entities;
using CgvMate.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CgvMate.Api.Services;

public class PostService : IPostService
{
    private readonly AppDbContext _context;

    public PostService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PostResDto> GetPostAsync(string boardId, int postNo)
    {
        var post = await _context.Posts
            .Include(p => p.Comments)
            .Where(p => p.BoardId == boardId && p.No == postNo)
            .FirstOrDefaultAsync();
        if (post == null)
        {
            throw new Exception("게시글을 찾을 수 없습니다.");
        }
        post.ViewCount++;
        _context.Update(post);
        await _context.SaveChangesAsync();
        return PostResDto.FromEntity(post);
    }

    public async Task<Tuple<string, int>> AddPostAsync(PostAddReqDto dto)
    {
        bool? isBannedIP = await _context.BannedIPs.Select(p => p.IP == dto.WriterIP).FirstOrDefaultAsync();
        if(isBannedIP != null)
        {
            throw new Exception("차단된 사용자입니다.");
        }

        var post = dto.ToEntity();
        if (dto.WriterName.IsNullOrEmpty() || dto.WriterPassword.IsNullOrEmpty()){
            throw new Exception("작성자명과 비밀번호를 모두 입력해 주세요");
        }
        post.WriterName.Replace(" ", "");
        if (dto.WriterPassword == Environment.GetEnvironmentVariable("ADMIN_PASSWORD"))
        {
            post.WriterName = "관리자";
            post.WriterIP = "0.0.0.0";
        }
        else
        {
            if (post.WriterName == "관리자")
                throw new Exception($"'관리자'는 사용할 수 없는 닉네임입니다.");
        }
        var res = await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return Tuple.Create(res.Entity.BoardId, res.Entity.No);
    }

    public async Task<Tuple<string, int>> UpdatePostAsync(PostAddReqDto dto, int postId)
    {
        var post = await _context.Posts
            .Where(p => p.Id == postId)
            .FirstOrDefaultAsync();
        if (post == null)
        {
            throw new Exception("존재하지 않는 게시글입니다.");
        }
        if (!PasswordHasher.VerifyPassword(dto.WriterPassword, post.WriterPasswordHash))
        {
            throw new Exception("비밀번호가 잘못되었습니다.");
        }
        post.Title = dto.Title;
        post.Content = dto.ToEntity().Content;
        var res = _context.Posts
                          .Update(post);
        await _context.SaveChangesAsync();
        return Tuple.Create(res.Entity.BoardId, res.Entity.No);
    }

    public async Task DeletePostAsync(int postId, string password)
    {
        var post = await _context.Posts
            .Where(p => p.Id == postId)
            .FirstOrDefaultAsync();
        if (post == null)
        {
            throw new Exception("존재하지 않는 게시글입니다.");
        }
        if(!PasswordHasher.VerifyPassword(password, post.WriterPasswordHash))
        {
            throw new Exception("비밀번호가 잘못되었습니다.");
        }
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PostSummary>> GetPostSummarysAsync(string boardId, int pageNo = 1, int pageSize = 10)
    {
        return await _context.Set<Post>()
            .Where(p => p.BoardId == boardId)
            .OrderByDescending(p => p.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.Board)
            .Include(p => p.User)
            .Include(p => p.Comments)
            .Select(p => new PostSummary
            {
                Id = p.Id,
                No = p.No,
                BoardId = p.BoardId,
                Board = p.Board,
                WriterIP = GetFirstTwoParts(p.WriterIP),
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

    private static string GetFirstTwoParts(string ipAddress)
    {
        var parts = ipAddress.Split('.');
        if (parts.Length >= 2)
        {
            return $"{parts[0]}.{parts[1]}";
        }
        return string.Empty;
    }
}
