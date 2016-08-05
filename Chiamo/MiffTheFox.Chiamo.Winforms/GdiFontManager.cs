using MiffTheFox.Chiamo.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Winforms
{
    public class GdiFontManager : FontManager
    {
        Dictionary<string, FontFamily> _Fonts;
        PrivateFontCollection _PrivFonts;
        List<IntPtr> _Memory;

        public GdiFontManager()
        {
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
        }

        public FontFamily GetFontFamily(string name)
        {
            return _Fonts[name];
        }
    }
}
