﻿using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities
{
    public abstract class Entity
    {
        public Vector Location;
        public Vector OldLocation;
        public Vector Size;
        public Rect GetBoundingBox() => new Rect(Location.X, Location.Y, Size.X, Size.Y);
        public Vector CenterLocation => Location + (Size / 2);
        public Point HalfSize => (Size * 0.5).ToPoint();

        public bool Disposable = false;
        public bool Disposed = false;

        public virtual void Update(uint time, int ticksPassed)
        { }

        public virtual void Render(IntPtr rendererId, Point viewOffset)
        { }

        public virtual void OnDisposed()
        { }

        public void Dispose()
        {
            Disposable = true;
        }

        public int DistanceTo(Entity otherEntity)
        {
            return (int) Math.Abs((Location - otherEntity.Location).Length);
        }
    }
}
