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
        public bool IsDisposed { get; private set; } = false;
        public Func<uint, Particle> ParticleFactory { get; set; }
        protected uint creationTime;

        public ParticleEmitter(uint creationTime)
        {
            this.creationTime = creationTime;
        }

        public abstract void Update(FrameTime time);

        protected void Dispose()
        {
            IsDisposed = true;
        }
    }

    public abstract class ConstantEmitter : ParticleEmitter
    {
        private uint lastParticleTime;
        public int ParticleInterval { get; set; }

        public ConstantEmitter(uint creationTime) : base(creationTime)
        {
            lastParticleTime = creationTime;
        }

        public override void Update(FrameTime time)
        {
            if (time.Time - lastParticleTime >= ParticleInterval)
            {
                lastParticleTime = time.Time;
                var particle = GenerateParticle(time.Time);
                Services.Game.Particles.AddParticle(particle);
            }
        }

        protected abstract Particle GenerateParticle(uint time);
    }

    public class SprayEmitter : ConstantEmitter
    {
        public Vector Vector { get; set; }
        public int SpreadInDegrees { get; set; }

        public SprayEmitter(uint creationTime) : base(creationTime)
        { }

        protected override Particle GenerateParticle(uint time)
        {
            var halfSpread = SpreadInDegrees / 2;
            var angle = Vector.AngleInDegrees + (Services.Random.Next(SpreadInDegrees) - halfSpread);
            var vector = Vector.FromAngleInDegrees(angle) * 10;

            var particle = ParticleFactory(time);
            particle.Velocity = vector;
            particle.Location = Location;
            return particle;
        }
    }

    public class CircleBurstEmitter : ParticleEmitter
    {
        public int ParticleCount { get; set; } = 250;
        public int Spread { get; set; }
        public int MinVelocity { get; set; } = 5;
        public int MaxVelocity { get; set; } = 10;

        private bool fired = false;
        private uint lastFireTime;
        public CircleBurstEmitter(uint creationTime) : base(creationTime)
        {}


        public override void Update(FrameTime time)
        {
            //if(time.Time - lastFireTime > 1000)
            if(!fired)
            {
                lastFireTime = time.Time;

                var angle = 0.0;
                var angleStep = 360 / (double) ParticleCount;

                for (var i = 0; i < ParticleCount; i++)
                {
                    angle = Services.Random.Next(360);
                    var vector = Vector.FromAngleInDegrees(angle) * Services.Random.Next(MinVelocity, MaxVelocity);

                    var particle = ParticleFactory(time.Time);
                    particle.Velocity = vector;
                    particle.Location = Location;
                    Services.Game.Particles.AddParticle(particle);
                }
                fired = true;
                Dispose();
            }
        }
    }
}
