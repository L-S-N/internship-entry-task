using Microsoft.AspNetCore.Mvc;
using TicTacToe.Api.Models;
using TicTacToe.Api.Services;
using TicTacToe.Core;
using TicTacToe.Core.Enums;
using TicTacToe.Core.Exceptions;

namespace TicTacToe.Api.Controllers;

[ApiController]
[Route("games")]
public class GameController : ControllerBase
{
    private readonly GameService _service;

    public GameController(GameService service)
    {
        _service = service;
    }

    // POST /games
    [HttpPost]
    public ActionResult CreateGame([FromBody] CreateGameRequest request)
    {
        if (request.BoardSize < 3 || request.WinCondition < 3)
            return BadRequest("Board size and win condition must be >= 3");

        var game = _service.CreateGame(request.BoardSize, request.WinCondition);

        return CreatedAtAction(nameof(GetGame), new { id = game.Id }, new
        {
            game.Id,
            PlayerX = game.PlayerX.Id,
            PlayerO = game.PlayerO.Id
        });
    }

    // GET /games/{id}
    [HttpGet("{id}")]
    public ActionResult GetGame(Guid id)
    {
        var game = _service.GetGame(id);
        if (game == null) return NotFound();

        var boardForJson = new CellState[game.Board.Size][];
        for (int i = 0; i < game.Board.Size; i++)
        {
            boardForJson[i] = new CellState[game.Board.Size];
            for (int j = 0; j < game.Board.Size; j++)
            {
                boardForJson[i][j] = game.Board.Grid[i, j];
            }
        }

        return Ok(new
        {
            game.Id,
            CurrentPlayerId = game.CurrentPlayerId,
            Winner = game.Winner,
            IsDraw = game.IsDraw,
            MoveCount = game.MoveCount,
            Board = game.Board.Grid
        });
    }

    // POST /games/{id}/moves
    [HttpPost("{id}/moves")]
    public ActionResult MakeMove(Guid id, [FromBody] MakeMoveRequest request)
    {
        var game = _service.GetGame(id);
        if (game == null) return NotFound();

        try
        {
            game.MakeMove(request.PlayerId, request.X, request.Y);

            // Преобразуем 2D-массив для JSON
            var boardForJson = new CellState[game.Board.Size][];
            for (int i = 0; i < game.Board.Size; i++)
            {
                boardForJson[i] = new CellState[game.Board.Size];
                for (int j = 0; j < game.Board.Size; j++)
                {
                    boardForJson[i][j] = game.Board.Grid[i, j];
                }
            }

            return Ok(new
            {
                Status = "OK",
                MoveCount = game.MoveCount,
                Winner = game.Winner,
                Board = boardForJson
            });
        }
        catch (InvalidMoveException e)
        {
            return BadRequest(new { /*...*/ });
        }
        catch (Exception e) 
        {
            Console.WriteLine(e.StackTrace);
            return StatusCode(500, new { Title = "Internal Server Error" });
        }
    }


}