using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MiffTheFox.Chiamo.TMX
{
    /// <summary>
    /// Parses map data in the TMX (Tiled Map Editor) format.
    /// </summary>
    public class TmxParser
    {
        private XmlDocument _TmxDoc;

        public TmxParser(byte[] tmxData)
        {
            _TmxDoc = new XmlDocument();
            using (var ms = new MemoryStream(tmxData))
            {
                _TmxDoc.Load(ms);
            }

            if (_TmxDoc.DocumentElement.Name != "map" || _TmxDoc.DocumentElement.GetAttribute("orientation") != "orthogonal" || _TmxDoc.DocumentElement.GetAttribute("renderorder") != "right-down")
            {
                throw new TmxParseException("This doesn't look like an orthogonal right-down TMX map.");
            }
        }

        public void ParseTileLayer(string name, TileMap tilemap)
        {
            string widthStr = tilemap.Width.ToString(CultureInfo.InvariantCulture);
            string heightStr = tilemap.Height.ToString(CultureInfo.InvariantCulture);

            if (widthStr != _TmxDoc.DocumentElement.GetAttribute("width") || heightStr != _TmxDoc.DocumentElement.GetAttribute("height"))
            {
                throw new TmxParseException("Width and height of TMX map do not match tilemap given.");
            }

            foreach (XmlElement layer in _TmxDoc.DocumentElement.GetElementsByTagName("layer"))
            {
                if (layer.GetAttribute("name") == name)
                {
                    XmlElement data = layer.GetElementsByTagName("data").Cast<XmlElement>().Where(_ => _.GetAttribute("encoding") == "csv").FirstOrDefault();
                    if (data != null)
                    {
                        string[] csv = data.InnerText.Split(',');
                        int x = 0, y = 0;

                        foreach (string tileIdStr in csv)
                        {
                            if (!int.TryParse(tileIdStr.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out int tileId)) tileId = 1;

                            // Tiled uses 1-based indexes, Chiamo uses 0-based indexes.
                            // However, Tiled uses a 0 for undefined. When loading an undefined tile, simply fall back to the default type type (id = 0)
                            if (tileId > 0)
                            {
                                tileId--;
                            }

                            tilemap[x, y] = Convert.ToByte(tileId);

                            x++;
                            if (x >= tilemap.Width)
                            {
                                x = 0;
                                y++;
                            }
                        }

                        return;
                    }
                }
            }

            throw new TmxParseException("A tile layer with the specified name was not found. Make sure that the encoding is CSV.");
        }

        public void ParseObjectLayer(string name, Action<TmxObjectInfo> actorCreationCallback)
        {
            foreach (XmlElement layer in _TmxDoc.DocumentElement.GetElementsByTagName("objectgroup"))
            {
                if (layer.GetAttribute("name") == name)
                {
                    foreach (XmlElement objectEl in layer.GetElementsByTagName("object"))
                    {
                        var toi = new TmxObjectInfo(objectEl);
                        actorCreationCallback(toi);
                    }

                    return;
                }
            }

            throw new TmxParseException("A object layer with the specified name was not found.");
        }
    }
}
