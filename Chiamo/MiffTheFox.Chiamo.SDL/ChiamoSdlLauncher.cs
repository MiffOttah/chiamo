namespace MiffTheFox.Chiamo.SDL
{
    public class ChiamoSdlLauncher : ChiamoFrontendLauncher
    {
        public ChiamoSdlLauncher() : base("SDL.NET")
        {
        }

        public override void Launch(Game game)
        {
            var chsdlinst = new ChiamoSdlInstance(game);
            chsdlinst.Main();
        }
    }
}