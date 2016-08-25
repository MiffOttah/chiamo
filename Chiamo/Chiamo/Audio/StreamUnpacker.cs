using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Audio
{
    internal static class StreamUnpacker
    {
        internal static byte[] Unpack(Stream mediaData)
        {
            if (mediaData.Length > int.MaxValue) throw new InvalidOperationException("Audio data is too long!");

            var data = new byte[mediaData.Length];
            mediaData.Read(data, 0, data.Length);

            return data;
        }
    }
}
