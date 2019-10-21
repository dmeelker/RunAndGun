using Game.Sprites;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Particle
{
    public class Particle
    {
        public const double tickMultiplier = 0.03;

        private readonly uint creationTime;
        public Vector Location { get; set; }
        public Vector Velocity { get; set; }
        public Sprite Sprite { get; set; }
        public double Scale { get; set; } = 1;
        public double Rotation { get; set; }
        public int MaxAge { get; set; }
        public bool IsDisposed { get; private set; } = false;
        public ParticleEmitter AttachedEmitter { get; set; }

        public Func<double, double> VelocityFunction { get; set; }
        public Func<double, double> ScaleFunction { get; set; }
        public Func<double, double> RotationFunction { get; set; }

        public Particle(uint creationTime)
        {
            this.creationTime = creationTime;
        }

        public void Update(FrameTime time)
        {
            var age = (int) (time.Time - creationTime);
            if (age >= MaxAge)
            {
                Dispose();
                return;
            }

            Location += Velocity * (time.TicksPassed * tickMultiplier);

            var animationTime = age / (double)MaxAge;

            if (ScaleFunction != null) Scale = ScaleFunction(animationTime);
            if (VelocityFunction != null) Velocity = Velocity.ToUnit() * VelocityFunction(animationTime);
            if (RotationFunction != null) Rotation = RotationFunction(animationTime);

            if (AttachedEmitter != null)
            {
                AttachedEmitter.Location = Location;
                AttachedEmitter.Velocity = Velocity;
            }
        }

        public void Render(IntPtr renderer, Point viewOffset)
        {
            if (Sprite != null)
            {
                Sprite.MakeRed();
                Sprite.DrawEx(renderer, Location.ToPoint() - viewOffset, Rotation, null, Scale, SDL2.SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            }
        }

        private void Dispose()
        {
            IsDisposed = true;
        }
    }
}
