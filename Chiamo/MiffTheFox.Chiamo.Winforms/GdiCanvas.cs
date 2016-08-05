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

        private static Dictionary<Color, Brush> _Brushes = new Dictionary<Color, Brush>();

        public GdiCanvas(int width, int height, System.Drawing.Graphics g) : base(width, height)
        {
            GdiGraphics = g;
        }

        private Brush _GetBrush(Color color)
        {
            if (_Brushes.ContainsKey(color))
            {
                return _Brushes[color];
            }
            else
            {
                var sb = new SolidBrush(color);
                _Brushes[color] = sb;
                return sb;
            }
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
        
        public override void DrawString(FontInfo font, string text, Color color, int textHeight, int x, int y, int w, int h, bool bold, bool italic, StringAlignment hAlign, StringAlignment vAlign)
        {
            var manager = font.Manager as GdiFontManager;
            if (manager == null) throw new InvalidOperationException("This font is not GDI-based.");

            var family = manager.GetFontFamily(font.Name);
            var destRect = new RectangleF(x, y, w, h);

            var style = FontStyle.Regular;
            if (bold) style |= FontStyle.Bold;
            if (italic) style |= FontStyle.Italic;

            var brush = _GetBrush(color);
            var gdiFont = new Font(family, textHeight, style, GraphicsUnit.Pixel);

            var sf = new StringFormat { Alignment = hAlign, LineAlignment = vAlign };

            GdiGraphics.DrawString(text, gdiFont, brush, destRect, sf);
        }
    }
}
