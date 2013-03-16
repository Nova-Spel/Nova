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
    class Planet
    {
        SpriteEx mountain;
        SpriteEx tree;
        SpriteEx grass;
        SpriteEx planet;
        Vector2 pos;

        float gravity, radius, gravityField;


        Collectible[] moons;

        public Planet()
        {
            mountain = new SpriteEx();
            tree = new SpriteEx();
            grass = new SpriteEx();
            planet = new SpriteEx();
            pos = new Vector2();
            gravity = 0;
            radius = 2048;
            gravityField = 3048;
        }


        /// <summary>
        /// Ladddar in en planet
        /// </summary>
        /// <param name="cm">Content</param>
        /// <param name="name">Namn på xml</param>
        public void Init(ContentManager cm, string p, string g, Collectible[] c, Vector2 pos, Vector2 textureSize, float radius, float gravityField)
        {
            this.pos = pos;
            planet.Init(cm, p, pos, new Rectangle(), new Vector2(radius * 2 / textureSize.X, radius * 2 / textureSize.Y), Color.White, true);
            if (g != "")
            {
                grass.Init(cm, g, pos, new Rectangle(), new Vector2(1), Color.White, true);
                grass.GetOrigin = new Vector2(grass.GetTexture.Width, grass.GetTexture.Height);
            }
            moons = new Collectible[c.Length];
            for(uint i=0; i<moons.Length; i++)
            {
                moons[i] = c[i];
            }

            this.radius = radius;
            this.gravityField = radius + gravityField;
        }

        public void Draw(SpriteBatch sb)
        {
            planet.Draw(sb);
            grass.Draw(sb);
            foreach (Collectible x in moons)
            {
                x.Draw(sb);
            }
        }

        public Vector2 Pos
        {
            get { return pos; }
        }
        public float Radius
        {
            get { return radius; }
        }
        public float GravityField
        {
            get { return gravityField; }
            set { gravityField = value; }
        }
    }
}
