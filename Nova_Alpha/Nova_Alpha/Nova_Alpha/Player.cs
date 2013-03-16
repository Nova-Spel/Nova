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
    public class Player
    {
        Vector2 pos;
        Vector2 up, right;

        float speed;
        float jumping;

        int planet;

        bool onPlanet, inPlanet, canJump;

        float dist;
        float charge;

        SpriteEx sprite;

        AnimationManager animationManager;

        public Player()
        {
            sprite = new SpriteEx();
            pos = new Vector2();
            up = new Vector2(0, -1);
            right = new Vector2(1, 0);
            speed = 600;
            jumping = -0.1f;
            charge = 0;

            animationManager = AnimationManager.CreateFromFile("Content\\mainchar.XML");
        }

        public void Init(ContentManager cm, string path, Vector2 p)
        {
            pos = p;
            sprite.Init(cm, path, p + new Vector2(640, 360), new Rectangle(0,0,128,128), new Vector2(1), Color.White);
            sprite.GetOrigin = new Vector2(64, 128);
        }

        public void Update(float delta)
        {
             Vector2 speedtemp = right * delta * speed;

            if (Input.IsGamePadConnected && planet != -1)
            {
                if (Input.IsGamePadButtonDown(Buttons.DPadRight) || Input.IsGamePadButtonDown(Buttons.LeftThumbstickRight))
                {
                    sprite.Pos += speedtemp;
                    sprite.Effect = SpriteEffects.None;

                    animationManager.SetAnimation("walk", false);
                }
                if (Input.IsGamePadButtonDown(Buttons.DPadLeft) || Input.IsGamePadButtonDown(Buttons.LeftThumbstickLeft))
                {
                    sprite.Pos -= speedtemp;
                    sprite.Effect = SpriteEffects.FlipHorizontally;

                    animationManager.SetAnimation("walk", false);
                }
                if (Input.IsGamePadButtonClicked(Buttons.A) && canJump)
                {
                    if (charge < 0.5f)
                        jumping = 0.5f;
                    else
                    {
                        jumping = charge;
                        charge = 0;
                    }
                    animationManager.SetAnimation("jump", false);
                }
                else if (jumping <= 0)
                {
                    animationManager.SetAnimation("idle", true);
                }
            }
            else
            {
                if (Input.IsKeyClicked(Keys.Space) && canJump)
                {
                    if (charge < 0.5f)
                        jumping = 0.5f;
                    else
                        jumping = charge;

                    animationManager.SetAnimation("jump", false);
                }
                if (Input.IsKeyDown(Keys.Right))
                {
                    sprite.Pos += speedtemp;
                    sprite.Effect = SpriteEffects.None;

                    animationManager.SetAnimation("walk", false);
                }
                else if (Input.IsKeyDown(Keys.Left))
                {
                    sprite.Pos -= speedtemp;
                    sprite.Effect = SpriteEffects.FlipHorizontally;

                    animationManager.SetAnimation("walk", false);
                }
                else if(jumping <= 0)
                {
                    animationManager.SetAnimation("idle", true);
                }
            }

            if(jumping > 0)
            {
                sprite.Pos -= delta * 600 * up;
                jumping -= delta;
            }
            else if (inPlanet)
            {
                if (dist < 14.816)
                {
                    sprite.Pos += up * dist;
                }
                else
                {
                    sprite.Pos += delta * 926 * up;
                }
            }

            pos = sprite.Pos - up * 480 - right * 640;

            animationManager.Update(delta);
        }
        public void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb, animationManager);
        }

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public SpriteEx mSprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        public Vector2 Up
        {
            get { return up; }
            set { up = value; }
        }
        public bool OnPlanet
        {
            get { return onPlanet; }
            set { onPlanet = value; }
        }

        public bool InPlanet
        {
            get { return inPlanet; }
            set { inPlanet = value; }
        }
        public bool CanJump
        {
            get { return canJump; }
            set { canJump = value; }
        }
        public float Angle
        {
            set
            {
                sprite.Angle = value;
                up.X = (float)Math.Cos(sprite.Angle + MathHelper.ToRadians(90));
                up.Y = (float)Math.Sin(sprite.Angle + MathHelper.ToRadians(90));
                right.X = -(float)Math.Cos(sprite.Angle + Math.PI);
                right.Y = -(float)Math.Sin(sprite.Angle + Math.PI);
            }
            get { return sprite.Angle; }
        }
        public float Dist
        {
            get { return dist; }
            set { dist = value; }
        }
        public Vector2 Right
        {
            get { return right; }
            set { right = value; }
        }

        public int Planet
        {
            get { return planet; }
            set { planet = value; }
        }
    }
}
