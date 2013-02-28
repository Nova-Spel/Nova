using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Nova_Alpha
{
    abstract class Screen
    {
        public event EventHandler<StringEventArgs> OnChangeScreen;
        public event EventHandler OnGoBack;
        public event EventHandler OnClearLastScreens;
        public event EventHandler OnExitGame;

        private static float masterVolumeValue;
        private static float musicVolumeValue;
        private static float fxVolumeValue;

        protected Song backgroundMusic;

        protected SoundEffect changeScreenSoundEffect;

        public Screen()
        {
            if (!File.Exists("Content\\Settings"))
                Input.GenerateDefaultSettingsFile();

            Input.ReadVolumeValues(out masterVolumeValue, out musicVolumeValue, out fxVolumeValue);
        }

        public abstract void Initialize();
        public virtual void LoadContent(ContentManager content)
        {
            changeScreenSoundEffect = content.Load<SoundEffect>("Sounds\\ChangeScreen");
        }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Reset();

        public void ChangeScreen(StringEventArgs args)
        {
            if (OnChangeScreen != null)
            {
                OnChangeScreen(this, args);

                if (changeScreenSoundEffect != null)
                    changeScreenSoundEffect.Play(GetFxValue, 0.0f, 0.0f);
            }
        }

        public void GoBack()
        {
            if (OnGoBack != null)
                OnGoBack(this, EventArgs.Empty);
        }

        public void ClearLastScreens()
        {
            if (OnClearLastScreens != null)
                OnClearLastScreens(this, EventArgs.Empty);
        }

        public void ExitGame()
        {
            if (OnExitGame != null)
                OnExitGame(this, EventArgs.Empty);
        }

        public static float MasterVolume
        {
            set { masterVolumeValue = value; }
        }

        public static float GetMasterVolume
        {
            get { return masterVolumeValue / 100.0f; }
        }

        public static float GetRawMasterVolume
        {
            get { return masterVolumeValue; }
        }

        public static float MusicVolume
        {
            set { musicVolumeValue = value; }
        }

        public static float GetMusicVolume
        { 
            get { return (masterVolumeValue / 100.0f) * (musicVolumeValue / 100.0f); }
        }

        public static float GetRawMusicVolume
        {
            get { return musicVolumeValue; }
        }

        public static float FxVolume
        {
            set { fxVolumeValue = value; }
        }

        public static float GetFxValue
        {
            get { return (masterVolumeValue / 100.0f) * (fxVolumeValue / 100.0f); }
        }

        public static float GetRawFxValue
        {
            get { return fxVolumeValue; }
        }
    }
}
