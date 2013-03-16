using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Nova_Alpha
{
    class ParticleManager
    {
        LinkedList<Particle> particles;
        LinkedList<ParticleEmitter> emitters;

        public ParticleManager()
        {
            particles = new LinkedList<Particle>();
            emitters = new LinkedList<ParticleEmitter>();
        }

        public void AddEmitter(ParticleEmitter emitter)
        {
            emitters.AddLast(emitter);
        }

        public void Update(GameTime gameTime)
        {
            Update(gameTime.ElapsedGameTime.Milliseconds);
        }

        public void Update(float delta)
        {
            foreach (ParticleEmitter emitter in emitters) //Update all emitters
                emitter.Update(delta);


            //Update all particles
            LinkedListNode<Particle> currentNode = particles.First;

            while (currentNode != null)
            {
                bool isAlive = currentNode.Value.Update(delta);

                currentNode = currentNode.Next;

                if (!isAlive) //Remove if dead
                {
                    if (currentNode == null)
                        particles.RemoveLast();
                    else
                        particles.Remove(currentNode.Previous);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in particles)
                particle.Draw(spriteBatch);
        }

        public void AddParticle(Particle particle)
        {
            particles.AddLast(particle);
        }

        public void Reset()
        {
            particles = new LinkedList<Particle>();
            emitters = new LinkedList<ParticleEmitter>();
        }
    }
}
