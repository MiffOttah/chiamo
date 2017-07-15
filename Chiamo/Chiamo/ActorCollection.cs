using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo
{
    public class ActorCollection : ICollection<Actor>
    {
        private readonly Dictionary<Guid, Actor> _Actors = new Dictionary<Guid, Actor>();

        public int Count { get { return _Actors.Count; } }
        public bool IsReadOnly { get { return false; } }

        public void Add(Actor item)
        {
            _Actors[item.Guid] = item;
        }

        public void Clear()
        {
            _Actors.Clear();
        }

        public bool Contains(Actor item)
        {
            return _Actors.ContainsValue(item);
        }
        public bool Contains(Guid item)
        {
            return _Actors.ContainsKey(item);
        }

        public void CopyTo(Actor[] array, int arrayIndex)
        {
            _Actors.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Actor> GetEnumerator()
        {
            return _Actors.Values.GetEnumerator();
        }

        public bool Remove(Actor item)
        {
            return Remove(item.Guid);
        }
        public bool Remove(Guid item)
        {
            if (_Actors.ContainsKey(item))
            {
                return _Actors.Remove(item);
            }
            else
            {
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public Actor this[Guid guid]
        {
            get
            {
                return _Actors[guid];
            }
        }
    }
}
