using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.MonoGame
{
    public class ChiamoMonoGameLauncher : ChiamoFrontendLauncher
    {
        public ChiamoMonoGameLauncher() : base("MonoGame")
        {
        }

        public override void Launch(Game game)
        {
            using (var inst = new ChiamoMonoInstance(game))
            {
                inst.Run();
            }
        }
    }
}
