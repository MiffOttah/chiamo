using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiffTheFox.Chiamo;

namespace MiffTheFox.Chiamo.Winforms
{
    public partial class GameForm : Form
    {
        private Game _Game;

        private JoyButton _Keys = JoyButton.None;
        private MouseButton _MouseButtons = MouseButton.None;
        private Point _MouseLocation = Point.Empty;

        public GameForm(Game game)
        {
            InitializeComponent();

            game.Sprites = new GdiSpriteManager();
            game.Fonts = new GdiFontManager();
            this._Game = game;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            _Game.Initalize();

            this.ClientSize = new Size(_Game.Width, _Game.Height);
            this.Text = _Game.Title;

            _GameTimer.Interval = _Game.TargetTimerSpeed;
            _GameTimer.Start();
        }

        private void _GameTimer_Tick(object sender, EventArgs e)
        {
            var inputState = new InputState()
            {
                JoyButton = _Keys,
                MouseButton = _MouseButtons,
                MouseX = _MouseLocation.X,
                MouseY = _MouseLocation.Y
            };

            _Game.Tick(new GameTickArgs() { Input = inputState });

            var gameImage = new Bitmap(_Game.Width, _Game.Height);
            var g = System.Drawing.Graphics.FromImage(gameImage);
            try
            {
                var gc = new GdiCanvas(_Game.Width, _Game.Height, g);
                gc.Clear(Color.Black);
                _Game.Draw(new GameDrawArgs(gc));
            }
            finally
            {
                g.Dispose();
            }

            var oldImage = this.BackgroundImage;
            this.BackgroundImage = gameImage;
            if (oldImage != null) oldImage.Dispose();
        }

        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _Game.Dispose();
        }

        private JoyButton _GetJoyButton(Keys k)
        {
            switch (k)
            {
                case Keys.Space: return JoyButton.Jump;
                case Keys.ControlKey: return JoyButton.Action1;
                case Keys.Alt: return JoyButton.Action2;
                case Keys.Escape: return JoyButton.Menu;
                case Keys.Up: return JoyButton.Up;
                case Keys.Down: return JoyButton.Down;
                case Keys.Left: return JoyButton.Left;
                case Keys.Right: return JoyButton.Right;
                default: return 0;
            }
        }

        private MouseButton _GetMouseButton(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left: return MouseButton.Left;
                case MouseButtons.Right: return MouseButton.Right;
                default: return 0;
            }
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            _Keys |= _GetJoyButton(e.KeyCode);
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            _Keys &= ~_GetJoyButton(e.KeyCode);
        }

        private void GameForm_MouseDown(object sender, MouseEventArgs e)
        {
            _MouseButtons |= _GetMouseButton(e.Button);
        }

        private void GameForm_MouseUp(object sender, MouseEventArgs e)
        {
            _MouseButtons &= ~_GetMouseButton(e.Button);
        }

        private void GameForm_MouseMove(object sender, MouseEventArgs e)
        {
            _MouseLocation = e.Location;
        }
    }
}
