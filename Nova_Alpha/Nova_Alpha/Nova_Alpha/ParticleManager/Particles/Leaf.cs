﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nova_Alpha
{
    class Leaf : Particle
    {
        Vector2 planetPosition;
<<<<<<< HEAD
        float planetRadius;

        Vector2 right;

=======

        Vector2 right;
>>>>>>> origin/David
        Vector2 windStrength;
        Vector2 windMin;
        Vector2 windMax;

        Vector2 sinRange;
<<<<<<< HEAD
        float sinTimer;
        float sinValue;

        Vector2 strayDistance;

=======

        Vector2 strayDistance;

        float planetRadius;
        float sinTimer;
        float sinValue;

>>>>>>> origin/David
        float rotationSpeed;

        bool atRest;

        public Leaf(Texture2D texture, Random random, float lifeTime, float fadeTime, Vector2 planetPosition, float planetRadius, Vector2 strayDistance, Vector2 windMin, Vector2 windMax, Vector2 sinRange)
            : base(texture, random, lifeTime, fadeTime)
        {
            this.planetPosition = planetPosition;
            this.planetRadius = planetRadius;
            this.strayDistance = strayDistance;
            this.windMin = windMin;
            this.windMax = windMax;
            this.sinRange = sinRange;

            position = new Vector2((float)random.NextDouble() * strayDistance.X, (float)random.NextDouble() * strayDistance.Y);

            right = new Vector2(1.0f, 0.0f);

            sinTimer = (float)(random.NextDouble() * Math.PI);
            rotation = (float)(random.NextDouble() * Math.PI);

            atRest = false;
        }


        public override bool Update(float delta)
        {
            if (!atRest)
            {
                if ((planetPosition - position).Length() > planetRadius)
                {
                    velocity = planetPosition - position;
                    velocity.Normalize();

                    right.X = velocity.Y;
                    right.Y = -velocity.X;

                    right.Normalize();

<<<<<<< HEAD
                    position += velocity * windStrength.Y + (float)Math.Sin(sinTimer) * sinValue * right + windStrength.X * right;
=======
                    position += velocity * windStrength.Y + (float)Math.Sin(sinTimer) * right + windStrength.X * right;
>>>>>>> origin/David

                    sinTimer += 0.1f;

                    rotation += rotationSpeed;
                }
                else
                    atRest = true;
            }
            else
                return base.Update(delta);

            return true;
        }

        public override void SetStartingPosition(Vector2 startingPosition)
        {
            position += startingPosition;

            windStrength = new Vector2(windMin.X + (float)random.NextDouble() * (windMax.X - windMin.X), windMin.Y + (float)random.NextDouble() * (windMax.Y - windMin.Y));

            rotationSpeed = (float)random.NextDouble() * 0.33f;
<<<<<<< HEAD
            sinValue = sinRange.X + (float)random.NextDouble() * (sinRange.Y - sinRange.X);
=======
            sinValue = sinRange.X + (float)random.NextDouble() * (sinRange.X - sinRange.X);
>>>>>>> origin/David
        }

        public override Particle GetCopy()
        {
            return new Leaf(texture, random, lifeTime, fadeTime, planetPosition, planetRadius, strayDistance, windMin, windMax, sinRange);
        }
    }
}
