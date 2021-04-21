using System;
using Raylib_cs;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Raylib.InitWindow(1280, 720, "Game");
            Raylib.SetTargetFPS(60);
            var player = new Player();

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.WHITE);
                player.Update();
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
