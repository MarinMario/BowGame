using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using System.Numerics;


namespace Game
{
    class Player : IBody
    {
        float speed = 300;
        float gravity = 980;
        Vector2 velocity = Vector2.Zero;
        float jumpForce = 700;
        int maxJumps = 9999;
        int currentJumps = 0;
        public CollisionBody Body { get; set; }
        AnimatedSprite sprite;
        Texture2D sword;
        Vector2 swordPos = Vector2.Zero;

        public Player()
        {
            var t = Raylib.LoadTexture("Content/Player.png");
            sprite = new AnimatedSprite(t, 2, 2, 4, Vector2.Zero, 1f);
            sword = Raylib.LoadTexture("Content/Sword.png");

            Body = new CollisionBody(Vector2.One * 100, new Vector2(64, 64));

        }

        public void Update(List<IBody> bodies)
        {
            PlatformerMovement(bodies);
            sprite.position = Body.position;
            sprite.Update();
        }

        public void Draw()
        {
            sprite.Draw();
            Raylib.DrawTextureEx(sword, Body.position + swordPos, 0, 1, Color.WHITE);
        }

        public void PlatformerMovement(List<IBody> bodies)
        {
            var delta = Raylib.GetFrameTime();
            if (delta > 0.25) return;

            var xdir = 0;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
                xdir = 1;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
                xdir = -1;

            velocity.X = xdir * speed;
            velocity.Y += gravity * delta;
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_W) && currentJumps < maxJumps)
            {
                velocity.Y = -jumpForce;
                currentJumps += 1;
            }
            velocity.Y = Math.Clamp(velocity.Y, -1000, 1000);

            var nextVel = velocity * delta;
            var collision = Body.Collides(nextVel, bodies);
            
            if (collision.direction.X != 0)
            {
                nextVel.X = 0;
            }
            if (collision.direction.Y != 0)
            {
                nextVel.Y = 0;
                velocity.Y = 0;
            }
            Body.position += nextVel;

            if (collision.direction.Y == 1)
            {
                velocity.Y = 0;
                currentJumps = 0;
            }

        }

        public void TopDownMovement(List<IBody> bodies)
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
            if (dir != Vector2.Zero)
                dir = Vector2.Normalize(dir);

            velocity = dir * speed * Raylib.GetFrameTime();
            var collision = Body.Collides(velocity, bodies);

            if (collision.direction.X == 0)
                Body.position.X += velocity.X;
            if (collision.direction.Y == 0)
                Body.position.Y += velocity.Y;
            if (collision.bodies.Count != 0)
                Raylib.DrawRectangle((int)Body.position.X, (int)Body.position.Y, (int)Body.size.X, (int)Body.size.Y, Color.BLACK);
        }
    }
}
