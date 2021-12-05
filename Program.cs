using System;
using Raylib_cs;
using System.Numerics;

namespace Game
{
    class Program
    {
        public static Feature.IScene scene;
        public static (int x, int y) res = (1920, 1080);
        static void Main(string[] args)
        {
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
