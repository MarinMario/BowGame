using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace Game
{
    class SceneWorld : IScene
    {
        List<IBody> bodies = new List<IBody>();
        Player player = new Player();
        Camera2D camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1);
        LevelEditor levelEditor = new LevelEditor();

        public SceneWorld()
        {
            bodies.Add(player);
            bodies.Add(new Platform(new Vector2(100, 400), new Vector2(300, 110)));
            camera.target = player.Body.position;
        }

        public void Update()
        {
            player.Update(bodies, camera.target - camera.offset);
            camera.offset = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()) / 2 - player.Body.size / 2;
            camera.target = camera.target.MoveTowards(player.Body.position, Raylib.GetFrameTime() * 500, 1);

            levelEditor.Update();
        }

        public void Draw()
        {
            Raylib.BeginMode2D(camera);
            
            player.Draw();
            foreach (var body in bodies)
                body.Body.Draw(new Color(100, 100, 100, 100));

            Raylib.EndMode2D();

            levelEditor.Draw();
        }


    }
}
