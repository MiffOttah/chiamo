using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo
{
    public class InputState
    {
        public int MouseX { get; set; }
        public int MouseY { get; set; }
        public MouseButton MouseButton { get; set; }
        public JoyButton JoyButton { get; set; }
    }

    [Flags]
    public enum MouseButton
    {
        None = 0,
        Left = 1,
        Right = 2
    }

    [Flags]
    public enum JoyButton
    {
        None = 0,
        Jump = 1,
        Action1 = 2,
        Action2 = 4,
        Menu = 8,
        Up = 16,
        Down = 32,
        Left = 64,
        Right = 128
    }
}
