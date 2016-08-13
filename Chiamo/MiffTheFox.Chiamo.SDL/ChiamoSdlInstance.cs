using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using SdlDotNet.Input;

namespace MiffTheFox.Chiamo.SDL
{
    public class ChiamoSdlInstance
    {
        private Game _Game;
        private DateTime _LastTick;


        private JoyButton _Keys = JoyButton.None;
        private MouseButton _MouseButtons = MouseButton.None;
        private Point _MouseLocation = Point.Empty;

        public ChiamoSdlInstance(Game game)
        {
            this._Game = game;
        }

        public void Main()
        {
            _LastTick = DateTime.Now;

            Video.SetVideoMode(_Game.Width, _Game.Height);
            Video.WindowCaption = _Game.Title;

            Events.Quit += Events_Quit;
            Events.Tick += Events_Tick;
            Events.KeyboardDown += Events_KeyboardDown;
            Events.KeyboardUp += Events_KeyboardUp;
            Events.MouseButtonDown += Events_MouseButtonDown;
            Events.MouseButtonUp += Events_MouseButtonUp;
            Events.MouseMotion += Events_MouseMotion;

            _Game.Sprites = new SdlSpriteManager();

            _Game.Fonts = new SdlFontManager();
            _Game.Fonts.AddFont("sans-serif", Properties.Resources.Vera);
            _Game.Fonts.AddFont("serif", Properties.Resources.VeraSe);
            _Game.Fonts.AddFont("monospace", Properties.Resources.VeraMono);

            _Game.Music = new SdlMusicManager();
            ((SdlMusicManager)_Game.Music).Initalize();

            _Game.Initalize();
            Events.Run();
        }

        private void Events_Quit(object sender, QuitEventArgs e)
        {
            _Game.Exit();
        }

        private void Events_Tick(object sender, TickEventArgs e)
        {
            // Tick the game if needed
            var elapsed = DateTime.Now - _LastTick;

            if (elapsed.TotalMilliseconds > _Game.TargetTimerSpeed)
            {
                _Game.Tick(new GameTickArgs
                {
                    Input = new InputState
                    {
                        JoyButton = _Keys,
                        MouseButton = _MouseButtons,
                        MouseX = _MouseLocation.X,
                        MouseY = _MouseLocation.Y
                    }
                });
                _LastTick = DateTime.Now;
            }

            // Draw the game
            using (var drawSurface = new Surface(_Game.Width, _Game.Height))
            {
                drawSurface.Fill(Color.White);
                var canv = new SdlCanvas(drawSurface, _Game.Width, _Game.Height);
                _Game.Draw(new GameDrawArgs(canv));
                Video.Screen.Blit(drawSurface, new Point(0, 0));
                Video.Update();
            }

            // Exit if requested
            if (_Game.ExitRequested)
            {
                Events.QuitApplication();
            }
        }

        private JoyButton _GetJoyButton(Key key)
        {
            switch (key)
            {
                case Key.UpArrow: return JoyButton.Up;
                case Key.DownArrow: return JoyButton.Down;
                case Key.LeftArrow: return JoyButton.Left;
                case Key.RightArrow: return JoyButton.Right;
                case Key.Space: return JoyButton.Jump;
                case Key.LeftControl: return JoyButton.Action1;
                case Key.LeftAlt: return JoyButton.Action2;
                case Key.Escape: return JoyButton.Menu;
                default: return 0;
            }
        }

        private void Events_KeyboardDown(object sender, SdlDotNet.Input.KeyboardEventArgs e)
        {
            _Keys |= _GetJoyButton(e.Key);
        }

        private void Events_KeyboardUp(object sender, SdlDotNet.Input.KeyboardEventArgs e)
        {
            _Keys &= ~_GetJoyButton(e.Key);
        }

        private MouseButton _GetMouseButton(SdlDotNet.Input.MouseButton button)
        {
            switch (button)
            {
                case SdlDotNet.Input.MouseButton.PrimaryButton: return MouseButton.Left;
                case SdlDotNet.Input.MouseButton.SecondaryButton: return MouseButton.Right;
                default: return 0;
            }
        }

        private void Events_MouseButtonDown(object sender, SdlDotNet.Input.MouseButtonEventArgs e)
        {
            _MouseButtons |= _GetMouseButton(e.Button);
        }

        private void Events_MouseButtonUp(object sender, SdlDotNet.Input.MouseButtonEventArgs e)
        {
            _MouseButtons &= ~_GetMouseButton(e.Button);
        }

        private void Events_MouseMotion(object sender, SdlDotNet.Input.MouseMotionEventArgs e)
        {
            _MouseLocation = e.Position;
        }
    }
}