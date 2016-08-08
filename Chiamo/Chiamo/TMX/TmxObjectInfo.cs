using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MiffTheFox.Chiamo.TMX
{
    public sealed class TmxObjectInfo
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        internal TmxObjectInfo(XmlElement objectElement)
        {
            Name = objectElement.GetAttribute("name") ?? "";
            X = 0;
            Y = 0;

            int n;
            if (int.TryParse(objectElement.GetAttribute("x"), NumberStyles.None, CultureInfo.InvariantCulture, out n)) X = n;
            if (int.TryParse(objectElement.GetAttribute("y"), NumberStyles.None, CultureInfo.InvariantCulture, out n))
            {
                Y = n;

                // Tiled uses the origin point of an actor in the bottom left, whereis Chiamo uses the top left.
                // Therefore, fix the Y value by adding the height the Tiled expected the object to be.

                if (int.TryParse(objectElement.GetAttribute("height"), NumberStyles.None, CultureInfo.InvariantCulture, out n))
                {
                    Y -= n;
                }
            }
        }
    }
}
