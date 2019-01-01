using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo
{
    public class InputState : IAdvancedInput
    {
        public int MouseX { get; set; }
        public int MouseY { get; set; }
        public MouseButton MouseButton { get; set; }
        public JoyButton JoyButton { get; set; }

        internal IAdvancedInput States { get; set; }
        public InputButtonState this[MouseButton mouse] => States[mouse];
        public InputButtonState this[JoyButton joy] => States[joy];
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
        Action1 = 1 << 1,
        Action2 = 1 << 2,
        Menu = 1 << 3,
        Up = 1 << 4,
        Down = 1 << 5,
        Left = 1 << 6,
        Right = 1 << 7,
        Up2 = 1 << 8,
        Down2 = 1 << 9,
        Left2 = 1 << 10,
        Right2 = 1 << 11,
        Action3 = 1 << 12,
        Action4 = 1 << 13,
        ScrollMinus = 1 << 14,
        ScrollPlus = 1 << 15
    }
}
