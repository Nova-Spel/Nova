using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.IO;

namespace Nova_Alpha
{
    static class Input
    {
        static KeyboardState keyState;
        static KeyboardState oldKeyState;
        static MouseState mouseState;
        static MouseState oldMouseState;
        static GamePadState gamePadState;
        static GamePadState oldGamePadState;

        static bool gamePad;

        /// <summary>
        /// Initializes all variables
        /// </summary>
        public static void Initialize()
        {
            keyState = Keyboard.GetState();
            oldKeyState = keyState;

            mouseState = Mouse.GetState();
            oldMouseState = mouseState;

            gamePadState = GamePad.GetState(PlayerIndex.One);
            oldGamePadState = gamePadState;
            gamePad = gamePadState.IsConnected;
        }

        /// <summary>
        /// Updates all input
        /// </summary>
        public static void Update()
        {
            oldKeyState = keyState;
            oldMouseState = mouseState;

            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            oldGamePadState = gamePadState;
            gamePadState = GamePad.GetState(PlayerIndex.One);
            
        }

        /// <summary>
        /// ChecInput whether or not Mouse1 is down during the current frame
        /// </summary>
        /// <returns></returns>
        public static bool IsMouse1Down
        {
            get { return mouseState.LeftButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// ChecInput whether or not Mouse1 is down during the current frame and released in the frame before
        /// </summary>
        /// <returns></returns>
        public static bool IsMouse1Clicked
        {
            get { return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released; }
        }

        /// <summary>
        /// ChecInput whether or not Mouse1 was just realeased
        /// </summary>
        public static bool IsMouse1Released
        {
            get { return mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed; }
        }

        public static bool IsMouse2Released
        {
            get { return mouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// ChecInput whether or not Mouse2 is down during the current frame
        /// </summary>
        /// <returns></returns>
        public static bool IsMouse2Down
        {
            get { return mouseState.RightButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// ChecInput whether or not Mouse2 is down during the current frame and released in the frame before
        /// </summary>
        /// <returns></returns>
        public static bool IsMouse2Clicked
        {
            get { return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released; }
        }

        /// <summary>
        /// ChecInput whether or not the given key is down during the current frame
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsKeyDown(Keys key)
        {
            return keyState.IsKeyDown(key);
        }

        /// <summary>
        /// ChecInput whether or not the given key is down during the current frame and released in the frame before
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsKeyClicked(Keys key)
        {
            return keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
        }

        /// <summary>
        /// Checks if any key is down
        /// </summary>
        /// <returns></returns>
        public static bool IsAnyKeyDown()
        {
            return keyState.GetPressedKeys().Length != 0;
        }

        /// <summary>
        /// Checks if any game pad button is down
        /// </summary>
        /// <returns></returns>
        public static bool IsAnyGamePadButtonDown()
        {
            foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
                if (gamePadState.IsButtonDown(button))
                    return true;

            return false;
        }

        /// <summary>
        /// Checks if the given game pad button is down
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool IsGamePadButtonDown(Buttons button)
        {
            return gamePadState.IsButtonDown(button);
        }

        public static bool WasGamePadButtonUp(Buttons button)
        {
            return oldGamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// Checks if the given game pad button was just clicked
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool IsGamePadButtonClicked(Buttons button)
        {
            return gamePadState.IsButtonDown(button) && !oldGamePadState.IsButtonDown(button);
        }

        /// <summary>
        /// Gets the value of the left thumbstick
        /// </summary>
        public static Vector2 GetLeftThumbstickValue
        {
            get { return gamePadState.ThumbSticks.Left; }
        }

        public static Vector2 GetOldLeftThumbstickValue
        {
            get { return oldGamePadState.ThumbSticks.Left; }
        }

        /// <summary>
        /// Gets the value of the right thumbstick
        /// </summary>
        public static Vector2 GetRightThumbstickValue
        {
            get { return gamePadState.ThumbSticks.Right; }
        }

        public static Vector2 GetOldRightThumbstickValue
        {
            get { return oldGamePadState.ThumbSticks.Right; }
        }


        /// <summary>
        /// Returns the mouse position as a Vector2
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetMousePositionVector2
        {
            get { return new Vector2(mouseState.X, mouseState.Y); }
        }

        /// <summary>
        /// Returns the mouse position as a Point
        /// </summary>
        /// <returns></returns>
        public static Point GetMousePositionPoint
        {
            get { return new Point(mouseState.X, mouseState.Y); }
        }

        /// <summary>
        /// Returns the mouse position as a Point
        /// </summary>
        /// <returns></returns>
        public static Point GetOldMousePositionPoint
        {
            get { return new Point(oldMouseState.X, oldMouseState.Y); }
        }

        /// <summary>
        /// Returns the amount of pixels the mouse cursor has moved since the last frame
        /// </summary>
        public static Point GetMousePoisitionDifference
        {
            get { return new Point(mouseState.X - oldMouseState.X, mouseState.Y - oldMouseState.Y); }
        }

        /// <summary>
        /// Returns how many mousewheel-ticInput have passed since the last frame
        /// </summary>
        /// <returns></returns>
        public static int GetMouseWheelTicInput
        {
            get { return mouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue; }
        }

        /// <summary>
        /// Returns all currently pressed keys
        /// </summary>
        /// <returns></returns>
        public static Keys[] GetAllPressedKeys()
        {
            return keyState.GetPressedKeys();
        }

        /// <summary>
        /// Returns all clicked keys
        /// </summary>
        /// <returns></returns>
        public static Keys[] GetAllClickedKeys()
        {
            List<Keys> clickedKeys = keyState.GetPressedKeys().ToList();

            for (int i = 0; i < clickedKeys.Count; i++)
            {
                if (!Input.IsKeyClicked(clickedKeys[i]))
                {
                    clickedKeys.RemoveAt(i);
                    i--;
                }
            }

            return clickedKeys.ToArray();
        }

        public static int GetPressedNumericKey()
        {
            Keys[] pressedKeys = GetAllPressedKeys();

            for (int i = 0; i < pressedKeys.Length; i++)
            {
                switch (pressedKeys[i])
                {
                    case Keys.NumPad0:
                    case Keys.D0:
                        return 0;
                    case Keys.NumPad1:
                    case Keys.D1:
                        return 1;
                    case Keys.NumPad2:
                    case Keys.D2:
                        return 2;
                    case Keys.NumPad3:
                    case Keys.D3:
                        return 3;
                    case Keys.NumPad4:
                    case Keys.D4:
                        return 4;
                    case Keys.NumPad5:
                    case Keys.D5:
                        return 5;
                    case Keys.NumPad6:
                    case Keys.D6:
                        return 6;
                    case Keys.NumPad7:
                    case Keys.D7:
                        return 7;
                    case Keys.NumPad8:
                    case Keys.D8:
                        return 8;
                    case Keys.NumPad9:
                    case Keys.D9:
                        return 9;
                }
            }

            return -1;
        }

        public static int GetClickedNumericKey()
        {
            Keys[] pressedKeys = GetAllClickedKeys();

            for (int i = 0; i < pressedKeys.Length; i++)
            {
                switch (pressedKeys[i])
                {
                    case Keys.NumPad0:
                    case Keys.D0:
                        return 0;
                    case Keys.NumPad1:
                    case Keys.D1:
                        return 1;
                    case Keys.NumPad2:
                    case Keys.D2:
                        return 2;
                    case Keys.NumPad3:
                    case Keys.D3:
                        return 3;
                    case Keys.NumPad4:
                    case Keys.D4:
                        return 4;
                    case Keys.NumPad5:
                    case Keys.D5:
                        return 5;
                    case Keys.NumPad6:
                    case Keys.D6:
                        return 6;
                    case Keys.NumPad7:
                    case Keys.D7:
                        return 7;
                    case Keys.NumPad8:
                    case Keys.D8:
                        return 8;
                    case Keys.NumPad9:
                    case Keys.D9:
                        return 9;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns a string corresponding to which buttonList the user is pressing
        /// </summary>
        /// <returns></returns>
        public static string GetTextInput()
        {
            List<Keys> pressedKeys = Input.GetAllPressedKeys().ToList();

            bool upperCase = false;

            for (int i = 0; i < pressedKeys.Count; i++)
            {
                Keys key = pressedKeys[i];

                if (Input.IsKeyClicked(key))
                {
                    if (KeyToString(key).Length != 1)
                    {
                        pressedKeys.RemoveAt(i);
                        i--;
                    }
                }
                else
                {
                    pressedKeys.RemoveAt(i);
                    i--;
                }
            }

            foreach (Keys key in pressedKeys)
                return upperCase ? KeyToString(key).ToUpper() : KeyToString(key).ToLower();

            return string.Empty;
        }

        /// <summary>
        /// Converts a key to a string
        /// </summary>
        /// <param name="key">Key to convert</param>
        /// <returns></returns>
        private static string KeyToString(Keys key)
        {
            if (key.ToString().Length == 1) //is it a letter?
                return key.ToString();

            switch (key)
            {
                case Keys.NumPad0:
                case Keys.D0:
                    return "0";
                case Keys.NumPad1:
                case Keys.D1:
                    return "1";
                case Keys.NumPad2:
                case Keys.D2:
                    return "2";
                case Keys.NumPad3:
                case Keys.D3:
                    return "3";
                case Keys.NumPad4:
                case Keys.D4:
                    return "4";
                case Keys.NumPad5:
                case Keys.D5:
                    return "5";
                case Keys.NumPad6:
                case Keys.D6:
                    return "6";
                case Keys.NumPad7:
                case Keys.D7:
                    return "7";
                case Keys.NumPad8:
                case Keys.D8:
                    return "8";
                case Keys.NumPad9:
                case Keys.D9:
                    return "9";
                default:
                    return string.Empty;
            }
        }

        public static void GenerateDefaultSettingsFile()
        {
            File.WriteAllLines("Content\\Settings",
                new string[] {
                    "mastervolume=100",
                    "musicvolume=50",
                    "fxvolume=50"
                });
        }

        public static void ReadVolumeValues(out float masterVolume, out float musicVolume, out float fxVolume)
        {
            string[] lines = File.ReadAllLines("Content\\Settings");

            masterVolume = 0.0f;
            musicVolume = 0.0f;
            fxVolume = 0.0f;

            foreach (string line in lines)
            {
                if (line.Substring(0, line.IndexOf('=')) == "mastervolume")
                {
                    if (!float.TryParse(line.Substring(line.IndexOf('=') + 1), out masterVolume))
                    {
                        GenerateDefaultSettingsFile();
                        ReadVolumeValues(out masterVolume, out musicVolume, out fxVolume);
                        break;
                    }
                }
                if (line.Substring(0, line.IndexOf('=')) == "musicvolume")
                {
                    if (!float.TryParse(line.Substring(line.IndexOf('=') + 1), out musicVolume))
                    {
                        GenerateDefaultSettingsFile();
                        ReadVolumeValues(out masterVolume, out musicVolume, out fxVolume);
                        break;
                    }
                }
                if (line.Substring(0, line.IndexOf('=')) == "fxvolume")
                {
                    if (!float.TryParse(line.Substring(line.IndexOf('=') + 1), out fxVolume))
                    {
                        GenerateDefaultSettingsFile();
                        ReadVolumeValues(out masterVolume, out musicVolume, out fxVolume);
                        break;
                    }
                }
            }
        }

        public static bool IsGamePadConnected
        {
            get { return Input.gamePad; }
            set { Input.gamePad = value; }
        }
    }
}
