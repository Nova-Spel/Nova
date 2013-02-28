using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Nova_Alpha
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Screen currentScreen;
        MainMenu mainMenu;
        OptionsMenu optionsMenu;
        GameScreen inGame;
        PauseMenu pauseMenu;

        Stack<Screen> backScreens;

        Dictionary<string, Screen> screens;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            //graphics.IsFullScreen = true;

            //graphics.IsFullScreen = true;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";

            screens = new Dictionary<string, Screen>();
            mainMenu = new MainMenu();
            optionsMenu = new OptionsMenu();
            inGame = new GameScreen();
            pauseMenu = new PauseMenu();

            backScreens = new Stack<Screen>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Input.Initialize();
            base.Initialize();

            mainMenu.Initialize();
            optionsMenu.Initialize();
            inGame.Initialize();
            pauseMenu.Initialize();

            screens.Add("MainMenu", mainMenu);
            screens.Add("Options", optionsMenu);
            screens.Add("Game", inGame);
            screens.Add("PauseMenu", pauseMenu);
            currentScreen = mainMenu;
            currentScreen.Reset();
            mainMenu.OnChangeScreen += ChangeScreen;
            mainMenu.OnExitGame += ExitGame;

            currentScreen.OnClearLastScreens += ClearLastScreens;
            currentScreen.OnGoBack += GoToLastScreen;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            mainMenu.LoadContent(Content);
            optionsMenu.LoadContent(Content);
            inGame.LoadContent(Content);
            pauseMenu.LoadContent(Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit

            Input.Update();
            currentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            currentScreen.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private void ChangeScreen(object sender, StringEventArgs args)
        {
            if (!screens.ContainsKey(args.value))
                throw new Exception("Screen does not contain \"" + args.value + "\"");

            backScreens.Push(currentScreen);

            currentScreen.OnChangeScreen -= ChangeScreen;
            currentScreen.OnExitGame -= ExitGame;

            currentScreen.OnClearLastScreens -= ClearLastScreens;
            currentScreen.OnGoBack -= GoToLastScreen;

            currentScreen = screens[args.value];

            currentScreen.OnChangeScreen += ChangeScreen;
            currentScreen.OnExitGame += ExitGame;

            currentScreen.OnClearLastScreens += ClearLastScreens;
            currentScreen.OnGoBack += GoToLastScreen;

            currentScreen.Reset();
        }

        private void GoToLastScreen(object sender, EventArgs args)
        {
            currentScreen.OnClearLastScreens -= ClearLastScreens;
            currentScreen.OnGoBack -= GoToLastScreen;

            currentScreen.OnChangeScreen -= ChangeScreen;
            currentScreen.OnExitGame -= ExitGame;

            currentScreen = backScreens.Pop();

            currentScreen.OnChangeScreen += ChangeScreen;
            currentScreen.OnExitGame += ExitGame;

            currentScreen.OnClearLastScreens += ClearLastScreens;
            currentScreen.OnGoBack += GoToLastScreen;
            currentScreen.Reset();
        }

        private void ClearLastScreens(object sender, EventArgs args)
        {
            backScreens.Clear();
        }

        private void ExitGame(object sender, EventArgs args)
        {
            this.Exit();
        }
    }
}
