using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Nova_Alpha
{
    class ActionButton : Button
    {
        public override void Update()
        {
            if (Vector2.Distance(backgroundTexture.Pos, targetPos) <= 50.0f)
            {
                if (hitbox.Contains(Input.GetMousePositionPoint))
                {
                    backgroundTexture.Alpha = MathHelper.Lerp(backgroundTexture.Alpha, 1.0f, 0.1f);
                    selected = true;

                    if (Input.IsMouse1Clicked)
                        Clicked();
                }
                else
                {
                    backgroundTexture.Alpha = MathHelper.Lerp(backgroundTexture.Alpha, 0.0f, 0.1f);
                    selected = false;

                    if (backgroundTexture.Alpha <= 0.10f)
                        backgroundTexture.Alpha = 0.0f;
                }
            }

            Fade();
        }

        public override void Update(bool isSelected)
        {
            if (Vector2.Distance(backgroundTexture.Pos, targetPos) <= 50.0f)
            {
                if (isSelected)
                {
                    backgroundTexture.Alpha = MathHelper.Lerp(backgroundTexture.Alpha, 1.0f, 0.1f);

                    if (Input.IsKeyClicked(Keys.Enter))
                        Clicked();

                    if (Input.IsGamePadConnected)
                        if (Input.IsGamePadButtonClicked(Buttons.A) || Input.IsGamePadButtonClicked(Buttons.Start))
                            Clicked();
                }
                else
                    backgroundTexture.Alpha = MathHelper.Lerp(backgroundTexture.Alpha, 0.0f, 0.1f);
            }

            Fade();
        }
    }
}
