using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Graphics
{
    public abstract class SpriteManager : IDisposable
    {
        public abstract void AddSprite(string name, Image image);
        public abstract void RemoveSprite(string name);
        public abstract string[] GetSpriteList();
        public abstract SpriteInfo GetSpriteInfo(string name);
        
        public SpriteInfo this[string name]
        {
            get
            {
                var si = GetSpriteInfo(name);
                si.Name = name;
                si.Manager = this;
                return si;
            }
        }

        public abstract void Dispose();
    }

    public class SpriteInfo
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public SpriteManager Manager { get; set; }

        public SpriteInfo(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}
