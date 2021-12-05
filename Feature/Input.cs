using Raylib_cs;

namespace Game.Feature
{
    class Input
    {
        public KeyboardKey Jump { get; set; }
        public KeyboardKey Left { get; set; }
        public KeyboardKey Right { get; set; }
        public KeyboardKey Pause { get; set; }

        public static Input Default()
        {
            var result = new Input();

            result.Jump = KeyboardKey.KEY_W;
            result.Left = KeyboardKey.KEY_A;
            result.Right = KeyboardKey.KEY_D;
            result.Pause = KeyboardKey.KEY_ESCAPE;

            return result;
        }
    }
}
