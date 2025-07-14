using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Core
{
    public class Player
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Symbol { get; set; } = string.Empty;
    }
}
