using System;
using TicTacToe.Core;
using TicTacToe.Core.Enums;
using TicTacToe.Core.Exceptions;
using Xunit;

namespace TicTacToe.Tests.Unit;

public class GameTests
{
    [Fact]
    //Корректная установка символа
    public void MakeMove_ShouldPlaceCorrectSymbol_WhenNoSabotage()
    {
        // Arrange
        var game = new Game(3, 3);
        var playerId = game.PlayerX.Id;

        // Act
        game.MakeMove(playerId, 0, 0);

        // Assert
        Assert.Equal(CellState.X, game.Board.Grid[0, 0]);
        Assert.Equal(game.PlayerO.Id, game.CurrentPlayerId);
        Assert.Null(game.Winner);
    }

    [Fact]
    //корректная смена хода
    public void MakeMove_ShouldSwitchPlayers()
    {
        var game = new Game(3, 3);
        var p1 = game.PlayerX;
        var p2 = game.PlayerO;

        game.MakeMove(p1.Id, 0, 0);
        Assert.Equal(p2.Id, game.CurrentPlayerId);

        game.MakeMove(p2.Id, 1, 1);
        Assert.Equal(p1.Id, game.CurrentPlayerId);
    }

    [Fact]
    //ошибка при ходе не в свою очередь
    public void MakeMove_ShouldThrow_WhenNotPlayersTurn()
    {
        var game = new Game(3, 3);
        var wrongPlayerId = game.PlayerO.Id;

        var ex = Assert.Throws<InvalidMoveException>(() => game.MakeMove(wrongPlayerId, 0, 0));
        Assert.Equal("Not your turn.", ex.Message);
    }

    [Fact]
    //ошибка при ходе в занятую ячейку
    public void MakeMove_ShouldThrow_WhenCellNotEmpty()
    {
        var game = new Game(3, 3);
        var p1 = game.PlayerX;
        var p2 = game.PlayerO;

        game.MakeMove(p1.Id, 0, 0);
        game.MakeMove(p2.Id, 1, 1);

        var ex = Assert.Throws<InvalidMoveException>(() => game.MakeMove(p1.Id, 0, 0));
        Assert.Equal("Cell is not empty.", ex.Message);
    }

    [Fact]
    //победа игрока по горизонтали
    public void MakeMove_ShouldDetectWin()
    {
        var game = new Game(3, 3);
        var p1 = game.PlayerX;
        var p2 = game.PlayerO;

        game.MakeMove(p1.Id, 0, 0); // X
        game.MakeMove(p2.Id, 1, 0); // O
        game.MakeMove(p1.Id, 0, 1); // X
        game.MakeMove(p2.Id, 1, 1); // O
        game.MakeMove(p1.Id, 0, 2); // X — победа по строке

        Assert.Equal("X", game.Winner);
    }

    [Fact]
    //победа по вертикали
    public void MakeMove_ShouldDetectWin_Vertical()
    {
        var game = new Game(3, 3);
        var p1 = game.PlayerX;
        var p2 = game.PlayerO;

        game.MakeMove(p1.Id, 0, 0); // X
        game.MakeMove(p2.Id, 0, 1); // O
        game.MakeMove(p1.Id, 1, 0); // X
        game.MakeMove(p2.Id, 1, 1); // O
        game.MakeMove(p1.Id, 2, 0); // X - победа по вертикали

        Assert.Equal("X", game.Winner);
    }

    [Fact]
    //победа по диагонали
    public void MakeMove_ShouldDetectWin_Diagonal()
    {
        var game = new Game(3, 3);
        var p1 = game.PlayerX;
        var p2 = game.PlayerO;

        game.MakeMove(p1.Id, 0, 0); // X
        game.MakeMove(p2.Id, 0, 1); // O
        game.MakeMove(p1.Id, 1, 1); // X
        game.MakeMove(p2.Id, 1, 0); // O
        game.MakeMove(p1.Id, 2, 2); // X - победа по диагонали

        Assert.Equal("X", game.Winner);
    }

    [Fact]
    //проверка ничьи
    public void MakeMove_ShouldDeclareDraw_WhenNoWinner()
    {
        var game = new Game(3, 3);
        var p1 = game.PlayerX;
        var p2 = game.PlayerO;

        // Заполняем поле без победителя
        game.MakeMove(p1.Id, 0, 0); // X
        game.MakeMove(p2.Id, 0, 1); // O
        game.MakeMove(p1.Id, 0, 2); // X
        game.MakeMove(p2.Id, 1, 1); // O
        game.MakeMove(p1.Id, 1, 0); // X
        game.MakeMove(p2.Id, 1, 2); // O
        game.MakeMove(p1.Id, 2, 1); // X
        game.MakeMove(p2.Id, 2, 0); // O
        game.MakeMove(p1.Id, 2, 2); // X

        Assert.Null(game.Winner);
        Assert.True(game.IsDraw);
    }

    [Fact]
    //запрет хода после окончания игры
    public void MakeMove_ShouldThrow_WhenGameIsAlreadyFinished()
    {
        var game = new Game(3, 3);
        var p1 = game.PlayerX;
        var p2 = game.PlayerO;

        game.MakeMove(p1.Id, 0, 0);
        game.MakeMove(p2.Id, 1, 0);
        game.MakeMove(p1.Id, 0, 1);
        game.MakeMove(p2.Id, 1, 1);
        game.MakeMove(p1.Id, 0, 2); // X победил

        var ex = Assert.Throws<InvalidMoveException>(() => game.MakeMove(p2.Id, 2, 2));
        Assert.Equal("Game is already finished.", ex.Message);
    }
}
