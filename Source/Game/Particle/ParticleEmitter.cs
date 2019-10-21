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
        public int ParticleInterval { get; set; }
        public bool IsDisposed { get; private set; } = false;

        public abstract void Update(FrameTime time);
    }

    public class SprayEmitter : ParticleEmitter
    {
        public Vector Vector { get; set; }
        public int SpreadInDegrees { get; set; }
        private uint lastParticleTime;


        public override void Update(FrameTime time)
        {
            if(time.Time - lastParticleTime >= ParticleInterval)
            {
                lastParticleTime = time.Time;
                var particle = GenerateParticle(time.Time);
                Services.Game.Particles.AddParticle(particle);
            }
        }

        private Particle GenerateParticle(uint time)
        {
            var halfSpread = SpreadInDegrees / 2;
            var angle = Vector.AngleInDegrees + (Services.Random.Next(SpreadInDegrees) - halfSpread);
            var vector = Vector.FromAngleInDegrees(angle) * 10;

            // https://gist.github.com/gre/1650294
            return new Particle(time) {
                Velocity = vector,
                Location = Location,
                MaxAge = 1000,
                Sprite = Services.Sprites["round-particle"],
                ScaleFunction = EasingFunctions.ScalarAnimation(1, 5, EasingFunctions.EaseInQuad),
                VelocityFunction = EasingFunctions.ScalarAnimation(10, 3, EasingFunctions.EaseInQuad),
                RotationFunction = EasingFunctions.ScalarAnimation(0, 360, EasingFunctions.Linear)
            };
        }
    }
}
