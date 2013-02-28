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
    public class Sprite
    {
        protected Texture2D text;
        protected Vector2 pos;
        protected Vector2 origin;
        protected Color color;
        protected float alpha;
        public Sprite()
        {
            text = null;
            pos = new Vector2(0,0);
            origin = Vector2.Zero;
            alpha = 1.0f;
        }
        public virtual void Init(ContentManager cm, string name, Vector2 p, Color color, float alpha, bool useOrigin)
        {
            text = cm.Load<Texture2D>(name);
            pos = p;
            this.color = color;
            this.alpha = alpha;

            if (useOrigin)
                origin = new Vector2((float)Math.Round(text.Width / 2.0f), (float)Math.Round(text.Height / 2.0f));
        }
        public virtual void Draw(SpriteBatch sb)
        {
            if(text != null)
                sb.Draw(text, pos, null, color * alpha, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
        public bool isVectorIn(Vector2 v)
        {
            Rectangle temppos = new Rectangle((int)(pos.X - origin.X),(int)(pos.Y - origin.Y),text.Width,text.Height);
            return temppos.Left <= v.X && v.X <= temppos.Right && temppos.Top <= v.Y && v.Y <= temppos.Bottom;
        }
        public float? isRayIn(Ray r)
        {
            Vector2 vtemp = pos - origin;
            BoundingBox b = new BoundingBox(new Vector3(vtemp, 0), new Vector3(vtemp + new Vector2(text.Width, text.Height), 0));
            float? temp = r.Intersects(b);
            if(temp == null)
                return -1.0f;
            else 
                return temp;
        }
        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public Vector2 GetOrigin { get { return origin; } set { origin = value;  } }
        public Texture2D GetTexture { get { return text; } }

        public Vector2 Pos { get { return pos; } set { pos = value; } }
    }
}