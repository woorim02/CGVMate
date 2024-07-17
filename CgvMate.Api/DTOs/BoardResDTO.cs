using CgvMate.Api.Entities;

namespace CgvMate.Api.DTOs;

public class BoardResDTO
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public static BoardResDTO FromEntity(Board board)
    {
        return new BoardResDTO
        {
            Id = board.Id,
            Title = board.Title,
            Description = board.Description,
        };
    }
}
