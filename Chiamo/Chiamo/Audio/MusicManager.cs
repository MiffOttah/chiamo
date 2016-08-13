using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Audio
{
    public abstract class MusicManager : IDisposable
    {
        public abstract bool AddSong(string song, byte[] mediaData);
        public abstract bool PlaySong(string song);
        public abstract string[] ListSongs();

        public abstract void Dispose();
    }
}
