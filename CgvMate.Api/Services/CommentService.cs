using CgvMate.Api.Data;
using CgvMate.Api.DTOs;
using CgvMate.Api.Entities;
using CgvMate.Api.Services.Interfaces;

namespace CgvMate.Api.Services;

public class CommentService : ICommentService
{
    public CommentService(AppDbContext context)
    {
        _context = context;
    }
    AppDbContext _context;

    public async Task AddCommentAsync(CommentAddReqDto dto)
    {
        var comment = dto.ToEntity();
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int id, string password)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            throw new Exception("존재하지 않는 댓글입니다.");
        }
        if(!PasswordHasher.VerifyPassword(password, comment.WriterPasswordHash))
        {
            throw new Exception("잘못된 비밀번호입니다.");
        }
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
}
