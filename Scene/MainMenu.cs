using Raylib_cs;
using Game.Engine;
using System.Numerics;
using System;

namespace Game.Scene
{
    class MainMenu : IScene
    {
        Button play = new Button(Vector2.Zero, Vector2.Zero, "Play", GuiStyle.Default());
        Button settings = new Button(Vector2.Zero, Vector2.Zero, "Settings", GuiStyle.Default());
        Camera2D camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1);
        public MainMenu()
        {

        }

        public void Update()
        {
            play.size = new Vector2(300, 100);
            settings.size = new Vector2(300, 100);

            var width = Raylib.GetScreenWidth();
            var height = Raylib.GetScreenHeight();

            play.position = -play.size / 2 + new Vector2(0, -60);
            settings.position = -settings.size / 2 + new Vector2(0, 60);
            camera.zoom = Util.ZoomToKeepRes(Program.res.x, Program.res.y);
            camera.offset = new Vector2(width, height) / 2;

            if (play.Active(camera.ScaledMousePosition()))
                Program.scene = new World();
        }

        public void Draw()
        {
            Raylib.BeginMode2D(camera);

            play.Draw(camera.ScaledMousePosition());
            settings.Draw(camera.ScaledMousePosition());

            Raylib.EndMode2D();
        }
    }
}
