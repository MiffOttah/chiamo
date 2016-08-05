using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo
{
    public abstract class Scene
    {
        public virtual bool IsTransparent { get; set; } = false;
        public virtual Game Game { get; set; }

        public ActorCollection Actors { get; private set; } = new ActorCollection();

        public event EventHandler Popped;

        public abstract void Initalize();

        public virtual void Tick(GameTickArgs e)
        {
            foreach (var actor in Actors)
            {
                actor.Tick(e);
            }
        }

        public virtual void Draw(GameDrawArgs e)
        {
            foreach (var actor in Actors)
            {
                actor.Draw(e);
            }
        }

        public virtual void OnPopped()
        {
            Popped?.Invoke(this, new EventArgs());
        }
    }
}