using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo
{
    public abstract class Actor
    {
        /// <summary>
        /// Represents a unique ID for each instance of an actor.
        /// </summary>
        public Guid Guid { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Rectangle Bounds
        {
            get { return new Rectangle(X, Y, Width, Height); }
            set { X = value.X; Y = value.Y; Width = value.Width; Height = value.Height; }
        }
        public int ZIndex { get; set; }

        public Actor(int width, int height)
        {
            Guid = Guid.NewGuid();
            X = 0;
            Y = 0;
            Width = width;
            Height = height;
            ZIndex = 0;
        }

        public virtual void Tick(GameTickArgs e, Scene s) { }

        public abstract void Draw(GameDrawArgs e);

        protected void DrawSprite(GameDrawArgs e, string spriteName, int row = 0, int col = 0)
        {
            e.Canvas.DrawSprite(e.Game.Sprites[spriteName], X, Y, Width, Height, row, col);
        }

        public override bool Equals(object obj)
        {
            var otherActor = obj as Actor;
            if (otherActor != null)
            {
                return this.Guid.Equals(otherActor.Guid);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Guid.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1:B}", this.GetType().Name, this.Guid);
        }
    }
}
