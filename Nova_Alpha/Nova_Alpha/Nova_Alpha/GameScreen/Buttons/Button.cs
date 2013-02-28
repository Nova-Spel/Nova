using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Nova_Alpha
{
    abstract class Button
    {
        protected Vector2 size;

        protected Vector2 targetPos;
        protected Vector2 startingPos;

        public Sprite backgroundTexture;
        public Text text;

        protected Rectangle hitbox;
        protected Vector2 hitboxOffset;

        protected bool selected;

        public event EventHandler OnClicked;
        public event EventHandler OnReleased;

        protected enum State { None, FadeIn, FadeOut };
        protected State currentState;

        public Button()
        {
            text = new Text();
            size = new Vector2(0);

            backgroundTexture = new Sprite();
            hitbox = Rectangle.Empty;

            selected = false;
        }

        public virtual void Init(ContentManager cm, string bgPath, string path, string t, Color bgColor, Color c, Vector2 targetPos, bool useOrigin, Vector2 hitboxOffset)
        {
            text.Init(cm, path, t, c, targetPos, useOrigin);
            size = text.Size;

            backgroundTexture.Init(cm, bgPath, targetPos, bgColor, 1.0f, useOrigin);

            backgroundTexture.Alpha = 0.0f;
            this.hitboxOffset = hitboxOffset;

            hitbox = new Rectangle((int)(targetPos.X - backgroundTexture.GetOrigin.X - hitboxOffset.X), (int)(targetPos.Y - backgroundTexture.GetOrigin.Y - hitboxOffset.Y), (int)(backgroundTexture.GetTexture.Width + hitboxOffset.X * 2), (int)(backgroundTexture.GetTexture.Height + hitboxOffset.Y * 2));
            this.targetPos = targetPos;
            this.startingPos = targetPos - new Vector2(300.0f, 0.0f);

            //SetPos(startingPos);
            text.Alpha = 0.0f;
        }

        public abstract void Update();

        public abstract void Update(bool isSelected);

        protected virtual void Fade()
        {
            switch (currentState)
            {
                case State.FadeIn:
                    SetPos(new Vector2(MathHelper.Lerp(backgroundTexture.Pos.X, targetPos.X, 0.1f), backgroundTexture.Pos.Y));

                    text.Alpha = (1 - (targetPos.X - backgroundTexture.Pos.X) / (targetPos.X - startingPos.X));
                    break;
                case State.FadeOut:
                    SetPos(new Vector2(MathHelper.Lerp(backgroundTexture.Pos.X, startingPos.X, 0.1f), backgroundTexture.Pos.Y));

                    text.Alpha = (1 - (targetPos.X - backgroundTexture.Pos.X) / (targetPos.X - startingPos.X));

                    if (Vector2.Distance(startingPos, backgroundTexture.Pos) <= 5.0f)
                        text.Alpha = 0.0f;

                    backgroundTexture.Alpha = 0.0f;
                    break;
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {
            backgroundTexture.Draw(sb);
            text.Draw(sb);
        }

        public void FadeIn()
        {
            currentState = State.FadeIn;

            SetPos(startingPos);
            text.Alpha = 0.0f;
            backgroundTexture.Alpha = 0.0f;
        }

        public void FadeOut()
        {
            currentState = State.FadeOut;
            backgroundTexture.Alpha = 0.0f;
        }

        protected void Clicked()
        {
            if (OnClicked != null)
                OnClicked(this, EventArgs.Empty);
        }

        protected void Released()
        {
            if (OnReleased != null)
                OnReleased(this, EventArgs.Empty);
        }

        protected virtual void SetPos(Vector2 position)
        {
            backgroundTexture.Pos = position;
            text.Pos = position;
        }

        public string TextString
        {
            set
            {
                text.TextString = value;
                size = text.Size;
            }
        }

        public bool IsSelected { get { return selected; } }
    }
}
