using System;
using Raylib_cs;
using System.Numerics;

namespace Game
{
    class Program
    {
        public static Feature.IScene scene;
        public static (int x, int y) res = (1920, 1080);
        public static Feature.Input input;
        public static string keybindingsPath = "keybindings.json";
        static void Main(string[] args)
        {
            input = Feature.Util.Load<Feature.Input>(keybindingsPath);
            if (input == null)
                input = Feature.Input.Default();
            

            Raylib.InitWindow(1280, 720, "Game");
            Raylib.SetWindowState(ConfigFlag.FLAG_WINDOW_RESIZABLE);
            Raylib.SetExitKey(KeyboardKey.KEY_END);
            scene = new Scene.MainMenu();

            while (!Raylib.WindowShouldClose())
            {
                var delta = Raylib.GetFrameTime();
                if(delta != 0 && delta < 0.25)
                    scene.Update();
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.WHITE);
                scene.Draw();
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
