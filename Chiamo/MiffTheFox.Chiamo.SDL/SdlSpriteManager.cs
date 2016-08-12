using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiffTheFox.Chiamo.Graphics;
using SdlDotNet.Graphics;

namespace MiffTheFox.Chiamo.SDL
{
    public class SdlSpriteManager : SpriteManager
    {
        private Dictionary<string, Surface> _SpriteSurfaces = new Dictionary<string, Surface>();

        public override void AddSprite(string name, Image image)
        {
            using (var b = new Bitmap(image))
            {
                _SpriteSurfaces.Add(name, new Surface(b));
            }
        }

        public override SpriteInfo GetSpriteInfo(string name)
        {
            if (_SpriteSurfaces.ContainsKey(name))
            {
                var surface = _SpriteSurfaces[name];
                return new SpriteInfo(surface.Width, surface.Height) { Manager = this, Name = name };
            }
            else
            {
                throw new InvalidOperationException("Unknown sprite: " + name);
            }
        }

        public override string[] GetSpriteList()
        {
            return _SpriteSurfaces.Keys.ToArray();
        }

        public override void RemoveSprite(string name)
        {
            _SpriteSurfaces.Remove(name);
        }

        public override void Dispose()
        {
            foreach (var ss in _SpriteSurfaces.Values.ToArray()) ss.Dispose();
            _SpriteSurfaces.Clear();
        }

        public Surface GetSdlSurface(string name)
        {
            if (_SpriteSurfaces.ContainsKey(name))
            {
                return _SpriteSurfaces[name];
            }
            else
            {
                throw new InvalidOperationException("Unknown sprite: " + name);
            }
        }
    }
}
