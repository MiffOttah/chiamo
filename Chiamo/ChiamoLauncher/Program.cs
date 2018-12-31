using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiffTheFox.Chiamo;

namespace ChiamoLauncher
{
    static class Program
    {
        public static ChiamoFrontendLauncher Frontend { get; internal set; }
        public static Game Game { get; internal set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            if (Frontend != null && Game != null)
            {
                Frontend.Launch(Game);
            }
        }
    }
}
