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
        public Color Color { get; set; }
        public double Scale { get; set; } = 1;
        public double Rotation { get; set; }
        public int MaxAge { get; set; }
        public bool IsDisposed { get; private set; } = false;
        public ParticleEmitter AttachedEmitter { get; set; }

        public Func<double, double> VelocityFunction { get; set; }
        public Func<double, double> ScaleFunction { get; set; }
        public Func<double, double> RotationFunction { get; set; }
        public Func<double, Color> ColorFunction{ get; set; }

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
            if (ColorFunction != null) Color = ColorFunction(animationTime);

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
                var width = Sprite.Width * Scale;
                var height = Sprite.Height * Scale;
                var locationOffset = new Vector((int)(width / 2.0), (int)(height / 2.0));
                Sprite.SetColor(Color);
                Sprite.DrawEx(renderer, Location.ToPoint() - locationOffset - viewOffset, Rotation, null, Scale, SDL2.SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            }
        }

        private void Dispose()
        {
            IsDisposed = true;
        }
    }
}
