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

        public GameForm(Game game)
        {
            InitializeComponent();

            game.Sprites = new GdiSpriteManager();
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
            _Game.Tick(new GameTickArgs());

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
    }
}
