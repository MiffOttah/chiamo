using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Graphics
{
    public class OffsetCanvas : Canvas
    {
        private Canvas _Parent;
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }

        public OffsetCanvas(Canvas parentCanvas, int offsetX, int offsetY) : base(parentCanvas.Width, parentCanvas.Height)
        {
            _Parent = parentCanvas;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }

        public override void Clear(Color color)
        {
            _Parent.Clear(color);
        }

        public override void DrawSprite(SpriteInfo sprite, int x, int y, int w, int h, int row, int col)
        {
            _Parent.DrawSprite(sprite, x + OffsetX, y + OffsetY, w, h, row, col);
        }

        public override void DrawString(FontInfo font, string text, Color color, int textHeight, int x, int y, int w, int h, bool bold, bool italic, StringAlignment hAlign, StringAlignment vAlign)
        {
            _Parent.DrawString(font, text, color, textHeight, x + OffsetX, y + OffsetY, w, h, bold, italic, hAlign, vAlign);
        }
    }
}
