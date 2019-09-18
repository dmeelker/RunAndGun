using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public abstract class Entity
    {
        public virtual void Update(int ticksPassed)
        { }

        public virtual void Render(IntPtr rendererId)
        { }
    }
}
