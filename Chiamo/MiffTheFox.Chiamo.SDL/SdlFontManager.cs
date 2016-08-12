using MiffTheFox.Chiamo.Graphics;
using SdlDotNet.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.SDL
{
    public class SdlFontManager : FontManager
    {
        private Dictionary<string, FontSizeCollection> _Fonts = new Dictionary<string, FontSizeCollection>();

        public override void AddFont(string name, byte[] ttfData)
        {
            if (_Fonts.ContainsKey(name)) _Fonts[name].Dispose();
            _Fonts[name] = new FontSizeCollection(ttfData);
        }

        public override void Dispose()
        {
            foreach (var f in _Fonts.Values.ToArray()) f.Dispose();
            _Fonts = null;
        }

        public override string[] GetFontList()
        {
            return _Fonts.Keys.ToArray();
        }

        public override void RemoveFont(string name)
        {
            _Fonts[name].Dispose();
            _Fonts.Remove(name);
        }

        public Font GetSdlFont(string name, int size)
        {
            if (_Fonts.ContainsKey(name))
            {
                return _Fonts[name].GetSize(size);
            }
            else
            {
                throw new InvalidOperationException("Unknown font: " + name);
            }
        }

        private class FontSizeCollection : IDisposable
        {
            private byte[] _TtfData;
            private Dictionary<int, Font> _Sizes = new Dictionary<int, Font>();

            public FontSizeCollection(byte[] ttfData)
            {
                _TtfData = ttfData;
            }

            public Font GetSize(int size)
            {
                if (!_Sizes.ContainsKey(size))
                {
                    _Sizes[size] = new Font(_TtfData, size);
                }

                return _Sizes[size];
            }

            public void Dispose()
            {
                foreach (var f in _Sizes.Values.ToArray()) f.Dispose();
                _Sizes = null;
            }
        }
    }
}
