using MiffTheFox.Chiamo.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiffTheFox.Chiamo;

namespace ChHelloWorld
{
    public class Player : PlayerActor
    {
        bool _Frame = false;
        bool _FacingLeft = false;

        public Player() : base(28, 38)
        {
        }

        public override void Draw(GameDrawArgs e)
        {
            DrawSprite(e, "testplayer", _FacingLeft ? 1 : 0, _Frame ? 1 : 0);
        }

        public override void Tick(GameTickArgs e, Scene s)
        {
            if (e.Input.JoyButton.HasFlag(JoyButton.Left))
            {
                _FacingLeft = true;
                if (Grounded) _Frame = !_Frame;
            }

            if (e.Input.JoyButton.HasFlag(JoyButton.Right))
            {
                _FacingLeft = false;
                if (Grounded) _Frame = !_Frame;
            }

            base.Tick(e, s);
        }
    }
}
