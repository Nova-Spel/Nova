using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nova_Alpha
{
    class ParticleEmitter
    {
        Vector2 position;

        float cooldownTime;
        float currentCooldownTime;
        float spawnChance;

        int maxParticleAmount;
        int currentParticleAmount;

        bool instantSpawn;

        Particle particle;

        ParticleManager particleManager;

        Random random;

        /// <summary>
        /// Creates a new particle emitter
        /// </summary>
        /// <param name="particleManager">Particle manager to use</param>
        /// <param name="particle">Particle to spawn</param>
        /// <param name="cooldownTime">Cooldown between particles (in ms)</param>
        /// <param name="spawnChance">Chance that this particle will spawn (per frame) when off cooldown</param>
        /// <param name="maxParticles">Max amount of particles</param>
        public ParticleEmitter(Vector2 position, ParticleManager particleManager, Particle particle, float cooldownTime, float spawnChance, int maxParticles, Random random)
        {
            this.position = position;
            this.particleManager = particleManager;
            this.particle = particle;
            this.cooldownTime = cooldownTime;
            this.spawnChance = spawnChance;
            this.maxParticleAmount = maxParticles;
            this.random = random;

            currentParticleAmount = 0;
            currentCooldownTime = 0.0f;

            instantSpawn = cooldownTime == 0.0f ? true : false;
        }

        public void Update(GameTime gameTime)
        {
            Update(gameTime.ElapsedGameTime.Milliseconds);
        }

        public void Update(float delta)
        {
            if (instantSpawn)
            {
                for (int i = currentParticleAmount; i < maxParticleAmount; i++)
                {
                    Particle newParticle = particle.GetCopy();

                    newParticle.SetStartingPosition(this.position);

                    newParticle.OnDeath += OnParticleDeath;

                    particleManager.AddParticle(newParticle); //Add copy of new particle

                    currentParticleAmount++;
                }
            }
            else
            {
                if (currentCooldownTime <= 0.0f)
                {
                    if (currentParticleAmount < maxParticleAmount)
                    {
                        if (random.NextDouble() < spawnChance) //If spawn
                        {
                            Particle newParticle = particle.GetCopy();

                            newParticle.SetStartingPosition(this.position);

                            newParticle.OnDeath += OnParticleDeath;

                            particleManager.AddParticle(newParticle); //Add copy of new particle

                            currentCooldownTime = cooldownTime;
                            currentParticleAmount++;
                        }

                    }
                }
                else
                {
                    currentCooldownTime -= delta;

                    if (currentCooldownTime <= 0)
                        currentCooldownTime = 0.0f;
                }
            }
        }

        /// <summary>
        /// Called when the particle dies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnParticleDeath(object sender, EventArgs args)
        {
            (sender as Particle).OnDeath -= OnParticleDeath;

            currentParticleAmount--;
        }
    }
}
