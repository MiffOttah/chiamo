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

        private readonly Camera _Camera = new Camera();
        public Camera Camera => _Camera;

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
            var viewPort = _Camera.RecalcuateViewPort(this);

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