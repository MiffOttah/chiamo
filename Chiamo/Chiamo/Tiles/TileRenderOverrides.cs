using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Tiles
{
    public class TileRenderOverrideCollection : ICollection<ITileRenderOverride>
    {
        private readonly Dictionary<byte, ITileRenderOverride> _Overrides = new Dictionary<byte, ITileRenderOverride>();

        public int Count => _Overrides.Count;

        public bool IsReadOnly => false;
        
        public void Add(ITileRenderOverride item)
        {
            _Overrides[item.TileId] = item;
        }

        public void Clear()
        {
            _Overrides.Clear();
        }

        public bool Contains(ITileRenderOverride item)
        {
            return _Overrides.Values.Contains(item);
        }

        public bool Contains(byte tileId)
        {
            return _Overrides.ContainsKey(tileId);
        }

        public void CopyTo(ITileRenderOverride[] array, int arrayIndex)
        {
            _Overrides.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ITileRenderOverride> GetEnumerator()
        {
            return _Overrides.Values.GetEnumerator();
        }

        public bool Remove(ITileRenderOverride item)
        {
            if (_Overrides.Values.Contains(item))
            {
                return _Overrides.Remove(item.TileId);
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Overrides.Values.GetEnumerator();
        }

        public ITileRenderOverride this[byte id]
        {
            get => _Overrides[id];
        }
    }

    public interface ITileRenderOverride
    {
        byte TileId { get; }
        void DrawTile(TileRenderArgs e);
    }
}
