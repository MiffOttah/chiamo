using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo
{
    public class Camera
    {
        public Point Focus { get; set; }
        public bool ConstrainToScene { get; set; } = true;

        private Rectangle? _LastViewPort;
        public Rectangle ViewPort => _LastViewPort ?? new Rectangle(0, 0, 0, 0);

        public Camera()
        {
            Focus = Point.Empty;
            _LastViewPort = null;
        }

        public Rectangle RecalcuateViewPort(Scene s)
        {
            var viewPort = new Rectangle(this.Focus.X - s.Game.Width / 2, this.Focus.Y - s.Game.Height / 2, s.Game.Width, s.Game.Height);

            if (ConstrainToScene)
            {
                if (viewPort.X < 0) viewPort.X = 0;
                if (viewPort.Y < 0) viewPort.Y = 0;
                if (viewPort.Right > s.Width) viewPort.X = s.Width - viewPort.Width;
                if (viewPort.Bottom > s.Height) viewPort.Y = s.Height - viewPort.Height;
            }

            _LastViewPort = viewPort;
            return viewPort;
        }
    }
}
