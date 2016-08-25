using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Audio
{
    public abstract class SoundManager : IDisposable
    {
        public bool AddSound(string sound, Stream mediaData)
        {
            return AddSound(sound, StreamUnpacker.Unpack(mediaData));
        }

        public abstract bool AddSound(string sound, byte[] mediaData);
        public abstract bool PlaySound(string sound);
        public abstract string[] ListSounds();

        public abstract void Dispose();
    }
}
