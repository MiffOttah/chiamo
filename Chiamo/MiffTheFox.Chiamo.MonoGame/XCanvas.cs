using MiffTheFox.Chiamo.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

using GColor = System.Drawing.Color;
using StringAlignment = System.Drawing.StringAlignment;

namespace MiffTheFox.Chiamo.MonoGame
{
    public class XCanvas : Canvas
    {
        private ChiamoMonoInstance _ChiamoMonoInstance;
        private SpriteBatch _SpriteBatch;
        
        public XCanvas(ChiamoMonoInstance chiamoMonoInstance, SpriteBatch spriteBatch, int width, int height) : base(width, height)
        {
            _ChiamoMonoInstance = chiamoMonoInstance;
            _SpriteBatch = spriteBatch;
        }

        public override void Clear(GColor color)
        {
            _ChiamoMonoInstance.GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A));
        }

        public override void DrawSprite(SpriteInfo sprite, int x, int y, int w, int h, int row, int col)
        {
            if (sprite.Manager is XSpriteManager m)
            {
                m.DrawTexture2d(sprite.Name, _SpriteBatch, x, y, w, h, row, col);
            }
        }

        public override void DrawString(FontInfo font, string text, GColor color, int textHeight, int x, int y, int w, int h, bool bold, bool italic, StringAlignment hAlign, StringAlignment vAlign)
        {
            if (font.Manager is XFontManager m)
            {
                m.PrerenderAndDraw(_SpriteBatch, font.Name, text, color, textHeight, x, y, w, h, bold, italic, hAlign, vAlign);
            }
        }
    }
}
