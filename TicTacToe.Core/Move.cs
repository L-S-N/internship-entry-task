using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Core
{
    public class Move
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
