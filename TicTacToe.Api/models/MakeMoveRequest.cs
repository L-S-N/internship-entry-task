namespace TicTacToe.Api.Models;

public class MakeMoveRequest
{
    public Guid PlayerId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}