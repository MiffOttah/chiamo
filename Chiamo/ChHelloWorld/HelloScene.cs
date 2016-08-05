using MiffTheFox.Chiamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChHelloWorld
{
    public class HelloScene : Scene
    {
        bool _Clicked = false;
        Random _RNG = new Random();

        public override void Initalize()
        {
            Actors.Add(new BallActor() { X = 10, Y = 10, XMomentum = 10, YMomentum = 10 });
            Actors.Add(new BallActor() { X = 100, Y = 10, XMomentum = -10, YMomentum = 10 });
        }

        public override void Tick(GameTickArgs e)
        {
            if (e.Input.MouseButton == MouseButton.Left)
            {
                if (!_Clicked)
                {
                    Actors.Add(new BallActor()
                    {
                        X = e.Input.MouseX - 20,
                        Y = e.Input.MouseY - 20,
                        XMomentum = _RNG.Next(-1, 1) * 10,
                        YMomentum = _RNG.Next(-1, 1) * 10
                    });
                    _Clicked = true;
                }
            }
            else
            {
                _Clicked = false;
            }

            // update actors
            base.Tick(e);
        }
    }
}
