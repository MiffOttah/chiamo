using MiffTheFox.Chiamo;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChiamoLauncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            btnLaunch.Font = new Font(btnLaunch.Font, FontStyle.Bold);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AddGame(typeof(ChHelloWorld.HelloGame));
            AddGame(typeof(PlatformExample.PxGame));

            AddFrontend(new MiffTheFox.Chiamo.MonoGame.ChiamoMonoGameLauncher());
            AddFrontend(new MiffTheFox.Chiamo.SDL.ChiamoSdlLauncher());
        }

        private void AddFrontend(ChiamoFrontendLauncher launcher)
        {
            cbFrontend.Items.Add(launcher);

            if (cbFrontend.Items.Count == 1)
            {
                cbFrontend.SelectedIndex = 0;
                cbFrontend.Enabled = false;
            }
            else
            {
                cbFrontend.Enabled = true;
            }
        }

        private void AddGame(Type type)
        {
            cbGame.Items.Add(new GameRef(type));

            if (cbGame.Items.Count == 1)
            {
                cbGame.SelectedIndex = 0;
                cbGame.Enabled = false;
            }
            else
            {
                cbGame.Enabled = true;
            }
        }

        private struct GameRef
        {
            private Type _GameType;

            public GameRef(Type type)
            {
                this._GameType = type;
            }

            public override string ToString() => _GameType.FullName;

            public Game CreateGame()
            {
                var ci = _GameType.GetConstructor(Type.EmptyTypes);
                return ci != null ? ci.Invoke(new object[0]) as Game : null;
            }
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            var frontend = (ChiamoFrontendLauncher)cbFrontend.SelectedItem;
            var gameRef = (GameRef)cbGame.SelectedItem;

            var game = gameRef.CreateGame();
            if (game != null)
            {
                Program.Frontend = frontend;
                Program.Game = game;
                Close();
            }
            else
            {
                MessageBox.Show(this, "Could not create an instance of the selected game.", "Chiamo Launcher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
