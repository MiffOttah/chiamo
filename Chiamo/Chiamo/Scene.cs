using MiffTheFox.Chiamo.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo
{
    public abstract class Scene
    {
        public virtual bool IsTransparent { get; set; } = false;
        public virtual Game Game { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public Point CameraFocus { get; set; }

        public ActorCollection Actors { get; private set; } = new ActorCollection();

        public event EventHandler Popped;

        public abstract void Initalize();

        public virtual void Tick(GameTickArgs e)
        {
            foreach (var actor in Actors.ToArray())
            {
                actor.Tick(e, this);
            }
        }

        public virtual void Draw(GameDrawArgs e)
        {
            var viewPort = new Rectangle(this.CameraFocus.X - Game.Width / 2, this.CameraFocus.Y - Game.Height / 2, Game.Width, Game.Height);

            if (viewPort.X < 0) viewPort.X = 0;
            if (viewPort.Y < 0) viewPort.Y = 0;
            if (viewPort.Right > Game.Width) viewPort.X = Game.Width - viewPort.Width;
            if (viewPort.Bottom > Game.Height) viewPort.Y = Game.Height - viewPort.Height;

            var oc = new OffsetCanvas(e.Canvas, -viewPort.X, -viewPort.Y);

            foreach (var actor in Actors.OrderBy(_ => _.ZIndex).ToArray())
            {
                if (actor.Bounds.IntersectsWith(viewPort))
                {
                    actor.Draw(new GameDrawArgs(oc) { Game = e.Game });
                }
            }
        }

        public virtual void OnPopped()
        {
            Popped?.Invoke(this, new EventArgs());
        }
    }
}