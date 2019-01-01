namespace MiffTheFox.Chiamo
{
    public class AdvancedInputController : IAdvancedInput
    {
        private readonly InputButtonState[] _States = new InputButtonState[32];

        public void Update(MouseButton mouse, JoyButton joy)
        {
            int ijoy = (int)joy;

            for (int i = 0; i < 30; i++)
            {
                int bitmask = 1 << i;
                _UpdateStates(i, (ijoy & bitmask) != 0);
            }

            _UpdateStates(30, ((mouse & MouseButton.Left) != 0));
            _UpdateStates(31, ((mouse & MouseButton.Right) != 0));
        }

        public InputButtonState this[MouseButton mouse]
        {
            get
            {
                return _States[(mouse == MouseButton.Right) ? 31 : 30];
            }
        }

        public InputButtonState this[JoyButton joy]
        {
            get
            {
                int ijoy = (int)joy;

                for (int i = 0; i < 30; i++)
                {
                    int bitmask = 1 << i;
                    if ((ijoy & bitmask) != 0) return _States[i];
                }

                return InputButtonState.Released;
            }
        }

        private void _UpdateStates(int i, bool condition)
        {
            if (condition)
            {
                _States[i] = _States[i] == InputButtonState.Released ? InputButtonState.Rising : InputButtonState.Held;
            }
            else
            {
                _States[i] = _States[i] == InputButtonState.Held ? InputButtonState.Falling : InputButtonState.Released;
            }
        }
    }

    public interface IAdvancedInput
    {
        InputButtonState this[MouseButton mouse] { get; }
        InputButtonState this[JoyButton joy] { get; }
    }

    public enum InputButtonState : byte
    {
        Released = 0,
        Rising = 1,
        Held = 2,
        Falling = 3
    }
}
