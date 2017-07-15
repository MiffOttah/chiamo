using MiffTheFox.Chiamo.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Util
{
    public static class ResourceImporter
    {
        public static void ImportSprites<T>(SpriteManager spriteManager)
        {
            var imageType = typeof(Image);
            foreach (var prop in typeof(T).GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (imageType.IsAssignableFrom(prop.PropertyType))
                {
                    if (prop.GetValue(null) is Image image)
                    {
                        spriteManager.AddSprite(prop.Name, image);
                    }
                }
            }
        }
    }

    public class ResourceImportException : Exception
    {
        public ResourceImportException() : base("Could not obtain this assembly's resources.") { }
        public ResourceImportException(string message) : base(message) { }
    }
}
