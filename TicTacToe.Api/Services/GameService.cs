using TicTacToe.Core;
using Microsoft.EntityFrameworkCore;
namespace TicTacToe.Api.Services;

public class GameService
{
    private readonly Dictionary<Guid, Game> _games = new();

    public Game CreateGame(int size, int win)
    {
        var game = new Game(size, win);
        _games.Add(game.Id, game);
        return game;
    }

    public Game? GetGame(Guid id) =>
        _games.TryGetValue(id, out var game) ? game : null;
}