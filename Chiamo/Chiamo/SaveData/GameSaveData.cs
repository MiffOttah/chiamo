using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.SaveData
{
    public abstract class GameSaveData
    {
        private readonly string _Id;
        public string GameId => _Id;
        public string SaveDirectory { get; protected set; }

        public GameSaveData(string gameId)
        {
            if (string.IsNullOrEmpty(gameId) || gameId.IndexOfAny(Path.GetInvalidFileNameChars()) != -1 || gameId.StartsWith(".") || gameId.EndsWith(".") || gameId.Contains(".."))
            {
                throw new ArgumentException("Invalid Game ID", "gameId");
            }
            
            _Id = GameId;
            SaveDirectory = gameId.Split('.').Aggregate(SavedGamesDirectoryFinder.FindSavedGamesDirectory(), Path.Combine);
        }
    }
}
