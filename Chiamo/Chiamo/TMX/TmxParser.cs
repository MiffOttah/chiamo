using MiffTheFox.Chiamo.Tiles;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace MiffTheFox.Chiamo.TMX
{
    /// <summary>
    /// Parses map data in the TMX (Tiled Map Editor) format.
    /// </summary>
    public class TmxParser
    {
        private XmlDocument _TmxDoc;

        public Size MapSizeInPixels { get; }
        public Size MapSizeInTiles { get; }
        public Size TileSize { get; }

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

            if (_TmxDoc.DocumentElement.GetAttribute("infinite") == "1")
            {
                throw new TmxParseException("Infinite maps are not supported.");
            }

            try
            {
                int width = int.Parse(_TmxDoc.DocumentElement.GetAttribute("width"));
                int height = int.Parse(_TmxDoc.DocumentElement.GetAttribute("height"));
                int tileWidth = int.Parse(_TmxDoc.DocumentElement.GetAttribute("tilewidth"));
                int tileHeight = int.Parse(_TmxDoc.DocumentElement.GetAttribute("tileheight"));

                MapSizeInPixels = new Size(width * tileWidth, height * tileHeight);
                MapSizeInTiles = new Size(width, height);
                TileSize = new Size(tileWidth, tileHeight);
            }
            catch (FormatException)
            {
                throw new TmxParseException("Failed to parse TMX map size.");
            }
        }

        public void ParseTileLayer(string name, TileMap tilemap)
        {
            if (tilemap.Width != MapSizeInTiles.Width || tilemap.Height != MapSizeInTiles.Height)
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
