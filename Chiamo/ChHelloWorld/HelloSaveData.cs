using MiffTheFox.Chiamo.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChHelloWorld
{
    public class HelloSaveData : GameSaveData
    {
        public DateTime LastUpdateTime = DateTime.MinValue;
        public int SomeRandomInt { get; set; } = 0;

        public HelloSaveData() : base("MiffTheFox.ChHelloWorld")
        {
            SaveFileObfuscationKey = "FooBar";
        }
    }
}
