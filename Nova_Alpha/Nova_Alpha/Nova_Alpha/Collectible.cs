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
    class Collectible
    {
        Vector2 pos;
        SpriteEx sprite;
        public Collectible()
        {
            pos = new Vector2();
            sprite = new SpriteEx();
        }
        public void Init(ContentManager cm, Vector2 pos)
        {
            this.pos = pos;
            sprite.Init(cm, "moonshine", pos, new Rectangle(0, 0, 50, 50), new Vector2(1), Color.White, true);
        }
        public void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb);
        }
    }
}
