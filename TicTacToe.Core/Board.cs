using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Core.Enums;

namespace TicTacToe.Core
{
    public class Board
    {
        public int Size { get; }
        public CellState[,] Grid { get; }

        public Board(int size)
        {
            Size = size;
            Grid = new CellState[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    Grid[i, j] = CellState.Empty;
        }

        public bool IsCellEmpty(int x, int y) => Grid[x, y] == CellState.Empty;

        public void SetCell(int x, int y, CellState state) => Grid[x, y] = state;
    }
    
}
