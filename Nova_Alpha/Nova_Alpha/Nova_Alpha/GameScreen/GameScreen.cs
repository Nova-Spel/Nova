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
using System.Globalization;
using System.Diagnostics;

namespace Nova_Alpha
{
    class GameScreen : Screen
    {
        Player player;
        List<Planet> planetList;
        Camera cam;
        HUD hud;

        Sprite[,] background;

        ParticleManager particleManager;

        Random random;

        CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone(); //Behövs för att . ska användas som decimaltecken i XML

        public GameScreen()
        {
            hud = new HUD();
            player = new Player();
            planetList = new List<Planet>();
            particleManager = new ParticleManager();

            random = new Random();
            //planet = new Planet[10];
            //for(uint i=0; i<10; i++)
            //{
            //    planet[i] = new Planet();
            //}
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
            //player.Init(content, "atlas_spritesheet", new Vector2(3048, 0));

            player.Init(content, "atlas_spritesheet", LoadWorld("mapTemplate.xml", content));

            
            //for(uint i=0; i<10; i++)
            //{
            //    planet[i].Init(content, "isplanet", "", c, new Vector2(3048 + (i * 6096), 3048));
            //}
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

            for (int i = 0; i < planetList.Count; i++)
            {
                temppos = player.mSprite.Pos - planetList[i].Pos;
                if (temppos.Length() < planetList[i].GravityField)
                {
                    player.Planet = i;
                    length = temppos.Length();
                }
            }
            if (player.Planet != -1)
            {
                if (length < planetList[player.Planet].Radius + 2)
                {
                    player.CanJump = true;
                }
                else
                {
                    player.CanJump = false;
                }
                if (length > planetList[player.Planet].Radius)
                {
                    player.InPlanet = true;
                    player.Dist = length - planetList[player.Planet].Radius;
                }
                else
                {
                    player.InPlanet = false;
                }
                Vector2 temp = player.mSprite.Pos - planetList[player.Planet].Pos;
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

            particleManager.Update(delta);
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
                foreach (Planet x in planetList)
                {
                    x.Draw(spriteBatch);
                }
            }
            else
            {
                planetList[player.Planet].Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            hud.Draw(spriteBatch);

            particleManager.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void Reset()
        {

        }

<<<<<<< HEAD
        #region World loading
=======
>>>>>>> origin/David
        /// <summary>
        /// Loads the given world
        /// </summary>
        /// <param name="fileName">"World to load".xml</param>
        public Vector2 LoadWorld(string fileName, ContentManager contentManager)
        {
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            XmlDocument file = new XmlDocument();
            file.Load("Content\\" + fileName);

            //Select all planet-nodes
            XmlNodeList planets = file.SelectNodes("//planet");

            //Get player spawn and set value to return
            XmlNode node = file.SelectSingleNode("//playerSpawn");

            Vector2 playerPos = new Vector2(float.Parse(node.ChildNodes.Item(0).InnerText), float.Parse(node.ChildNodes.Item(1).InnerText));

            //Loop through and create all planets
            foreach (XmlNode currentPlanet in planets)
            {
                Planet newPlanet = new Planet();

                newPlanet.Init(contentManager, currentPlanet["texture"].InnerText, "", new Collectible[] { }, ParseVectorFromXML(currentPlanet["position"]), ParseVectorFromXML(currentPlanet["textureSize"]), ParseFloatFromXML(currentPlanet["radius"]), ParseFloatFromXML(currentPlanet["radius"]) + ParseFloatFromXML(currentPlanet["gravitationField"]));

                planetList.Add(newPlanet);

                XmlNodeList emitters = currentPlanet.SelectNodes("//emitter");

<<<<<<< HEAD
                Dictionary<string, XmlNode> idNodes = new Dictionary<string, XmlNode>();

=======
>>>>>>> origin/David
                //Loops through all emitters and creates a new one
                foreach (XmlNode emitter in emitters)
                {
                    Particle particle;
<<<<<<< HEAD
                    ParticleEmitter newEmitter;

                    if (emitter.Attributes["id"] != null)
                        idNodes.Add(emitter.Attributes["id"].Value, emitter);

                    if (emitter.Attributes["copyFrom"] == null)
                    {
                        switch (emitter["type"].InnerText)
                        {
                            case "lightbug":
                                particle = new Lightbug(contentManager.Load<Texture2D>("particles\\lightbug"), random, ParseFloatFromXML(emitter["lifeTime"]), ParseFloatFromXML(emitter["fadeTime"]), newPlanet.Pos, ParseFloatFromXML(emitter["strayDistance"]));
                                break;
                            case "leafbrown":
                                particle = new Leaf(contentManager.Load<Texture2D>("particles\\leafbrown"), random, ParseFloatFromXML(emitter["lifeTime"]), ParseFloatFromXML(emitter["fadeTime"]), newPlanet.Pos, newPlanet.Radius, ParseVectorFromXML(emitter["stray"]), ParseVectorFromXML(emitter["windMin"]), ParseVectorFromXML(emitter["windMax"]), ParseVectorFromXML(emitter["sinRange"]));
                                break;
                            case "leafgreen":
                                particle = new Leaf(contentManager.Load<Texture2D>("particles\\leafgreen"), random, ParseFloatFromXML(emitter["lifeTime"]), ParseFloatFromXML(emitter["fadeTime"]), newPlanet.Pos, newPlanet.Radius, ParseVectorFromXML(emitter["stray"]), ParseVectorFromXML(emitter["windMin"]), ParseVectorFromXML(emitter["windMax"]), ParseVectorFromXML(emitter["sinRange"]));
                                break;
                            case "leafred":
                                particle = new Leaf(contentManager.Load<Texture2D>("particles\\leafred"), random, ParseFloatFromXML(emitter["lifeTime"]), ParseFloatFromXML(emitter["fadeTime"]), newPlanet.Pos, newPlanet.Radius, ParseVectorFromXML(emitter["stray"]), ParseVectorFromXML(emitter["windMin"]), ParseVectorFromXML(emitter["windMax"]), ParseVectorFromXML(emitter["sinRange"]));
                                break;
                            default:
                                throw new Exception("Could not find emitter type! Check spelling and/or implementation");
                        }

                        //Create new emitter
                        newEmitter = new ParticleEmitter(ParseVectorFromXML(emitter["position"]), particleManager, particle, ParseFloatFromXML(emitter["cooldownTime"]), ParseFloatFromXML(emitter["spawnChance"]), ParseIntFromXML(emitter["maxParticleAmount"]), random);

                    }
                    else
                    {
                        XmlNode copyNode = idNodes[emitter.Attributes["copyFrom"].Value];

                        switch (emitter["type"].InnerText)
                        {
                            case "lightbug":
                                particle = new Lightbug(contentManager.Load<Texture2D>("particles\\lightbug"), random, ParseFloatFromXML(copyNode["lifeTime"]), ParseFloatFromXML(copyNode["fadeTime"]), newPlanet.Pos, ParseFloatFromXML(copyNode["strayDistance"]));
                                break;
                            case "leafbrown":
                                particle = new Leaf(contentManager.Load<Texture2D>("particles\\leafbrown"), random, ParseFloatFromXML(copyNode["lifeTime"]), ParseFloatFromXML(copyNode["fadeTime"]), newPlanet.Pos, newPlanet.Radius, ParseVectorFromXML(copyNode["stray"]), ParseVectorFromXML(copyNode["windMin"]), ParseVectorFromXML(copyNode["windMax"]), ParseVectorFromXML(copyNode["sinRange"]));
                                break;
                            case "leafgreen":
                                particle = new Leaf(contentManager.Load<Texture2D>("particles\\leafgreen"), random, ParseFloatFromXML(copyNode["lifeTime"]), ParseFloatFromXML(copyNode["fadeTime"]), newPlanet.Pos, newPlanet.Radius, ParseVectorFromXML(copyNode["stray"]), ParseVectorFromXML(copyNode["windMin"]), ParseVectorFromXML(copyNode["windMax"]), ParseVectorFromXML(copyNode["sinRange"]));
                                break;
                            case "leafred":
                                particle = new Leaf(contentManager.Load<Texture2D>("particles\\leafred"), random, ParseFloatFromXML(copyNode["lifeTime"]), ParseFloatFromXML(copyNode["fadeTime"]), newPlanet.Pos, newPlanet.Radius, ParseVectorFromXML(copyNode["stray"]), ParseVectorFromXML(copyNode["windMin"]), ParseVectorFromXML(copyNode["windMax"]), ParseVectorFromXML(copyNode["sinRange"]));
                                break;
                            default:
                                throw new Exception("Could not find emitter type! Check spelling and/or implementation");


                        }

                        //Create new emitter
                        newEmitter = new ParticleEmitter(ParseVectorFromXML(copyNode["position"]), particleManager, particle, ParseFloatFromXML(copyNode["cooldownTime"]), ParseFloatFromXML(copyNode["spawnChance"]), ParseIntFromXML(copyNode["maxParticleAmount"]), random);
                    }
                    

=======

                    switch (emitter["type"].InnerText)
                    {
                        case "lightbug":
                            particle = new Lightbug(contentManager.Load<Texture2D>("particles\\lightbug"), random, ParseFloatFromXML(emitter["lifeTime"]), ParseFloatFromXML(emitter["fadeTime"]), newPlanet.Pos, ParseVectorFromXML(emitter["stray"]));
                            break;
                        case "leafbrown":
                            particle = new Leaf(contentManager.Load<Texture2D>("particles\\leafbrown"), random, ParseFloatFromXML(emitter["lifeTime"]), ParseFloatFromXML(emitter["fadeTime"]), newPlanet.Pos, newPlanet.Radius, ParseVectorFromXML(emitter["stray"]), ParseVectorFromXML(emitter["windMin"]), ParseVectorFromXML(emitter["windMax"]), ParseVectorFromXML(emitter["sinRange"]));
                            break;
                        case "leafgreen":
                            particle = new Leaf(contentManager.Load<Texture2D>("particles\\leafgreen"), random, ParseFloatFromXML(emitter["lifeTime"]), ParseFloatFromXML(emitter["fadeTime"]), newPlanet.Pos, newPlanet.Radius, ParseVectorFromXML(emitter["stray"]), ParseVectorFromXML(emitter["windMin"]), ParseVectorFromXML(emitter["windMax"]), ParseVectorFromXML(emitter["sinRange"]));
                            break;
                        case "leafred":
                            particle = new Leaf(contentManager.Load<Texture2D>("particles\\leafred"), random, ParseFloatFromXML(emitter["lifeTime"]), ParseFloatFromXML(emitter["fadeTime"]), newPlanet.Pos, newPlanet.Radius, ParseVectorFromXML(emitter["stray"]), ParseVectorFromXML(emitter["windMin"]), ParseVectorFromXML(emitter["windMax"]), ParseVectorFromXML(emitter["sinRange"]));
                            break;
                        default:
                            throw new Exception("Could not find emitter type! Check spelling and/or implementation");
                    }

                    //Create new emitter
                    ParticleEmitter newEmitter = new ParticleEmitter(ParseVectorFromXML(emitter["position"]), particleManager, particle, ParseFloatFromXML(emitter["cooldownTime"]), ParseFloatFromXML(emitter["spawnChance"]), ParseIntFromXML(emitter["maxParticleAmount"]), random);
>>>>>>> origin/David

                    particleManager.AddEmitter(newEmitter);
                }
            }

            return playerPos;
        }

        /// <summary>
        /// Parses a Vector2 from the given node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Vector2 ParseVectorFromXML(XmlNode node)
        {
            return new Vector2(float.Parse(node["x"].InnerText, cultureInfo), float.Parse(node["y"].InnerText, cultureInfo));
        }

        /// <summary>
        /// Parses a Point from the given node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Point ParsePointFromXML(XmlNode node)
        {
            return new Point(int.Parse(node["x"].InnerText, cultureInfo), int.Parse(node["y"].InnerText, cultureInfo));
        }

        /// <summary>
        /// Prases a float from the given node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private float ParseFloatFromXML(XmlNode node)
        {
            return float.Parse(node.InnerText, cultureInfo);
        }

        /// <summary>
        /// Parses an int from the given node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private int ParseIntFromXML(XmlNode node)
        {
            return int.Parse(node.InnerText, cultureInfo);
        }
<<<<<<< HEAD
        #endregion
=======
>>>>>>> origin/David
    }
}
