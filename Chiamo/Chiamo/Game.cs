using MiffTheFox.Chiamo.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiffTheFox.Chiamo
{
    public abstract class Game : IDisposable
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Title { get; private set; }

        public int TargetTimerSpeed { get; private set; }
        public bool ExitRequested { get; private set; }

        public SpriteManager Sprites { get; set; }
        public FontManager Fonts { get; set; }
        public Audio.MusicManager Music { get; set; }
        public Audio.SoundManager Sounds { get; set; }

        protected Stack<Scene> Scenes = new Stack<Scene>();

        private long _GameTime = 0;
        public long GameTime {  get { return _GameTime; } }

        public event EventHandler<SceneChangeEventArgs> SceneChanged;

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
            _GameTime++;
            if (_GameTime == long.MaxValue) _GameTime = 0;

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
            if (Fonts != null)
            {
                Sprites.Dispose();
            }
            if (Music != null)
            {
                Music.Dispose();
            }
        }

        public void PushScene(Scene scene)
        {
            Scenes.Push(scene);

            scene.Game = this;
            scene.Width = this.Width;
            scene.Height = this.Height;

            scene.Initalize();

            OnSceneChanged(new SceneChangeEventArgs(scene));
        }

        public void PopScene()
        {
            var s = Scenes.Pop();
            s.OnPopped();

            OnSceneChanged(new SceneChangeEventArgs(Scenes.FirstOrDefault()));
        }

        protected void OnSceneChanged(SceneChangeEventArgs e)
        {
            SceneChanged?.Invoke(this, e);
        }

        public void Exit()
        {
            ExitRequested = true;
        }
    }
}
