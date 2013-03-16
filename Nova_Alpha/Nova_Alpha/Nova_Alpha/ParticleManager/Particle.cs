using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nova_Alpha
{
    abstract class Particle
    {
        Sprite sprite;
        public Texture2D texture;

        public Vector2 position;
        public Vector2 velocity;
        public Vector2 origin;

        public float rotation;

        public float scale;
        public float alpha;
        public float lifeTime;
        public float fadeTime;

        public Color color;

        public event EventHandler OnDeath;

        protected Random random;

        public Particle(Texture2D texture, Random random, float lifeTime, float fadeTime)
        {
            this.texture = texture;
            this.random = random;
            this.lifeTime = lifeTime;
            this.fadeTime = fadeTime;

            position = Vector2.Zero;
            velocity = Vector2.Zero;
            origin = new Vector2(texture.Height / 2, texture.Width / 2);

            rotation = 0.0f;

            scale = 1.0f;
            alpha = 1.0f;

            color = Color.White;
        }

        ~Particle()
        {
            if (OnDeath != null)
                OnDeath(this, EventArgs.Empty); //Called so the emitter can update its' particle count
        }

        public virtual bool Update(float delta)
        {
            lifeTime -= delta * 1000.0f;

            if (fadeTime != 0)
            {
                if (lifeTime < fadeTime)
                {
                    alpha = lifeTime / fadeTime;

                    if (alpha <= 0.0f)
                    {                   
                        if (OnDeath != null)
                            OnDeath(this, EventArgs.Empty);

                        return false;
                    }
                }
            }
            else if (lifeTime <= 0)
            {
                alpha = 0.0f;

                if (OnDeath != null)
                    OnDeath(this, EventArgs.Empty);
                
                return false;
            }

            return true;
        }

        public abstract void SetStartingPosition(Vector2 startingPosition);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color * alpha, rotation, origin, scale, SpriteEffects.None, 0.0f);
        }

        public abstract Particle GetCopy();

        protected int GetPosOrNegValue()
        {
            return random.Next(2) == 0 ? -1 : 1;
        }
    }
}
