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

        private Rectangle? _LastViewPort = null;
        public Rectangle ViewPort => _LastViewPort ?? new Rectangle(0, 0, Width, Height);

        public ActorCollection Actors { get; private set; } = new ActorCollection();
        public TileMapCollection TileMaps { get; private set; } = new TileMapCollection();

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
            if (viewPort.Right > this.Width) viewPort.X = this.Width - viewPort.Width;
            if (viewPort.Bottom > this.Height) viewPort.Y = this.Height - viewPort.Height;

            _LastViewPort = viewPort;
            var oc = new OffsetCanvas(e.Canvas, -viewPort.X, -viewPort.Y);
            var oa = new GameDrawArgs(oc) { Game = e.Game };

            foreach (var tm in TileMaps)
            {
                Rectangle tileDrawRegion = new Rectangle(
                    viewPort.X - tm.Tileset.TileWidth,
                    viewPort.Y - tm.Tileset.TileHeight,
                    viewPort.Width + tm.Tileset.TileWidth * 2,
                    viewPort.Height + tm.Tileset.TileHeight * 2
                );

                for (int i = 0; i < tm.Width; i++)
                {
                    for (int j = 0; j < tm.Height; j++)
                    {
                        int canvasX = i * tm.Tileset.TileWidth;
                        int canvasY = j * tm.Tileset.TileHeight;

                        if (tileDrawRegion.Contains(canvasX, canvasY))
                        {
                            tm.Tileset.DrawTitle(oa, canvasX, canvasY, tm[i, j]);
                        }
                    }
                }
            }

            foreach (var actor in Actors.OrderBy(_ => _.ZIndex).ToArray())
            {
                if (actor.Bounds.IntersectsWith(viewPort))
                {
                    actor.Draw(oa);
                }
            }
        }

        public virtual void OnPopped()
        {
            Popped?.Invoke(this, new EventArgs());
        }
    }
}