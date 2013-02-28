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
    class Enemy
    {
        Vector2 pos;
        Vector2 up, right;

        SpriteEx sprite;
        float speed;
        bool onPlanet;


        public Enemy()
        {
            pos = new Vector2();
            up = new Vector2(0,-1);
            right = new Vector2(1,0);
            sprite = new SpriteEx();
            speed = 0;
            onPlanet = true;
        }
        void Init(ContentManager cm, string path, Vector2 p, Vector2 o)
        {
            pos = p;
            up = p - o;
            up.Normalize();
            float angle = Vector2.Dot(up, new Vector2(0, -1)), temp = Vector2.Dot(up, new Vector2(1,0));

            angle = (float)Math.Acos(angle);
            if(temp < 0)
            {
                angle = (float)MathHelper.Pi * 2 - angle; 
            }
            right.X = -(float)Math.Cos(sprite.Angle + Math.PI);
            right.Y = -(float)Math.Sin(sprite.Angle + Math.PI);
            sprite.Init(cm, path, p, new Rectangle(0,0,0,0), new Vector2(1), Color.White, true,0.1f, angle);
        }
        void Update(float delta)
        { 
            
        }
        public bool OnPlanet
        {
            get { return onPlanet; }
            set { onPlanet = value; }
        }

    }
}
