using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MiffTheFox.Chiamo.SaveData
{
    public abstract class GameSaveData
    {
        private string _savedGameId;
        private SaveFileObfuscation _obfuscation;

        [JsonIgnore] public string GameId { get; }
        [JsonIgnore] public string SaveDirectory { get; protected set; }

        [JsonIgnore]
        public string SavedGameId
        {
            get => _savedGameId;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _savedGameId = "_default";
                }
                else
                {
                    //var invalid = Path.GetInvalidFileNameChars();
                    const string invalid = ":;<>?@\\^`|";
                    var sb = new StringBuilder();
                    foreach (char c in value.ToLowerInvariant())
                    {
                        if (c >= '0' && invalid.IndexOf(c) == -1) sb.Append(c);
                    }
                    _savedGameId = sb.ToString();
                }
            }
        }

        [JsonIgnore]
        public string SaveFileObfuscationKey
        {
            get => _obfuscation == null ? _obfuscation.KeyAsString : null;
            set => _obfuscation = string.IsNullOrEmpty(value) ? null : new SaveFileObfuscation(value);
        }


        public GameSaveData(string gameId)
        {
            if (string.IsNullOrEmpty(gameId) || gameId.IndexOfAny(Path.GetInvalidFileNameChars()) != -1 || gameId.StartsWith(".") || gameId.EndsWith(".") || gameId.Contains(".."))
            {
                throw new ArgumentException("Invalid Game ID", "gameId");
            }

            GameId = gameId;
            SaveDirectory = gameId.Split('.').Aggregate(SavedGamesDirectoryFinder.FindSavedGamesDirectory(), Path.Combine);
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(this);

            string saveDataFile = Path.Combine(SaveDirectory, SavedGameId + ".sav");

            if (Directory.Exists(SaveDirectory))
            {
                string backupSaveDataFile = Path.ChangeExtension(saveDataFile, ".bak");
                if (File.Exists(backupSaveDataFile)) File.Delete(backupSaveDataFile);
                if (File.Exists(saveDataFile)) File.Move(saveDataFile, backupSaveDataFile);
            }
            else
            {
                Directory.CreateDirectory(SaveDirectory);
            }

            if (_obfuscation != null)
            {
                string obfuscatedJson = _obfuscation.Obfuscate(json);
                File.WriteAllText(saveDataFile, "oj:" + obfuscatedJson, Encoding.UTF8);
            }
            else
            {
                File.WriteAllText(saveDataFile, "js:" + json, Encoding.UTF8);
            }
        }
        public void Save(string gameId)
        {
            SavedGameId = gameId;
            Save();
        }

        public bool Load()
        {
            string saveDataFile = Path.Combine(SaveDirectory, SavedGameId + ".sav");
            if (File.Exists(saveDataFile))
            {
                string data = File.ReadAllText(saveDataFile);
                if (data.Length > 3)
                {
                    string prefix = data.Remove(3);
                    data = data.Substring(3);

                    switch (prefix)
                    {
                        case "js:":
                            JsonConvert.PopulateObject(data, this);
                            return true;

                        case "oj:":
                            if (_obfuscation != null && _obfuscation.TryDeobfuscate(data, out string plaintext))
                            {
                                JsonConvert.PopulateObject(plaintext, this);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                    }
                }
            }
            return false;
        }
        public bool Load(string gameId)
        {
            SavedGameId = gameId;
            return Load();
        }

        public string[] GetSaves()
        {
            return Directory.GetFiles(SaveDirectory, "*.sav").Select(f => Path.GetFileNameWithoutExtension(f).ToLowerInvariant()).ToArray();
        }
    }
}
