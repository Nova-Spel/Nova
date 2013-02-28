using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nova_Alpha
{
    abstract class MenuScreen : Screen
    {
        protected InputManager inputManager;

        //protected string buttonPressed;

        protected enum State { None, FadeIn, FadeOut };
        protected State currentState;

        protected ActionButton buttonPressed;

        protected Sprite background;

        public MenuScreen()
        {
            inputManager = new InputManager();

            inputManager.OnButtonPressed += ButtonPressed;
        }

        public override void Update(GameTime gameTime)
        {
            if (currentState == State.FadeOut)
                foreach(Button button in inputManager.GetButtons)
                    if(button.text.Alpha == 0.0f)
                        HandleButtonPress();
        }

        protected virtual void FadeOut()
        {
            foreach (Button button in inputManager.GetButtons)
                button.FadeOut();

            currentState = State.FadeOut;
        }

        public override void Reset()
        {
            currentState = State.FadeIn;
        }

        protected void ButtonPressed(object sender, EventArgs args)
        {
            if (sender is ActionButton)
            {
                ActionButton button = (ActionButton)sender;

                if (button != null)
                    buttonPressed = button;

                FadeOut();
            }
        }

        protected abstract void HandleButtonPress();
    }
}
