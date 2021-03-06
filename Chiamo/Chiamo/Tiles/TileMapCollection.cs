﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Tiles
{
    public class TileMapCollection : ICollection<TileMap>
    {
        private List<TileMap> _Tmap = new List<TileMap>();

        public int Count { get { return _Tmap.Count; } }
        public bool IsReadOnly { get { return false; } }

        public void Add(TileMap item) => _Tmap.Add(item);
        public void Clear() => _Tmap.Clear();
        public bool Contains(TileMap item) => _Tmap.Contains(item);
        public void CopyTo(TileMap[] array, int arrayIndex) => _Tmap.CopyTo(array, arrayIndex);
        public IEnumerator<TileMap> GetEnumerator() => _Tmap.OrderBy(_ => _.ZIndex).GetEnumerator();
        public bool Remove(TileMap item) => _Tmap.Remove(item);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
