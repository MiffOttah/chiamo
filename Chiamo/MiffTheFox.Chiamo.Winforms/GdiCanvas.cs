using MiffTheFox.Chiamo.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MiffTheFox.Chiamo.Winforms
{
    public class GdiCanvas : Canvas
    {
        protected System.Drawing.Graphics GdiGraphics { get; private set; }

        public GdiCanvas(int width, int height, System.Drawing.Graphics g) : base(width, height)
        {
            GdiGraphics = g;
        }

        public override void Clear(Color color)
        {
            GdiGraphics.Clear(color);
        }

        public override void DrawSprite(SpriteInfo sprite, int x, int y, int w, int h, int row, int col)
        {
            var manager = sprite.Manager as GdiSpriteManager;
            if (manager == null) throw new InvalidOperationException("This sprite is not GDI-based.");
            var spriteBitmap = manager.GetGdiSprite(sprite.Name);

            var destRect = new Rectangle(x, y, w, h);
            var srcRect = new Rectangle(w * col, h * row, w, h);

            GdiGraphics.DrawImage(spriteBitmap, destRect, srcRect, GraphicsUnit.Pixel);
        }
    }
}
