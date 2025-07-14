using TicTacToe.Core;

namespace TicTacToe.Tests;

public class UnitTest1
{
    [Fact]
    //простенький тест на условие победы игрока ’
    public void PlayerX_Wins_Horizontally()
    {
        var game = new Game(3, 3);
        var x = game.PlayerX.Id;
        var o = game.PlayerO.Id;

        game.MakeMove(x, 0, 0);
        game.MakeMove(o, 1, 0);
        game.MakeMove(x, 0, 1);
        game.MakeMove(o, 1, 1);
        game.MakeMove(x, 0, 2);

        Assert.Equal("X", game.Winner);
    }
}