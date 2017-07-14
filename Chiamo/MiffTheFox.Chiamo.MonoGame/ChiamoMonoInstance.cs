using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CGame = MiffTheFox.Chiamo.Game;

namespace MiffTheFox.Chiamo.MonoGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ChiamoMonoInstance : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _Graphics;
        private SpriteBatch _SpriteBatch;
        private readonly CGame _Game;
        private XFontManager _FontManager;

        //public GraphicsDevice ChiamoGraphicsDevice => _Graphics.GraphicsDevice;

        public ChiamoMonoInstance(CGame chiamoGame)
        {
            _Game = chiamoGame;
            _Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            TargetElapsedTime = System.TimeSpan.FromMilliseconds(_Game.TargetTimerSpeed);
            Window.Title = _Game.Title;

            _Graphics.PreferredBackBufferHeight = _Game.Height;
            _Graphics.PreferredBackBufferWidth = _Game.Width;
            _Graphics.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _SpriteBatch = new SpriteBatch(GraphicsDevice);

            _Game.Sprites = new XSpriteManager(this);
            _Game.Fonts = _FontManager = new XFontManager(this);
            _Game.Initalize();

            _Game.SceneChanged += (sender, e) => _FontManager.ClearStringCache();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            _Game.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            var chInputState = new InputState();
            if (gamePadState.IsConnected)
            {
                _ProcessGamepad(gamePadState, chInputState);
            }

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) chInputState.JoyButton |= JoyButton.Up;
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) chInputState.JoyButton |= JoyButton.Left;
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) chInputState.JoyButton |= JoyButton.Down;
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) chInputState.JoyButton |= JoyButton.Right;
            if (keyboardState.IsKeyDown(Keys.Space)) chInputState.JoyButton |= JoyButton.Jump;
            if (keyboardState.IsKeyDown(Keys.Z)) chInputState.JoyButton |= JoyButton.Action1;
            if (keyboardState.IsKeyDown(Keys.X)) chInputState.JoyButton |= JoyButton.Action2;
            if (keyboardState.IsKeyDown(Keys.Escape)) chInputState.JoyButton |= JoyButton.Menu;

            if (mouseState.LeftButton == ButtonState.Pressed) chInputState.MouseButton |= MouseButton.Left;
            if (mouseState.RightButton == ButtonState.Pressed) chInputState.MouseButton |= MouseButton.Right;
            chInputState.MouseX = mouseState.X;
            chInputState.MouseY = mouseState.Y;

            _Game.Tick(new GameTickArgs { Input = chInputState });
            if (_Game.ExitRequested)
            {
                Exit();
            }

            _FontManager.Now = gameTime.ElapsedGameTime.Ticks;
            _FontManager.Cleanup();

            base.Update(gameTime);
        }

        private void _ProcessGamepad(GamePadState gamePadState, InputState chInputState)
        {
            const float STICK_DEADZONE = 0.3f;
            float lx = gamePadState.ThumbSticks.Left.X;
            float ly = gamePadState.ThumbSticks.Left.Y;

            if (lx > STICK_DEADZONE) chInputState.JoyButton |= JoyButton.Right;
            if (lx < -STICK_DEADZONE) chInputState.JoyButton |= JoyButton.Left;
            if (ly > STICK_DEADZONE) chInputState.JoyButton |= JoyButton.Down;
            if (ly < -STICK_DEADZONE) chInputState.JoyButton |= JoyButton.Up;

            if (gamePadState.IsButtonDown(Buttons.A)) chInputState.JoyButton |= JoyButton.Jump;
            if (gamePadState.IsButtonDown(Buttons.B)) chInputState.JoyButton |= JoyButton.Action1;
            if (gamePadState.IsButtonDown(Buttons.X)) chInputState.JoyButton |= JoyButton.Action2;
            if (gamePadState.IsButtonDown(Buttons.Start)) chInputState.JoyButton |= JoyButton.Menu;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _FontManager.Now = gameTime.ElapsedGameTime.Ticks;
            _FontManager.Cleanup();

            GraphicsDevice.Clear(Color.White);
            _SpriteBatch.Begin();

            var xCanvas = new XCanvas(this, _SpriteBatch, _Game.Width, _Game.Height);
            _Game.Draw(new GameDrawArgs(xCanvas));

            _SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
