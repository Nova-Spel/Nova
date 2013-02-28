using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Nova_Alpha
{
    class OptionsMenu : MenuScreen
    {
        ActionButton backButton;

        Slider masterVolumeSlider;
        Slider fxVolumeSlider;
        Slider musicVolumeSlider;

        SoundEffect fxSoundEffect;

        public OptionsMenu()
        {
            background = new Sprite();

            backButton = new ActionButton();
            masterVolumeSlider = new Slider();
            fxVolumeSlider = new Slider();
            musicVolumeSlider = new Slider();

            inputManager.SetButtons(new Button[] { backButton, masterVolumeSlider, fxVolumeSlider, musicVolumeSlider });

            currentState = State.None;
        }

        public override void LoadContent(ContentManager content)
        {
            background.Init(content, "MainMenu\\novamenubg", Vector2.Zero, Color.White, 1.0f, false);

            backButton.Init(content, "MainMenu\\startbg", "Shanti", "Back", Color.DarkRed, Color.White, new Vector2(640.0f, 640), true, Vector2.Zero);

            masterVolumeSlider.Init(content, GetRawMasterVolume, 0.0f, 100.0f, "OptionsMenu\\masterbg", "Shanti", "Master:", Color.White, Color.White, new Vector2(640.0f, 200.0f), true, Vector2.Zero);
            fxVolumeSlider.Init(content, GetRawFxValue, 0.0f, 100.0f, "OptionsMenu\\fxbg", "Shanti", "Sound FX:", Color.White, Color.White, new Vector2(640.0f, 260.0f), true, Vector2.Zero);
            musicVolumeSlider.Init(content, GetRawMusicVolume, 0.0f, 100.0f, "OptionsMenu\\musicbg", "Shanti", "Music:", Color.White, Color.White, new Vector2(640.0f, 320.0f), true, Vector2.Zero);

            fxSoundEffect = content.Load<SoundEffect>("Sounds\\Click");

            inputManager.Init(content);

            base.LoadContent(content);
        }

        public override void Initialize()
        {
            fxVolumeSlider.OnReleased += FxSliderReleased;
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();

            if (Input.IsKeyClicked(Keys.Escape))
                FadeOut();

            if (Input.IsGamePadConnected)
                if (Input.IsGamePadButtonClicked(Buttons.Back) || Input.IsGamePadButtonClicked(Buttons.B))
                    FadeOut();

            MasterVolume = masterVolumeSlider.GetValue;
            MusicVolume = musicVolumeSlider.GetValue;
            FxVolume = fxVolumeSlider.GetValue;

            MediaPlayer.Volume = (masterVolumeSlider.GetValue / 100.0f) * (musicVolumeSlider.GetValue / 100.0f);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            background.Draw(spriteBatch);
            inputManager.Draw(spriteBatch);

            spriteBatch.End();
        }

        protected override void HandleButtonPress()
        {
            SaveData();
            GoBack();
        }

        private void SaveData()
        {
            File.WriteAllLines("Content\\Settings",
                new string[] {
                    "mastervolume=" + masterVolumeSlider.GetValue,
                    "musicvolume=" + musicVolumeSlider.GetValue,
                    "fxvolume=" + fxVolumeSlider.GetValue
                });
        }

        public override void Reset()
        {
            backButton.FadeIn();
            masterVolumeSlider.FadeIn();
            fxVolumeSlider.FadeIn();
            musicVolumeSlider.FadeIn();

            currentState = State.FadeIn;

            buttonPressed = null;
        }

        private void FxSliderReleased(object sender, EventArgs args)
        {
            fxSoundEffect.Play(GetFxValue, 0.0f, 0.0f);
        }
    }
}
