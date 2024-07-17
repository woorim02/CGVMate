using CgvMate.Api.DTOs;
using CgvMate.Api.Entities;
using CgvMate.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CgvMate.Api.Controllers;

[Route("board")]
[ApiController]
public class BoardsController : ControllerBase
{
    private readonly IBoardService _boardService;

    public BoardsController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBoards()
    {
        var boards = await _boardService.GetAllBoardsAsync();
        return Ok(boards);
    }
}
