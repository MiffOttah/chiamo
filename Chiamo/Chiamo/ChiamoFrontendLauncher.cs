using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo
{
    public abstract class ChiamoFrontendLauncher
    {
        public string Name { get; private set; }
        public abstract void Launch(Game game);
        
        protected ChiamoFrontendLauncher(string name)
        {
            Name = name;
        }
    }
}
