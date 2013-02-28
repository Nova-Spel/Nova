using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Nova_Alpha
{
    class PauseMenu : MenuScreen
    {
        ActionButton resumeGameButton;
        ActionButton optionsButton;
        ActionButton exitButton;
        Sprite logo;

        public PauseMenu()
        {
            background = new Sprite();

            resumeGameButton = new ActionButton();
            optionsButton = new ActionButton();
            exitButton = new ActionButton();

            logo = new Sprite();

            inputManager.SetButtons(new ActionButton[] { resumeGameButton, optionsButton, exitButton });
            currentState = State.None;
        }

        public override void LoadContent(ContentManager content)
        {
            background.Init(content, "MainMenu\\novamenubg", Vector2.Zero, Color.White, 1.0f, false);

            resumeGameButton.Init(content, "MainMenu\\startbg", "Shanti", "Resume", Color.DodgerBlue, Color.White, new Vector2(640, 500), true, new Vector2(0.0f, -10.0f));
            optionsButton.Init(content, "MainMenu\\optionsbg", "Shanti", "Options", Color.YellowGreen, Color.White, new Vector2(640, 560), true, new Vector2(0.0f, -10.0f));
            exitButton.Init(content, "MainMenu\\exitbg", "Shanti", "Exit", Color.DarkRed, Color.White, new Vector2(640, 620), true, new Vector2(0.0f, -10.0f));

            backgroundMusic = content.Load<Song>("MainMenu\\Music\\Background");
            MediaPlayer.Volume = GetMusicVolume;
            MediaPlayer.Play(backgroundMusic);
            inputManager.Init(content);

            logo.Init(content, "MainMenu\\logo", new Vector2(600, 200), Color.White, 1.0f, true);

            base.LoadContent(content);
        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();

            base.Update(gameTime);

            if (currentState != State.None)
                logo.Alpha = resumeGameButton.text.Alpha;

            if (Input.IsKeyClicked(Keys.Escape))
                FadeOut();

            if (Input.IsGamePadConnected)
                if (Input.IsGamePadButtonClicked(Buttons.Back) || Input.IsGamePadButtonClicked(Buttons.B))
                    FadeOut();
                    
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            background.Draw(spriteBatch);

            logo.Draw(spriteBatch);

            resumeGameButton.Draw(spriteBatch);
            optionsButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Reset()
        {
            resumeGameButton.FadeIn();
            optionsButton.FadeIn();
            exitButton.FadeIn();

            buttonPressed = null;
        }

        protected override void HandleButtonPress()
        {
            if (buttonPressed == optionsButton)
                ChangeScreen(new StringEventArgs("Options"));
            else if (buttonPressed == exitButton)
                ExitGame();
            else if (buttonPressed == resumeGameButton)
                ChangeScreen(new StringEventArgs("Game"));
            else //Back/other back button pressed
            {
                ClearLastScreens();
                ChangeScreen(new StringEventArgs("Game"));
            }
        }
    }
}
