using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.Actors
{
    public interface IClickableActor
    {
        void Clicked(GameTickArgs e, Scene s);
    }
}
