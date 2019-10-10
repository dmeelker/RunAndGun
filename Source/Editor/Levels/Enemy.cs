using SharedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Levels
{
    public class Enemy
    {
        public int X { get; set; }
        public int Y { get; set; }
        public EnemyType Type { get; set; }
        public Direction Direction { get; set; }
    }
}
