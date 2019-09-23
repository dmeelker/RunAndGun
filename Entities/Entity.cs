using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public abstract class Entity
    {
        public Vector Location;
        public Vector Size;
        public Rect GetBoundingBox() => new Rect(Location.X, Location.Y, Size.X, Size.Y);

        public bool Disposable = false;
        public bool Disposed = false;

        public virtual void Update(int ticksPassed)
        { }

        public virtual void Render(IntPtr rendererId)
        { }

        public void Dispose()
        {
            Disposable = true;
        }
    }
}
