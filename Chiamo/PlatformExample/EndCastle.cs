﻿using MiffTheFox.Chiamo.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiffTheFox.Chiamo;

namespace PlatformExample
{
    public class EndCastle : PickUpActor
    {
        public EndCastle() : base(80, 80)
        {
        }

        public override void Draw(GameDrawArgs e)
        {
            DrawSprite(e, "EndCastle");
        }

        public override void OnPickUp(Scene s, Actor player)
        {
            int score = s is PxLevel ? ((PxLevel)s).Score : 0;

            s.Game.PopScene();
            s.Game.PushScene(new GameOverScreen(true, score));
        }
    }
}
