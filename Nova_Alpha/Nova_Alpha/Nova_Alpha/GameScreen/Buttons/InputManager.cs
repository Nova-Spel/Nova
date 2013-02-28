using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Nova_Alpha
{
    class InputManager
    {
        enum InputDevice { Mouse, Keyboard, GamePad };
        InputDevice inputDevice;

        int selectedButton;

        Button[] buttons;

        public event EventHandler OnButtonPressed;

        protected SoundEffect buttonSoundEffect;

        public InputManager()
        {
            inputDevice = InputDevice.Mouse;
        }

        public void Init(ContentManager content)
        {
            buttonSoundEffect = content.Load<SoundEffect>("Sounds\\Click");
        }

        public void Update()
        {
            //if (inputDevice != InputDevice.Mouse)
            //{
                if (Input.GetMousePoisitionDifference != Point.Zero || Input.IsMouse1Down || Input.IsMouse2Down)
                    inputDevice = InputDevice.Mouse;
            //}
            //else if (inputDevice != InputDevice.Keyboard)
            //{
                if (Input.IsAnyKeyDown())
                    inputDevice = InputDevice.Keyboard;
            //}
            //else if (inputDevice != InputDevice.GamePad)
            //{
                if (Input.IsAnyGamePadButtonDown())
                    inputDevice = InputDevice.GamePad;
            //}


            switch (inputDevice)
            {
                case InputDevice.Mouse:
                    bool anySelected = false;

                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i].Update();

                        if (buttons[i].IsSelected)
                        {
                            anySelected = true;

                            if (selectedButton != i)
                                buttonSoundEffect.Play(Screen.GetFxValue, 0.0f, 0.0f);

                            selectedButton = i;
                        }
                    }

                    if (!anySelected)
                        selectedButton = -1;
                    break;
                case InputDevice.Keyboard:
                    for (int i = 0; i < buttons.Length; i++)
                        buttons[i].Update(i == selectedButton);

                    if (Input.IsKeyClicked(Keys.Down) || Input.IsKeyClicked(Keys.W))
                    {
                        selectedButton++;

                        buttonSoundEffect.Play(Screen.GetFxValue, 0.0f, 0.0f);

                        if (selectedButton >= buttons.Length)
                            selectedButton = 0;
                    }
                    else if (Input.IsKeyClicked(Keys.Up) || Input.IsKeyClicked(Keys.W))
                    {
                        selectedButton--;

                        buttonSoundEffect.Play(Screen.GetFxValue, 0.0f, 0.0f);

                        if (selectedButton < 0)
                            selectedButton = buttons.Length - 1;
                    }
                    break;
                case InputDevice.GamePad:
                    for (int i = 0; i < buttons.Length; i++)
                        buttons[i].Update(i == selectedButton);

                    if (Input.IsGamePadButtonClicked(Buttons.DPadDown) || Input.IsGamePadButtonClicked(Buttons.LeftThumbstickDown))
                    {
                        selectedButton++;

                        buttonSoundEffect.Play(Screen.GetFxValue, 0.0f, 0.0f);

                        if (selectedButton >= buttons.Length)
                            selectedButton = 0;
                    }
                    else if (Input.IsGamePadButtonClicked(Buttons.DPadUp) || Input.IsGamePadButtonClicked(Buttons.LeftThumbstickUp))
                    {
                        selectedButton--;

                        buttonSoundEffect.Play(Screen.GetFxValue, 0.0f, 0.0f);

                        if (selectedButton < 0)
                            selectedButton = buttons.Length - 1;
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in buttons)
                button.Draw(spriteBatch);
        }

        private void ButtonPressed(object sender, EventArgs args)
        {
            if (sender is ActionButton)
            {
                foreach (Button button in buttons)
                    button.FadeOut();

                if (OnButtonPressed != null)
                    OnButtonPressed(sender, args);
            }
        }

        public void SetButtons(Button[] buttons)
        {
            this.buttons = buttons;

            foreach (Button button in buttons)
                button.OnClicked += ButtonPressed;
        }

        public Button[] GetButtons { get { return buttons; } }
    }
}
