using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiffTheFox.Chiamo.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using GdiImage = System.Drawing.Image;

namespace MiffTheFox.Chiamo.MonoGame
{
    public class XSpriteManager : SpriteManager
    {
        private Dictionary<string, Texture2D> _SpriteTextures = new Dictionary<string, Texture2D>();
        private ChiamoMonoInstance _Instance;

        public XSpriteManager(ChiamoMonoInstance instance)
        {
            _Instance = instance;
        }

        public override void AddSprite(string name, GdiImage image)
        {
            _SpriteTextures.Add(name, GdiToXna(_Instance.GraphicsDevice ,image));
        }

        public static Texture2D GdiToXna(GraphicsDevice d, GdiImage image)
        {
            Texture2D texture;
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                texture = Texture2D.FromStream(d, ms);
            }

            return texture;
        }

        public override SpriteInfo GetSpriteInfo(string name)
        {
            if (_SpriteTextures.ContainsKey(name))
            {
                var texture = _SpriteTextures[name];
                return new SpriteInfo(texture.Width, texture.Height) { Manager = this, Name = name };
            }
            else
            {
                throw new InvalidOperationException("Unknown sprite: " + name);
            }
        }

        public override string[] GetSpriteList()
        {
            return _SpriteTextures.Keys.ToArray();
        }

        public override void RemoveSprite(string name)
        {
            _SpriteTextures.Remove(name);
        }

        public override void Dispose()
        {
            foreach (var ss in _SpriteTextures.Values.ToArray()) ss.Dispose();
            _SpriteTextures.Clear();
        }

        public void DrawTexture2d(string name, SpriteBatch batch, int x, int y, int w, int h, int row, int col)
        {
            if (_SpriteTextures.ContainsKey(name))
            {
                var texture = _SpriteTextures[name];

                var destR = new Rectangle(x, y, w, h);
                var sourceR = new Rectangle(w * col, h * row, w, h);

                batch.Draw(texture, destR, sourceR, Color.White);
            }
            else
            {
                throw new InvalidOperationException("Unknown sprite: " + name);
            }
        }
    }
}
