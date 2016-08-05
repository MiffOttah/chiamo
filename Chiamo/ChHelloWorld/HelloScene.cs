using MiffTheFox.Chiamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChHelloWorld
{
    /// <summary>
    /// This is a simple scene to demonstrate the capabilities of Chiamo. Left-click to place a ball onto the field, right-click a ball to remove it, or press escape to remove them all.
    /// </summary>
    public class HelloScene : Scene
    {
        bool _Clicked = false;
        Random _RNG = new Random();

        public override void Initalize()
        {
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
                        XMomentum = _RNG.Next(-1, 2) * 10,
                        YMomentum = _RNG.Next(-1, 2) * 10
                    });
                    _Clicked = true;
                }
            }
            else
            {
                _Clicked = false;
            }

            // press the menu button (escape) to remove all balls
            if (e.Input.JoyButton.HasFlag(JoyButton.Menu))
            {
                foreach (var actor in Actors.Where(_ => _ is BallActor).ToArray())
                {
                    Actors.Remove(actor);
                }
            }

            // update actors
            base.Tick(e);
        }

        public override void Draw(GameDrawArgs e)
        {
            e.Canvas.Clear(System.Drawing.Color.White);
            base.Draw(e);
        }
    }
}
