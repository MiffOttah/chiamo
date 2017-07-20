using MiffTheFox.Chiamo;
using MiffTheFox.Chiamo.Actors;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        const int GRAVITY = 2;

        public override void Initalize()
        {
            var player = new Player();
            player.X = (Game.Width - player.Width) / 2;
            player.Y = (Game.Height - player.Height) / 2;
            player.Gravity = GRAVITY;
            player.ZIndex = 10;
            //player.CameraFollows = true;
            Actors.Add(player);
        }

        public override void Tick(GameTickArgs e)
        {
            if (e.Input.MouseButton == MouseButton.Left)
            {
                if (!_Clicked)
                {
                    MomentumCollisionActor ball = _RNG.Next(2) == 0 ? (MomentumCollisionActor)(new Ball()) : (MomentumCollisionActor)(new GravityBall() { Gravity = GRAVITY });

                    ball.X = e.Input.MouseX - 20;
                    ball.Y = e.Input.MouseY - 20;
                    ball.XMomentum = _RNG.Next(-1, 2) * 10;
                    ball.YMomentum = _RNG.Next(-1, 2) * 10;

                    Actors.Add(ball);
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
                foreach (var actor in Actors.Where(_ => _ is Ball || _ is GravityBall).ToArray())
                {
                    Actors.Remove(actor);
                }
            }

            // demonstrate mouse capture when the Z key is held down
            Game.CaptureMouse = e.Input.JoyButton.HasFlag(JoyButton.Action1);

            // update actors
            base.Tick(e);
        }

        public override void Draw(GameDrawArgs e)
        {
            e.Canvas.Clear(Color.White);

            e.Canvas.DrawString(Game.Fonts["ArchitectsDaughter"], "Hello, world!", Color.Black, 80, 0, 0, Game.Width, Game.Height, true, false, StringAlignment.Center, StringAlignment.Center);
            e.Canvas.DrawString(Game.Fonts["ArchitectsDaughter"], "Font © Kimberly Geswein", Color.Black, 12, 0, 0, Game.Width, Game.Height, false, false, StringAlignment.Far, StringAlignment.Far);

            base.Draw(e);
        }
    }
}
