using MiffTheFox.Chiamo.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SdlDotNet.Graphics;

namespace MiffTheFox.Chiamo.SDL
{
    public class SdlCanvas : Canvas
    {
        Surface _TargetSurface;

        public SdlCanvas(Surface target, int width, int height) : base(width, height)
        {
            _TargetSurface = target;
        }

        public override void Clear(Color color)
        {
            _TargetSurface.Fill(color);
        }

        public override void DrawSprite(SpriteInfo sprite, int x, int y, int w, int h, int row, int col)
        {
            var m = sprite.Manager as SdlSpriteManager;
            if (m != null)
            {
                var surface = m.GetSdlSurface(sprite.Name);
                _TargetSurface.Blit(surface, new Point(x, y), new Rectangle(w * col, h * row, w, h));
            }
        }

        public override void DrawString(FontInfo font, string text, Color color, int textHeight, int x, int y, int w, int h, bool bold, bool italic, StringAlignment hAlign, StringAlignment vAlign)
        {
            var m = font.Manager as SdlFontManager;
            if (m != null)
            {
                var sdlFont = m.GetSdlFont(font.Name, textHeight);
                sdlFont.Bold = bold;
                sdlFont.Italic = italic;
                
                using (var rendered = sdlFont.Render(text, color, false))
                {
                    var destPoint = new Point(x, y);

                    if (hAlign == StringAlignment.Center) { destPoint.X += (w - rendered.Width) >> 1; }
                    else if (hAlign == StringAlignment.Far) { destPoint.X += w - rendered.Width; }

                    if (vAlign == StringAlignment.Center) { destPoint.Y += (h - rendered.Height) >> 1; }
                    else if (vAlign == StringAlignment.Far) { destPoint.Y += h - rendered.Height; }

                    _TargetSurface.Blit(rendered, destPoint);
                }
            }
        }
    }
}
