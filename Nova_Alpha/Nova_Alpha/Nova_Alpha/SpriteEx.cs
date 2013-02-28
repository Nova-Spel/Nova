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
    public class SpriteEx : Sprite
    {
        Rectangle clip;
        float angle, depth;
        

        SpriteEffects effect;

        Vector2 scale;
        //Color color;
        public SpriteEx()
        {
            clip = new Rectangle(0,0,0,0);
            angle = 0;
            depth = 0;
            effect = SpriteEffects.None;
            scale = new Vector2(0,0);
        }
        public void Init(ContentManager cm, string name, Vector2 p, Rectangle c, Vector2 s, Color color, bool useOrigin = false, float d = 0.0f, float a = 0.0f, SpriteEffects e = SpriteEffects.None)
        {
            base.Init(cm, name, p, color, 1.0f, useOrigin);
            if (c.Width == 0 || c.Height == 0)
            {
                clip = new Rectangle(0, 0, text.Width, text.Height);
            }
            else
            {
                clip = c;
            }

            if (useOrigin)
            {
                origin.X = clip.Width / 2;
                origin.Y = clip.Height / 2;
            }

            angle = a;
            depth = d;
            effect = e;
            scale = s;
        }
        public override void Draw(SpriteBatch sb)
        {
            if(text != null)
                sb.Draw(text, this.pos, clip, Color.White, angle, origin, scale, effect, depth);
        }
        public void Draw(SpriteBatch sb, AnimationManager animationManager)
        {
            if (text != null)
                sb.Draw(text, this.pos, animationManager.GetSourceRect, Color.White, angle, origin, scale, effect, depth);
        }
        public SpriteEffects Effect
        {
            get { return effect; }
            set { effect = value; }
        }
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }
    }
}
