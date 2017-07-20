using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace MiffTheFox.Chiamo.TMX
{
    public sealed class TmxObjectInfo
    {
        public string Name { get; private set; }
        public string Type { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Rectangle Bounds => new Rectangle(X, Y, Width, Height);
        public bool HasProperties => _Properties != null;
        public string this[string propertyKey] => (_Properties != null && _Properties.ContainsKey(propertyKey)) ? _Properties[propertyKey] : null;

        private readonly Dictionary<string, string> _Properties;

        internal TmxObjectInfo(XmlElement objectElement)
        {
            Name = objectElement.GetAttribute("name");
            Type = objectElement.GetAttribute("type");
            if (string.IsNullOrEmpty(Type)) Type = Name;

            X = _ParseAttr(objectElement, "x");
            Y = _ParseAttr(objectElement, "y");
            Width = _ParseAttr(objectElement, "width");
            Height = _ParseAttr(objectElement, "height");

            if (objectElement.HasAttribute("gid"))
            {
                Y -= Height;
            }

            var propertiesElement = objectElement.ChildNodes.Cast<XmlNode>().Where(xn => xn is XmlElement && xn.Name == "properties").FirstOrDefault() as XmlElement;
            if (propertiesElement == null)
            {
                _Properties = null;
            }
            else
            {
                _Properties = new Dictionary<string, string>();

                foreach (XmlElement propertyElement in propertiesElement.GetElementsByTagName("property"))
                {
                    string name = propertyElement.GetAttribute("name");
                    string value = string.IsNullOrEmpty(propertyElement.InnerText) ? propertyElement.GetAttribute("value") : propertyElement.InnerText;
                    _Properties.Add(name, value);
                }
            }
        }

        private static int _ParseAttr(XmlElement element, string attributeName)
        {
            string v = element.GetAttribute(attributeName);
            if (!string.IsNullOrEmpty(v) && float.TryParse(v, NumberStyles.Number, CultureInfo.InvariantCulture, out float f))
            {
                return Convert.ToInt32(f);
            }
            else
            {
                return 0;
            }
        }
    }
}
