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
    public class Text
    {
        string text;

        SpriteFont font;
        Color color;
        Vector2 pos;
        Vector2 origin;

        float alpha;
        public Text()
        {
            text = "";
            font = null;
            color = Color.Black;
            pos = new Vector2(0);
            origin = Vector2.Zero;
            alpha = 1.0f;
        }

        public void Init(ContentManager cm, string path, string t, Color c, Vector2 p, bool useOrigin)
        {
            font = cm.Load<SpriteFont>(path);
            text = t;
            color = c;
            pos = p;

            if (useOrigin)
                origin = new Vector2((float)Math.Round(font.MeasureString(text).X / 2.0f), (float)Math.Round(font.MeasureString(text).Y / 2.0f));
        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(font, text, pos, color * alpha, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        public Vector2 Size
        {
            get { return font.MeasureString(text); }
        }
        public string TextString
        {
            get { return text; }
            set { text = value; }
        }   

        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }
        
    }
}
