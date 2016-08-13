using MiffTheFox.Chiamo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformExample
{
    public class GameOverScreen : Scene
    {
        private bool _Success;
        private int _Score;

        public GameOverScreen(bool success, int score)
        {
            _Success = success;
            _Score = score;
        }

        public override void Initalize()
        {
            Game.Music.PlaySong(null);
        }

        public override void Draw(GameDrawArgs e)
        {
            e.Canvas.DrawSprite(Game.Sprites["EndGame"], 0, 0);
            e.Canvas.DrawString(Game.Fonts["sans-serif"], _Success ? "You win!" : "Game over", Color.White, 40, 0, 0, 640, 400, true, false, StringAlignment.Center, StringAlignment.Center);
            e.Canvas.DrawString(Game.Fonts["sans-serif"], $"Score: {_Score}", Color.White, 22, 0, 80, 640, 400, false, false, StringAlignment.Center, StringAlignment.Center);

            base.Draw(e);
        }

        public override void Tick(GameTickArgs e)
        {
            if (e.Input.JoyButton.HasFlag(JoyButton.Jump))
            {
                Game.PushScene(new PxLevel());
            }

            base.Tick(e);
        }
    }
}
