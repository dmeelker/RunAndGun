using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Particle
{
    public abstract class ParticleEmitter
    {
        public Vector Location { get; set; }
        public Vector Velocity { get; set; }
        public int ParticlesPerSecond { get; set; }
        public bool IsDisposed { get; private set; } = false;

        public abstract void Update(FrameTime time);
    }
}
