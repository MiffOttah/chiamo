using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Audio
{
    public abstract class MusicManager : IDisposable
    {

        public bool AddSong(string song, Stream mediaData)
        {
            return AddSong(song, StreamUnpacker.Unpack(mediaData));
        }

        public abstract bool AddSong(string song, byte[] mediaData);
        public abstract bool PlaySong(string song);
        public abstract string[] ListSongs();

        public abstract void Dispose();
    }
}
