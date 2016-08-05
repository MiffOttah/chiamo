using MiffTheFox.Chiamo.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MiffTheFox.Chiamo.Winforms
{
    public class GdiSpriteManager : SpriteManager
    {
        private Dictionary<string, Bitmap> _Sprites = new Dictionary<string, Bitmap>();

        public override void AddSprite(string name, Image image)
        {
            RemoveSprite(name);
            _Sprites[name] = new Bitmap(image);
        }

        public override void RemoveSprite(string name)
        {
            if (_Sprites.ContainsKey(name))
            {
                _Sprites[name].Dispose();
                _Sprites.Remove(name);
            }
        }

        public override void Dispose()
        {
            while (_Sprites.Count > 0)
            {
                RemoveSprite(_Sprites.Keys.First());
            }
        }

        public Bitmap GetGdiSprite(string name)
        {
            return _Sprites[name];
        }

        public override SpriteInfo GetSpriteInfo(string name)
        {
            var gs = GetGdiSprite(name);
            return new SpriteInfo(gs.Width, gs.Height);
        }

        public override string[] GetSpriteList()
        {
            return _Sprites.Keys.ToArray();
        }
    }
}
