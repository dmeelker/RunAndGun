using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Utilities
{
    public class TickTimer
    {
        public int Interval { get; set; }
        private uint lastTickTime;

        public TickTimer(int interval)
        {
            Interval = interval;
        }

        public bool Update(uint time)
        {
            if(time - lastTickTime > Interval)
            {
                lastTickTime = time;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            lastTickTime = 0;
        }
    }
}
