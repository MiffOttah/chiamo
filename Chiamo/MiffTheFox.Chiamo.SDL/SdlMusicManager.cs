using MiffTheFox.Chiamo.Audio;
using SdlDotNet.Audio;
using SdlDotNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.SDL
{
    public class SdlMusicManager : MusicManager
    {
        private Dictionary<string, Music> _Songs = new Dictionary<string, Music>();
        private string _CurrentMusicSelection = null;

        public void Initalize()
        {
            MusicPlayer.EnableMusicFinishedCallback();
            Events.MusicFinished += Events_MusicFinished;
        }

        private void Events_MusicFinished(object sender, MusicFinishedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_CurrentMusicSelection))
            {
                PlaySong(_CurrentMusicSelection);
            }
        }

        public override bool AddSong(string song, byte[] mediaData)
        {
            try
            {
                var sdlmus = new Music(mediaData);
                _Songs.Add(song, sdlmus);
                return true;
            }
            catch (Exception)
            {
                // Can't load the audio file
                return false;
            }
        }

        public override string[] ListSongs()
        {
            return _Songs.Keys.ToArray();
        }

        public override bool PlaySong(string song)
        {
            MusicPlayer.Stop();

            if (!string.IsNullOrEmpty(song) && _Songs.ContainsKey(song))
            {
                _Songs[song].Play();
                _CurrentMusicSelection = song;
                return true;
            }
            else
            {
                _CurrentMusicSelection = null;
                return false;
            }
        }

        public override void Dispose()
        {
            foreach (var v in _Songs.Values.ToArray()) v.Dispose();
            _Songs.Clear();
        }
    }
}
