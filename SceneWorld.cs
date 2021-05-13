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
        Animation a = new Animation(new KeyFrame[]
        {
            new KeyFrame(0, 0),
            new KeyFrame(360, 2f),
            new KeyFrame(100, 1f),
            new KeyFrame(100, 2f)
        });
        Animation b = new Animation(new KeyFrame[]
        {
            new KeyFrame(100, 0),
            new KeyFrame(200, 2f),
            new KeyFrame(100, 1f),
            new KeyFrame(100, 2f),
        });
        Animation c = new Animation(new KeyFrame[]
        {
            new KeyFrame(100, 0),
            new KeyFrame(200, 2f),
            new KeyFrame(300, 1f),
            new KeyFrame(300, 2f)
        });

        public SceneWorld()
        {
            bodies.Add(player);
            bodies.Add(new Platform(new Vector2(100, 400), new Vector2(300, 110)));
            camera.target = player.Body.position;
        }

        public void Update()
        {
            player.Update(bodies);
            camera.offset = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()) / 2 - player.Body.size / 2;
            camera.target = camera.target.MoveTowards(player.Body.position, Raylib.GetFrameTime() * 300, 1);

            a.Update();
            b.Update();
            c.Update();

        }

        public void Draw()
        {
            Raylib.BeginMode2D(camera);
            
            player.Draw();
            foreach (var body in bodies)
                body.Body.Draw(new Color(100, 100, 100, 100));

            //for (var i = 0; i < a.keyFrames.Length - 1; i++)
            //    Raylib.DrawLine((int)a.keyFrames[i].value, 300, (int)a.keyFrames[i + 1].value, 300, Color.RED);
            Raylib.DrawRectanglePro(new Rectangle(b.value, c.value, 50, 50), Vector2.Zero, a.value, Color.BLUE);

            Raylib.EndMode2D();
        }


    }
}
