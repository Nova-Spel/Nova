using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Nova_Alpha
{
    class Slider : Button
    {
        public Sprite sliderBackgroundTexture;
        public Sprite sliderBlockTexture;
        public Sprite sliderIcons;

        public Text valueText;

        Rectangle sliderHitbox;

        float value;
        float minValue;
        float maxValue;

        float fadeConstant;

        Vector2 sliderTargetPos;

        int[] holdThreshold = { 30, 60, 120 };

        int holdTime;
        int incTick;
        Keys lastHeldKey;
        Buttons lastHeldButton;

        bool sliderDown;

        public void Init(ContentManager cm, float value, float minValue, float maxValue, string bgPath, string path, string t, Color bgColor, Color c, Vector2 targetPos, bool useOrigin, Vector2 hitboxOffset)
        {
            base.Init(cm, bgPath, path, t, bgColor, c, targetPos, useOrigin, hitboxOffset);

            sliderBackgroundTexture = new Sprite();
            sliderBlockTexture = new Sprite();
            valueText = new Text();
            backgroundTexture = new Sprite();
            sliderIcons = new Sprite();

            valueText.Init(cm, path, value.ToString(), Color.White, Vector2.Zero, true);
            backgroundTexture.Init(cm, bgPath, Vector2.Zero, Color.White, 1.0f, true);
            sliderIcons.Init(cm, "OptionsMenu\\sliderBarIcons", Vector2.Zero, Color.White, 1.0f, true);
            sliderBackgroundTexture.Init(cm, "OptionsMenu\\sliderBar", Vector2.Zero, Color.White, 1.0f, true);
            sliderBlockTexture.Init(cm, "OptionsMenu\\sliderBlock", Vector2.Zero, Color.White, 1.0f, true);

            this.value = value;
            this.minValue = minValue;
            this.maxValue = maxValue;

            sliderTargetPos = Vector2.Zero;

            holdTime = 0;
            incTick = 0;
            lastHeldKey = Keys.None;
            lastHeldButton = Buttons.B;

            fadeConstant = 0.1f;

            sliderHitbox = new Rectangle(0, 0, sliderBackgroundTexture.GetTexture.Width, sliderBackgroundTexture.GetTexture.Height);

            SetPos(startingPos);
            SetAlpha(0.0f);
        }

        public override void Update()
        {
            if (Vector2.Distance(sliderBackgroundTexture.Pos, targetPos) <= 50.0f)
            {
                if (hitbox.Contains(Input.GetMousePositionPoint))
                {
                    backgroundTexture.Alpha = MathHelper.Lerp(backgroundTexture.Alpha, 1.0f, 0.1f);
                    selected = true;
                }
                else
                {
                    backgroundTexture.Alpha = MathHelper.Lerp(backgroundTexture.Alpha, 0.0f, 0.1f);
                    selected = false;
                }
            }
            else
                selected = false;

            if (selected)
            {
                if (Input.IsMouse1Clicked)
                {
                    if (hitbox.Contains(Input.GetMousePositionPoint))
                        sliderDown = true;
                }
            }
            else
                if (!(sliderHitbox.Top < Input.GetMousePositionPoint.Y && sliderHitbox.Bottom > Input.GetMousePositionPoint.Y))
                    sliderDown = false;

            if (sliderDown)
            {
                value = (float)Math.Round((Input.GetMousePositionVector2.X - hitbox.X) / hitbox.Width * 100);
                UpdateSlider();
            }

            if (Input.IsMouse1Released)
            {
                if (sliderDown)
                    Released();

                sliderDown = false;
            }

            Fade();
        }

        public override void Update(bool isSelected)
        {
            if (Vector2.Distance(sliderBackgroundTexture.Pos, targetPos) <= 50.0f)
            {
                selected = isSelected;

                if (isSelected)
                {
                    backgroundTexture.Alpha = MathHelper.Lerp(backgroundTexture.Alpha, 1.0f, 0.1f);

                    if (!Input.IsGamePadConnected)
                    {
                        if (Input.IsKeyDown(Keys.Left))
                        {
                            if (lastHeldKey != Keys.Left)
                            {
                                incTick = 0;
                                holdTime = 0;
                                lastHeldKey = Keys.Left;
                            }
                            else
                                holdTime++;

                            if (holdTime >= holdThreshold[2])
                            {
                                if (incTick % 10 == 0)
                                    value -= 10;
                            }
                            else if (holdTime >= holdThreshold[1])
                            {
                                if (incTick % 10 == 0)
                                    value -= 5;
                            }
                            else if (holdTime >= holdThreshold[0])
                            {
                                if (incTick % 10 == 0)
                                    value -= 2;
                            }
                            else
                                if (incTick == 0)
                                    value--;

                            incTick++;
                        }
                        else if (Input.IsKeyDown(Keys.Right))
                        {
                            if (lastHeldKey != Keys.Right)
                            {
                                holdTime = 0;
                                incTick = 0;
                                lastHeldKey = Keys.Right;
                            }
                            else
                                holdTime++;

                            if (holdTime >= holdThreshold[2])
                            {
                                if (incTick % 10 == 0)
                                    value += 10;
                            }
                            else if (holdTime >= holdThreshold[1])
                            {
                                if (incTick % 10 == 0)
                                    value += 5;
                            }
                            else if (holdTime >= holdThreshold[0])
                            {
                                if (incTick % 10 == 0)
                                    value += 2;
                            }
                            else
                                if (incTick == 0)
                                    value++;

                            incTick++;
                        }
                        else
                        {
                            lastHeldKey = Keys.None;
                        }
                    }
                    else
                    {
                        if (Input.IsGamePadButtonDown(Buttons.LeftThumbstickLeft))
                        {
                            if (lastHeldButton != Buttons.LeftThumbstickLeft)
                            {
                                incTick = 0;
                                holdTime = 0;
                                lastHeldButton = Buttons.LeftThumbstickLeft;
                            }
                            else
                                holdTime++;

                            if (holdTime >= holdThreshold[2])
                            {
                                if (incTick % 10 == 0)
                                    value -= 10;
                            }
                            else if (holdTime >= holdThreshold[1])
                            {
                                if (incTick % 10 == 0)
                                    value -= 5;
                            }
                            else if (holdTime >= holdThreshold[0])
                            {
                                if (incTick % 10 == 0)
                                    value -= 2;
                            }
                            else
                                if (incTick == 0)
                                    value--;

                            incTick++;
                        }
                        else if (Input.IsGamePadButtonDown(Buttons.LeftThumbstickRight))
                        {
                            if (lastHeldButton != Buttons.LeftThumbstickRight)
                            {
                                holdTime = 0;
                                incTick = 0;
                                lastHeldButton = Buttons.LeftThumbstickRight;
                            }
                            else
                                holdTime++;

                            if (holdTime >= holdThreshold[2])
                            {
                                if (incTick % 10 == 0)
                                    value += 10;
                            }
                            else if (holdTime >= holdThreshold[1])
                            {
                                if (incTick % 10 == 0)
                                    value += 5;
                            }
                            else if (holdTime >= holdThreshold[0])
                            {
                                if (incTick % 10 == 0)
                                    value += 2;
                            }
                            else
                                if (incTick == 0)
                                    value++;

                            incTick++;
                        }
                        else
                        {
                            lastHeldButton = Buttons.B;
                        }
                    }
                }
                else
                    backgroundTexture.Alpha = MathHelper.Lerp(backgroundTexture.Alpha, 0.0f, 0.1f);
            }

            UpdateSlider();
            Fade();
        }

        protected override void Fade()
        {
            switch (currentState)
            {
                case State.FadeIn:
                    SetPos(new Vector2(MathHelper.Lerp(sliderBackgroundTexture.Pos.X, targetPos.X, fadeConstant), sliderBackgroundTexture.Pos.Y));
                    SetAlpha(1 - (targetPos.X - sliderBackgroundTexture.Pos.X) / (targetPos.X - startingPos.X));
                    sliderBackgroundTexture.Alpha = MathHelper.Max(0.25f, (1 - (targetPos.X - sliderBackgroundTexture.Pos.X) / (targetPos.X - startingPos.X)) * (value / maxValue));

                    if (Vector2.Distance(sliderBackgroundTexture.Pos, targetPos) <= 0.5f)
                    {
                        SetPos(targetPos);
                        currentState = State.None;
                        fadeConstant = 0.1f;
                    }
                    else if (Vector2.Distance(sliderBackgroundTexture.Pos, targetPos) <= 5.0f)
                        fadeConstant += 0.1f;
                    break;
                case State.FadeOut:
                    SetPos(new Vector2(MathHelper.Lerp(sliderBackgroundTexture.Pos.X, startingPos.X, 0.1f), sliderBackgroundTexture.Pos.Y));
                    SetAlpha(1 - (targetPos.X - sliderBackgroundTexture.Pos.X) / (targetPos.X - startingPos.X));
                    sliderBackgroundTexture.Alpha = (1 - (targetPos.X - sliderBackgroundTexture.Pos.X) / (targetPos.X - startingPos.X)) * (value / maxValue);

                    if (text.Alpha < 0.0f)
                        text.Alpha = 0.0f;

                    backgroundTexture.Alpha = 0.0f;
                    break;
            }
        }

        protected void UpdateSlider()
        {
            value = MathHelper.Clamp(value, minValue, maxValue);

            sliderTargetPos = new Vector2(sliderBackgroundTexture.Pos.X + (value / maxValue) * (sliderBackgroundTexture.GetTexture.Width) - sliderBackgroundTexture.GetTexture.Width / 2.0f, sliderBlockTexture.Pos.Y);

            sliderBlockTexture.Pos = new Vector2(MathHelper.Lerp(sliderBlockTexture.Pos.X, sliderTargetPos.X, 1.0f), sliderTargetPos.Y);
            valueText.TextString = value.ToString();
            sliderHitbox.Location = new Point((int)(sliderBlockTexture.Pos.X - sliderBlockTexture.GetTexture.Width / 2.0f), (int)(sliderBlockTexture.Pos.Y - sliderBlockTexture.GetTexture.Height / 2.0f));

            sliderBackgroundTexture.Alpha = MathHelper.Max(0.25f, value / maxValue);
        }

        protected void UpdatelSiderLerp()
        {
            value = MathHelper.Clamp(value, minValue, maxValue);

            sliderTargetPos = new Vector2(sliderBackgroundTexture.Pos.X + (value / maxValue) * (sliderBackgroundTexture.GetTexture.Width) - sliderBackgroundTexture.GetTexture.Width / 2.0f, sliderBlockTexture.Pos.Y);

            sliderBlockTexture.Pos = new Vector2(MathHelper.Lerp(sliderBlockTexture.Pos.X, sliderTargetPos.X, 0.1f), sliderTargetPos.Y);
            valueText.TextString = value.ToString();

            sliderBackgroundTexture.Alpha = MathHelper.Max(0.25f, value / maxValue);
        }

        protected override void SetPos(Vector2 position)
        {
            sliderBackgroundTexture.Pos = position;
            sliderBlockTexture.Pos = new Vector2(position.X + (value / maxValue) * (sliderBackgroundTexture.GetTexture.Width) - sliderBackgroundTexture.GetTexture.Width / 2.0f, position.Y);
            text.Pos = position - new Vector2(sliderBackgroundTexture.GetTexture.Width / 2.0f + text.Size.X / 2.0f + 15, 0.0f);
            valueText.Pos = position + new Vector2(sliderBackgroundTexture.GetTexture.Width / 2.0f + valueText.Size.X / 2.0f, 0.0f);
            backgroundTexture.Pos = text.Pos;

            hitbox = new Rectangle((int)(sliderBackgroundTexture.Pos.X - sliderBackgroundTexture.GetTexture.Width / 2.0f), (int)(sliderBackgroundTexture.Pos.Y - sliderBackgroundTexture.GetTexture.Height / 2.0f), sliderBackgroundTexture.GetTexture.Width, sliderBackgroundTexture.GetTexture.Height);
            sliderHitbox.Location = new Point((int)(sliderBackgroundTexture.Pos.X - sliderBackgroundTexture.GetTexture.Width / 2.0f), (int)(sliderBackgroundTexture.Pos.Y - sliderBackgroundTexture.GetTexture.Height / 2.0f));

            sliderIcons.Pos = position;
        }

        protected void SetAlpha(float alpha)
        {
            text.Alpha = alpha;
            sliderBackgroundTexture.Alpha = alpha;
            sliderBlockTexture.Alpha = alpha;
            sliderIcons.Alpha = alpha;
        }

        public override void Draw(SpriteBatch sb)
        {
            backgroundTexture.Draw(sb);
            sliderBackgroundTexture.Draw(sb);
            sliderBlockTexture.Draw(sb);
            text.Draw(sb);
            sliderIcons.Draw(sb);

            //if (selected)
            //valueText.Draw(sb);
        }

        public float GetValue { get { return value; } }
    }
}
