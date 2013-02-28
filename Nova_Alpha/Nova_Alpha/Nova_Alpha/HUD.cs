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
    class HUD
    {
        private SpriteEx hud;

        public HUD()
        {
            hud = new SpriteEx();
        }

        public void Init(ContentManager cm, string name)
        {
            hud.Init(cm, name, new Vector2(0, 0), new Rectangle(0, 0, 0, 0), new Vector2(1), Color.White);
        }

        public void Update(Vector2 pos, float angle)
        {
            hud.Pos = pos;
            hud.Angle = angle;
        }

        public void Draw(SpriteBatch sb)
        {
            hud.Draw(sb);
        }
    }
}
