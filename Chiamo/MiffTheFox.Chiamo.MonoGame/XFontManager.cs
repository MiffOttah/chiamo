using MiffTheFox.Chiamo.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace MiffTheFox.Chiamo.MonoGame
{
    public class XFontManager : FontManager
    {
        Dictionary<string, FontFamily> _Fonts;
        PrivateFontCollection _PrivFonts;
        List<IntPtr> _Memory;
        private ChiamoMonoInstance _Instance;

        private Dictionary<long, Texture2D> _StringRenderings = new Dictionary<long, Texture2D>();
        private Dictionary<long, long> _StringAges = new Dictionary<long, long>();
        const long TIMEOUT_AGE = 10000000 * 40;
        private long _LastCleanup = 0;
        public long Now { get; set; } = 0;

        public XFontManager(ChiamoMonoInstance instance)
        {
            _Instance = instance;

            _Fonts = new Dictionary<string, FontFamily>();
            _Fonts.Add("sans-serif", FontFamily.GenericSansSerif);
            _Fonts.Add("serif", FontFamily.GenericSerif);
            _Fonts.Add("monospace", FontFamily.GenericMonospace);

            _PrivFonts = new PrivateFontCollection();
            _Memory = new List<IntPtr>();
        }

        public override void AddFont(string name, byte[] ttfData)
        {
            // System.Drawing gives us no good way to get the family we just added,
            // so we'll compare to see what got added when we load the TTF data.

            string[] familes = _PrivFonts.Families.Select(_ => _.Name).ToArray();

            // We have to copy to an IntPtr to load from memory for some reason.
            var ttfPointer = Marshal.AllocHGlobal(ttfData.Length);
            Marshal.Copy(ttfData, 0, ttfPointer, ttfData.Length);

            _PrivFonts.AddMemoryFont(ttfPointer, ttfData.Length);
            _Memory.Add(ttfPointer);

            // Now find out the name of the new font, and add its family
            foreach (var f in _PrivFonts.Families.Where(_ => !familes.Contains(_.Name)))
            {
                _Fonts[name] = f;
            }
        }

        public override void RemoveFont(string name)
        {
            _Fonts.Remove(name);
        }

        public override string[] GetFontList()
        {
            return _Fonts.Keys.ToArray();
        }

        public override void Dispose()
        {
            // clear the font dictionary
            foreach (var v in _Fonts) v.Value.Dispose();
            _Fonts.Clear();

            // clear the private font cache
            _PrivFonts.Dispose();

            // clear the ttf files from memory
            foreach (var ip in _Memory) Marshal.FreeHGlobal(ip);
            _Memory.Clear();

            // clear the prerendered strings
            ClearStringCache();
        }

        public void PrerenderAndDraw(SpriteBatch batch, string fontName, string text, Color color, int textHeight, int x, int y, int w, int h, bool bold, bool italic, StringAlignment hAlign, StringAlignment vAlign)
        {
            long jobId;
            bool render = true;

            unchecked
            {
                jobId = fontName.GetHashCode();
                jobId = (jobId * 397) ^ textHeight;
                jobId = (jobId * 397) ^ w;
                jobId = (jobId * 397) ^ h;
                jobId = (jobId * 397) ^ color.ToArgb();

                jobId ^= ((long)text.GetHashCode()) << 32;
            }

            if (_StringAges.ContainsKey(jobId))
            {
                if ((Now - _StringAges[jobId]) >= TIMEOUT_AGE)
                {
                    _RemoveRendering(jobId);
                }
                else
                {
                    render = false;
                }
            }

            if (render)
            {
                if (!_Fonts.ContainsKey(fontName)) throw new InvalidOperationException("Cannot find the provided font.");
                using (var gdiBitmap = new Bitmap(w, h))
                {
                    using (var g = System.Drawing.Graphics.FromImage(gdiBitmap))
                    {
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                        var destRect = new RectangleF(0, 0, w, h);
                        var style = FontStyle.Regular;
                        if (bold) style |= FontStyle.Bold;
                        if (italic) style |= FontStyle.Italic;
                        var brush = new SolidBrush(color);
                        var gdiFont = new Font(_Fonts[fontName], textHeight, style, GraphicsUnit.Pixel);
                        var sf = new StringFormat { Alignment = hAlign, LineAlignment = vAlign };
                        g.DrawString(text, gdiFont, brush, destRect, sf);
                    }

                    _StringRenderings[jobId] = XSpriteManager.GdiToXna(_Instance.GraphicsDevice, gdiBitmap);
                }
            }

            batch.Draw(_StringRenderings[jobId], new Microsoft.Xna.Framework.Rectangle(x, y, w, h), Microsoft.Xna.Framework.Color.White);
        }

        public void Cleanup()
        {
            if ((Now - _LastCleanup) >= TIMEOUT_AGE)
            {
                foreach (long jobId in _StringAges.Where(a => (Now - a.Value) >= TIMEOUT_AGE).Select(a => a.Key).ToArray())
                {
                    _StringAges.Remove(jobId);
                    _RemoveRendering(jobId);
                }
            }
        }

        public void ClearStringCache()
        {
            foreach (var sr in _StringRenderings.Values.ToArray()) sr.Dispose();
            _StringRenderings.Clear();
            _StringAges.Clear();
        }

        private void _RemoveRendering(long jobId)
        {
            _StringRenderings[jobId].Dispose();
            _StringRenderings.Remove(jobId);
        }
    }
}
