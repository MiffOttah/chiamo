using System;

namespace MiffTheFox.Chiamo.MonoGame
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var launcher = new ChiamoMonoGameLauncher();
            launcher.Launch(new ChHelloWorld.HelloGame());
        }
    }
#endif
}
