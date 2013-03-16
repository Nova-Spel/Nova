using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nova_Alpha
{
    class Lightbug : Particle
    {
        float xTimer;
        float yTimer;

        float xTimerInc;
        float yTimerInc;

        Vector2 startingPosition;
        Vector2 targetPosition;
        Vector2 lerpPos;

        Vector2 oldTargetPosition;

        Vector2 strayDistance;

        Vector2 right;
        Vector2 planetPosition;

        float xLerpTimer;
        float xLerpTimerInc;
        float yLerpTimer;
        float yLerpTimerInc;

        public Lightbug(Texture2D texture, Random random, float lifeTime, float fadeTime, Vector2 planetPosition, Vector2 strayDistance)
            : base(texture, random, lifeTime, fadeTime)
        {
            this.strayDistance = strayDistance;
            this.planetPosition = planetPosition;

            position = new Vector2((float)random.NextDouble() * strayDistance.X * GetPosOrNegValue(), (float)random.NextDouble() * strayDistance.Y * GetPosOrNegValue());

            xTimerInc = (float)random.NextDouble() * 0.08f;
            yTimerInc = (float)random.NextDouble() * 0.08f;

            xTimer = (float)random.NextDouble();
            yTimer = (float)random.NextDouble();

            xLerpTimerInc = (float)random.NextDouble() * 0.01f;
            if (xLerpTimerInc < 0.001f)
                xLerpTimerInc = 0.001f;

            yLerpTimerInc = (float)random.NextDouble() * 0.01f;
            if (yLerpTimerInc < 0.001f)
                yLerpTimerInc = 0.001f;

            xLerpTimer = 1.0f;
            yLerpTimer = 1.0f;
        }

        public override void SetStartingPosition(Vector2 startingPosition)
        {
            this.startingPosition = startingPosition + position;

            position = this.startingPosition;
            targetPosition = position;
            oldTargetPosition = targetPosition;

            lerpPos = position;

            Vector2 down = planetPosition - this.startingPosition;
            down.Normalize();

            right = new Vector2(down.Y, -down.X);
        }

        public override bool Update(float delta)
        {
            if (xLerpTimer == 1.0f && yLerpTimer == 1.0f)
            {
                oldTargetPosition = targetPosition;
                targetPosition = new Vector2(startingPosition.X + (float)random.NextDouble() * strayDistance.X * GetPosOrNegValue(), startingPosition.Y + (float)random.NextDouble() * strayDistance.Y * GetPosOrNegValue());

                xLerpTimer = 0.0f;
                yLerpTimer = 0.0f;
            }

            lerpPos.X = MathHelper.Lerp(oldTargetPosition.X, targetPosition.X, xLerpTimer);
            lerpPos.Y = MathHelper.Lerp(oldTargetPosition.Y, targetPosition.Y, yLerpTimer);

            xLerpTimer += xLerpTimerInc;
            if (xLerpTimer > 1.0f)
                xLerpTimer = 1.0f;

            yLerpTimer += yLerpTimerInc;
            if (yLerpTimer > 1.0f)
                yLerpTimer = 1.0f;

            position.X = lerpPos.X + (float)Math.Sin(xTimer) * 10.0f;
            position.Y = lerpPos.Y + (float)Math.Sin(yTimer) * 10.0f;

            xTimer += yTimerInc;
            yTimer += xTimerInc;

            return true;
        }

        public override Particle GetCopy()
        {
            return new Lightbug(texture, random, lifeTime, fadeTime, planetPosition, strayDistance);
        }
    }
}
