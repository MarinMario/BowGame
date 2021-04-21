using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using System.Numerics;


namespace Game
{
    class Player
    {
        public float speed = 300;
        Vector2 position = new Vector2(300, 300);
        CollisionBox box = new CollisionBox(Vector2.Zero, Vector2.One * 100);
        List<CollisionBox> testBox = new List<CollisionBox>
        {
            new CollisionBox(Vector2.One * 200, Vector2.One * 200),
            new CollisionBox(new Vector2(700, 300), Vector2.One * 100),
            new CollisionBox(new Vector2(700, 400), Vector2.One * 100),
            new CollisionBox(new Vector2(700, 500), Vector2.One * 100)
        };
        public void Update()
        {
            var dir = Vector2.Zero;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
                dir.X = 1;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
                dir.X = -1;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
                dir.Y = -1;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
                dir.Y = 1;
            if(dir != Vector2.Zero)
                dir = Vector2.Normalize(dir);

            var velocity = dir * speed * Raylib.GetFrameTime();
            box.Move(velocity, testBox);
            box.Draw(Color.BLUE);
            foreach (var box in testBox)
                box.Draw(Color.RED);
            //Raylib.DrawRectangle((int)position.X, (int)position.Y, 50, 50, Color.BLACK);
        }
    }
}
