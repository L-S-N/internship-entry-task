using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Core.Enums;
using TicTacToe.Core.Exceptions;

namespace TicTacToe.Core
{
    public class Game
    {
        public Guid Id { get; } = Guid.NewGuid();  // Уникальный идентификатор игры
        public Board Board { get; private set; }
        public Player PlayerX { get; private set; } = new Player();    //Игрок Х
        public Player PlayerO { get; private set; } = new Player();   //Игрок О
        public Guid CurrentPlayerId { get; private set; }  //Текущий игрок (чей ход)
        public int MoveCount { get; private set; } = 0;    //Кол-во ходов
        public List<Move> Moves { get; set; } = new();    //список совершённых ходов
        public string? Winner { get; private set; } = null; //Кто победил? Х или О
        public bool IsDraw => Winner == null && MoveCount >= Board.Size * Board.Size; //Условие ничьи
        public int WinCondition { get; private set; } //Сколько символов подряд нужно для победы (т.к. поле может быть больше, чем 3 на 3)
        private readonly Random random = new();

        private Game() { }

        public Game(int boardSize, int winCondition)
        {
            Id = Guid.NewGuid();
            this.Board = new Board(boardSize);
            this.WinCondition = winCondition;
            this.PlayerX = new Player { Symbol = "X" };
            this.PlayerO = new Player { Symbol = "O" };
            this.CurrentPlayerId = PlayerX.Id;   //Пусть игрок Х всегда ходит первым
        }

        //Метод-контроллер хода
        public void MakeMove(Guid playerId, int x, int y)
        {
            if (Winner != null)   //проверка есть ли уже победитель
                throw new InvalidMoveException("Game is already finished.");

            if (playerId != CurrentPlayerId)  //проверка ходит ли тот игрок
                throw new InvalidMoveException("Not your turn.");

            if (!Board.IsCellEmpty(x, y))  //проверка занята ли уже ячейка
                throw new InvalidMoveException("Cell is not empty.");

            //каждый третий ход — 10% шанс ошибки
            bool sabotage = (MoveCount + 1) % 3 == 0 && random.NextDouble() < 0.1;
            var player = GetPlayerById(playerId);
            var symbol = sabotage ? GetOpponentSymbol(player.Symbol) : player.Symbol; //определяем какой символ ставить

            var cellState = symbol == "X" ? CellState.X : CellState.O; //преобразуем символ в наш enum класс
            Board.SetCell(x, y, cellState);

            MoveCount++;

            //проверка условия победы
            if (CheckWin(cellState, x, y))
            {
                Winner = symbol;
            }
            else if (IsDraw)
            {
                Winner = null;
            }

            // Переход хода
            CurrentPlayerId = playerId == PlayerX.Id ? PlayerO.Id : PlayerX.Id;
        }

        private bool CheckWin(CellState symbol, int x, int y)
        {
            int[][] directions = new[]
            {
                new[] {0, 1},  // горизонталь -
                new[] {1, 0},  // вертикаль |
                new[] {1, 1},  // диагональ \
                new[] {1, -1}, // диагональ /
            };

            foreach (var dir in directions)
            {
                int count = 1;

                count += CountDirection(symbol, x, y, dir[0], dir[1]);
                count += CountDirection(symbol, x, y, -dir[0], -dir[1]);

                if (count >= WinCondition)
                    return true;
            }

            return false;
        }

        //фун-ция, которая считает подряд идущие символы в указанном направлении
        private int CountDirection(CellState symbol, int x, int y, int dx, int dy)
        {
            int count = 0;
            int nx = x + dx;
            int ny = y + dy;

            while (nx >= 0 && ny >= 0 && nx < Board.Size && ny < Board.Size && Board.Grid[nx, ny] == symbol)
            {
                count++;
                nx += dx;
                ny += dy;
            }

            return count;
        }

        //фун-ция, которая определяет кто ходит
        private Player GetPlayerById(Guid id)
        {
            if (id == PlayerX.Id) return PlayerX;
            if (id == PlayerO.Id) return PlayerO;
            throw new InvalidMoveException("Unknown player.");
        }

        //возвращает символ оппонента
        private string GetOpponentSymbol(string symbol) => symbol == "X" ? "O" : "X";
    }
}
