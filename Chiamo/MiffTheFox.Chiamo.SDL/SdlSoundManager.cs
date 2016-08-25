using MiffTheFox.Chiamo.Audio;
using SdlDotNet.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.SDL
{
    public class SdlSoundManager : SoundManager
    {
        private Dictionary<string, Sound> _Sounds = new Dictionary<string, Sound>();

        public override bool AddSound(string sound, byte[] mediaData)
        {
            try
            {
                var sdlmus = new Sound(mediaData);
                _Sounds.Add(sound, sdlmus);
                return true;
            }
            catch (Exception)
            {
                // Can't load the audio file
                return false;
            }
        }

        public override string[] ListSounds()
        {
            return _Sounds.Keys.ToArray();
        }

        public override bool PlaySound(string sound)
        {
            if (!string.IsNullOrEmpty(sound) && _Sounds.ContainsKey(sound))
            {
                _Sounds[sound].Play();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Dispose()
        {

        }
    }
}
