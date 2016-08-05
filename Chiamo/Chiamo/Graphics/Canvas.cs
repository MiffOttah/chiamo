using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Graphics
{
    public abstract class Canvas
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        protected Canvas(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public abstract void Clear(Color color);

        public void DrawSprite(SpriteInfo sprite, int x, int y)
        {
            DrawSprite(sprite, x, y, sprite.Width, sprite.Height, 0, 0);
        }
        public void DrawSprite(SpriteInfo sprite, int x, int y, int w, int h)
        {
            DrawSprite(sprite, x, y, w, h, 0, 0);
        }
        public void DrawSprite(SpriteInfo sprite, Point location)
        {
            DrawSprite(sprite, location.X, location.Y);
        }
        public void DrawSprite(SpriteInfo sprite, Rectangle location)
        {
            DrawSprite(sprite, location.X, location.Y, location.Width, location.Height);
        }
        public abstract void DrawSprite(SpriteInfo sprite, int x, int y, int w, int h, int row, int col);

        public void DrawString(FontInfo font, string text, Color color, int textHeight, Rectangle location)
        {
            DrawString(font, text, color, textHeight, location.X, location.Y, location.Width, location.Height, false, false, StringAlignment.Near, StringAlignment.Near);
        }
        public void DrawString(FontInfo font, string text, Color color, int textHeight, int x, int y, int w, int h)
        {
            DrawString(font, text, color, textHeight, x, y, w, h, false, false, StringAlignment.Near, StringAlignment.Near);
        }
        public void DrawString(FontInfo font, string text, Color color, int textHeight, Rectangle location, bool bold, bool italic, StringAlignment hAlign, StringAlignment vAlign)
        {
            DrawString(font, text, color, textHeight, location.X, location.Y, location.Width, location.Height, bold, italic, hAlign, vAlign);
        }
        public abstract void DrawString(FontInfo font, string text, Color color, int textHeight, int x, int y, int w, int h, bool bold, bool italic, StringAlignment hAlign, StringAlignment vAlign);
    }
}
