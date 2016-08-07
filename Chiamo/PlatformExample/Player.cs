using System;
using MiffTheFox.Chiamo;
using MiffTheFox.Chiamo.Actors;

namespace PlatformExample
{
    public class Player : PlayerActor
    {
        private bool _Frame = false;

        public Player() : base(40, 75)
        {
        }

        public override void Draw(GameDrawArgs e)
        {
            DrawSprite(e, "Player", (int)this.Facing, _Frame ? 1 : 0);
        }


        public override void Tick(GameTickArgs e, Scene s)
        {
            if ((e.Input.JoyButton.HasFlag(JoyButton.Left) || e.Input.JoyButton.HasFlag(JoyButton.Right)) && Grounded)
            {
                _Frame = !_Frame;
            }

            base.Tick(e, s);
        }
    }
}