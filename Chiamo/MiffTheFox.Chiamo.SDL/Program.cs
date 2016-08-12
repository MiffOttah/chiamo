using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.SDL
{
    class Program
    {
        static void Main(string[] args)
        {
            var fl = new ChiamoSdlLauncher();
            fl.Launch(new PlatformExample.PxGame());
        }
    }
}
