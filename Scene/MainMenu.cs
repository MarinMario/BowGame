using Raylib_cs;
using Game.Feature;
using System.Numerics;

namespace Game.Scene
{
    class MainMenu : Feature.IScene
    {
        Button play = new Button(Vector2.Zero, new Vector2(300, 100), "Play", GuiStyle.Default());
        Button settings = new Button(Vector2.Zero, new Vector2(300, 100), "Settings", GuiStyle.Default());
        Feature.Transition transition = new Feature.Transition();

        public MainMenu()
        {

        }

        public void Update()
        {
            var width = Raylib.GetScreenWidth();
            var height = Raylib.GetScreenHeight();
            var size = new Vector2(width, height);

            play.position = size / 2 - play.size / 2 + new Vector2(0, -60);
            settings.position = size / 2 - settings.size / 2 + new Vector2(0, 60);

            var mousePos = Raylib.GetMousePosition();
            if (play.Active(mousePos))
                transition.FadeOut(new Scene.World());

            if (settings.Active(mousePos))
                transition.FadeOut(new Scene.Settings());

            transition.Update();
        }

        public void Draw()
        {
            var mousePos = Raylib.GetMousePosition();
            play.Draw(mousePos);
            settings.Draw(mousePos);


            transition.Draw();
        }
    }
}
