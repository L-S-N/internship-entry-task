namespace TicTacToe.Api.Models;

public class CreateGameRequest
{
    public int BoardSize { get; set; }
    public int WinCondition { get; set; }
}