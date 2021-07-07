using System;
using System.Collections.Generic;
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
        public Bow bow = new Bow(Vector2.Zero, 0);

        public Player()
        {
            sprite = new AnimatedSprite(Texture.Player, 2, 2, 4, Vector2.Zero, 0.2f);
            Body = new CollisionBody(new Vector2(200, -400), new Vector2(64, 64));
        }

        public void Update(List<IBody> bodies, Vector2 cameraPos)
        {
            PlatformerMovement(bodies);
            
            sprite.position = Body.position;
            //sprite.Update();

            var bowOffset = new Vector2(Texture.Bow.width, Texture.Bow.height) / 2;
            bow.position = Body.position + bowOffset;
            bow.rotation = (cameraPos + Raylib.GetMousePosition() - bow.position).ToAngle();
            bow.Update(Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON));
        }

        public void Draw()
        {
            sprite.Draw();
            bow.Draw();
        }

        public void PlatformerMovement(List<IBody> bodies)
        {
            var delta = Raylib.GetFrameTime();

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
        }
    }
}
