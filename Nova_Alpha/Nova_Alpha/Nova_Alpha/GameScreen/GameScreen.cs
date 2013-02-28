using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Nova_Alpha
{
    class GameScreen : Screen
    {

        Player player;
        Planet[] planet;
        Camera cam;
        HUD hud;

        Sprite[,] background;

        public GameScreen()
        {
            hud = new HUD();
            player = new Player();
            planet = new Planet[10];
            for(uint i=0; i<10; i++)
            {
                planet[i] = new Planet();
            }
            cam = new Camera(new Vector2(0));
            background = new Sprite[10, 10];
            for(int i=0; i<10; i++)
            {
                for(int k=0; k<10; k++)
                {
                    background[i, k] = new Sprite();
                }
            }
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager content)
        {
            hud.Init(content, "HUD");
            Collectible[] c = new Collectible[1];
            c[0] = new Collectible();
            c[0].Init(content, new Vector2(700,300));
            player.Init(content, "atlas_spritesheet", new Vector2(3048, 0));

            
            for(uint i=0; i<10; i++)
            {
                planet[i].Init(content, "isplanet", "", c, new Vector2(3048 + (i * 6096), 3048));
            }
            for (int i = 0; i < 10; i++)
            {
                for (int k = 0; k < 10; k++)
                {
                    background[i, k].Init(content, "Background", new Vector2(i * 3000, k * 3000), Color.White, 1, false);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.IsKeyClicked(Keys.Escape))
                ChangeScreen(new StringEventArgs("PauseMenu"));

            float delta = gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            Vector2 temppos = new Vector2();
            float length = float.MaxValue;
            player.Planet = -1;

            for (int i = 0; i < planet.Length; i++)
            {
                temppos = player.mSprite.Pos - planet[i].Pos;
                if (temppos.Length() < planet[i].GravityField)
                {
                    player.Planet = i;
                    length = temppos.Length();
                }
            }
            if (player.Planet != -1)
            {
                if (length < planet[player.Planet].Radie + 2)
                {
                    player.CanJump = true;
                }
                else
                {
                    player.CanJump = false;
                }
                if (length > planet[player.Planet].Radie)
                {
                    player.InPlanet = true;
                    player.Dist = length - planet[player.Planet].Radie;
                }
                else
                {
                    player.InPlanet = false;
                }
                Vector2 temp = player.mSprite.Pos - planet[player.Planet].Pos;
                temp.Normalize();

                float angle = Vector2.Dot(temp, new Vector2(0, -1)), tempangle = Vector2.Dot(temp, new Vector2(1, 0));

                player.Angle = (float)Math.Acos(angle);
                if (tempangle < 0)
                {
                    player.Angle = (2 * MathHelper.Pi) - player.Angle;
                }
            }


            player.Update(delta);

            cam.rotation = player.Angle;
            //cam.position = player.Pos;
            cam.LookAt(player.Pos);

            hud.Update(player.Pos, player.Angle);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cam.GetViewMatrix());
            Vector2[] corner = new Vector2[4];
            Vector3 pos, dir;
            Ray[] rays = new Ray[4];

            corner[0] = player.Pos;
            corner[1] = corner[0] + (1280 * player.Up);
            corner[3] = corner[0] + (720 * player.Right) + (1280 * player.Up);
            corner[2] = corner[0] + (720 * player.Right);

            for(uint i=0; i<4; i++)
            {
                pos = new Vector3(corner[i], 0);
                dir = new Vector3(corner[(i + 1) % 4] - corner[i], 0);
                dir.Normalize();
                rays[i] = new Ray(pos, dir);
            }

            // funkar inte än
            bool test;
            for (uint x = 0; x < 10; x++ )
            {
                for (uint y = 0; y < 10; y++)
                {
                    test = false;
                    foreach (Ray r in rays)
                    {
                        float? temp = background[x, y].isRayIn(r);
                        if (temp != -1.0f && temp < 3000)
                            test = true;
                    }
                    if (test)
                    {
                        background[x, y].Draw(spriteBatch);
                    }
                }
            }
            if (player.Planet == -1)
            {
                foreach (Planet x in planet)
                {
                    x.Draw(spriteBatch);
                }
            }
            else
            {
                planet[player.Planet].Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            hud.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void Reset()
        {

        }
    }
}
