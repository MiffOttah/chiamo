using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.SaveData
{
    internal static class SavedGamesDirectoryFinder
    {
        [DllImport("shell32.dll")]
        static extern int SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
            uint dwFlags,
            IntPtr hToken,
            out IntPtr pszPath  // API uses CoTaskMemAlloc
        );

        private static readonly Guid _SavedGamesKnownFolderId = new Guid("4C5C32FF-BB9D-43b0-B5B4-2D72E54EAAA4");

        internal static string FindSavedGamesDirectory()
        {
            string chiamoSavedDirectory = Environment.GetEnvironmentVariable("CHIAMO_SAVE_DIRECTORY");
            if (!string.IsNullOrEmpty(chiamoSavedDirectory)) return chiamoSavedDirectory;

            switch (Environment.OSVersion.Platform)
            {
                // Use a proper .folder on 'nix systems.
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                    string home = Environment.GetEnvironmentVariable("HOME");
                    if (string.IsNullOrEmpty(home)) return "./chiamo-saved-games";
                    return Path.Combine(home, ".chiamo-saved-games");

                // Try to get the NT 6.0 "Saved Games" folder, but fallback in case of failure.
                case PlatformID.Win32NT:
                    try
                    {
                        if (SHGetKnownFolderPath(_SavedGamesKnownFolderId, 0, IntPtr.Zero, out IntPtr pszPath) == 0)
                        {
                            string savedGamesPath = Marshal.PtrToStringUni(pszPath);
                            Marshal.FreeCoTaskMem(pszPath);
                            return savedGamesPath;
                        }
                    }
                    catch
                    {
                        // In case of error, just continue through and use the fallback.
                    }
                    break;
            }

            // Fallback default path.
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "My Games");
        }
    }
}
