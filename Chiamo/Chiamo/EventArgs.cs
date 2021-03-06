﻿using MiffTheFox.Chiamo.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo
{
    public class GameTickArgs
    {
        public Game Game { get; set; }
        public InputState Input { get; set; }
    }

    public class GameDrawArgs
    {
        public Game Game { get; set; }
        public Canvas Canvas { get; protected set; }

        public GameDrawArgs(Canvas canvas)
        {
            Canvas = canvas;
        }
    }

    public class SceneChangeEventArgs : EventArgs
    {
        private readonly Scene _Scene;
        public Scene Scene => _Scene;

        public SceneChangeEventArgs(Scene scene)
        {
            _Scene = scene;
        }
    }
}
