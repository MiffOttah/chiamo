using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Graphics
{
    public abstract class FontManager : IDisposable
    {
        public abstract void AddFont(string name, byte[] ttfData);
        public abstract void RemoveFont(string name);
        public abstract string[] GetFontList();
        public abstract void Dispose();

        public FontInfo this[string name]
        {
            get
            {
                return new FontInfo { Manager = this, Name = name };
            }
        }
    }
    
    public class FontInfo
    {
        public FontManager Manager { get; set; }
        public string Name { get; set; }
    }
}
