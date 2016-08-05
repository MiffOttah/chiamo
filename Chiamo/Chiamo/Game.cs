﻿using MiffTheFox.Chiamo.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo
{
    public abstract class Game : IDisposable
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Title { get; private set; }

        public int TargetTimerSpeed { get; private set; }

        public SpriteManager Sprites { get; set; }

        protected Stack<Scene> Scenes = new Stack<Scene>();

        protected Game(int width, int height, string title, int speed = 66)
        {
            this.Width = width;
            this.Height = height;
            this.Title = title;
            this.TargetTimerSpeed = speed;
        }

        public abstract void Initalize();

        public virtual void Tick(GameTickArgs e)
        {
            e.Game = this;

            if (Scenes.Count > 0)
            {
                Scenes.Peek().Tick(e);
            }
        }

        public virtual void Draw(GameDrawArgs e)
        {
            e.Game = this;

            foreach (var sc in Scenes)
            {
                sc.Draw(e);
                if (!sc.IsTransparent) break;
            }
        }

        public void Dispose()
        {
            if (Sprites != null)
            {
                Sprites.Dispose();
            }
        }

        public void PushScene(Scene scene)
        {
            Scenes.Push(scene);

            scene.Game = this;
            scene.Initalize();
        }

        public void PopScene()
        {
            var s = Scenes.Pop();
            s.OnPopped();
        }
    }
}
