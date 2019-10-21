using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Particle
{
    public class ParticleSystem
    {
        private readonly LinkedList<Particle> particles = new LinkedList<Particle>();
        private readonly LinkedList<ParticleEmitter> emitters = new LinkedList<ParticleEmitter>();

        public void Update(FrameTime time)
        {
            UpdateParticles(time);
            UpdateEmitters(time);
        }

        private void UpdateParticles(FrameTime time)
        {
            var currentNode = particles.First;

            while (currentNode != null)
            {
                var nextNode = currentNode.Next;
                currentNode.Value.Update(time);

                if (currentNode.Value.IsDisposed)
                    particles.Remove(currentNode);

                currentNode = nextNode;
            }
        }

        private void UpdateEmitters(FrameTime time)
        {
            var currentNode = emitters.First;

            while (currentNode != null)
            {
                var nextNode = currentNode.Next;
                currentNode.Value.Update(time);

                if (currentNode.Value.IsDisposed)
                    emitters.Remove(currentNode);

                currentNode = nextNode;
            }
        }

        public void AddParticle(Particle particle)
        {
            particles.AddLast(particle);
        }

        public void AddEmitter(ParticleEmitter emitter)
        {
            emitters.AddLast(emitter);
        }
    }
}
