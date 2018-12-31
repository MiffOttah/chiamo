using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiamoLauncher
{
    public class LauncherConfigStatus : MiffTheFox.Chiamo.SaveData.GameSaveData
    {
        public LauncherConfigStatus() : base("ChiamoLauncher.LauncherConfiguration")
        {
        }

        public string LastGame { get; set; }
        public string LastFrontend { get; set; }
    }
}
