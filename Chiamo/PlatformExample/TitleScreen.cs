using MiffTheFox.Chiamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformExample
{
    public class TitleScreen : Scene
    {
        public override void Initalize()
        {
        }

        public override void Draw(GameDrawArgs e)
        {
            e.Canvas.DrawSprite(Game.Sprites["Title"], 0, 0);

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
