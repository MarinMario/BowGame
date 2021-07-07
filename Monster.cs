using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;


namespace Game
{
    class Monster : IBody
    {
        public CollisionBody Body { get; set; }
        float speed = 300;
        float gravity = 980;
        Vector2 velocity = Vector2.Zero;
        float jumpForce = 700;

        public Monster(Vector2 position)
        {
            Body = new CollisionBody(position, Vector2.One * 100);
        }

        public void Update(List<IBody> bodies)
        {
            var delta = Raylib.GetFrameTime();
            velocity.Y += gravity * delta;
            velocity.Y = Math.Clamp(velocity.Y, -1000, 1000);

            var nextVel = new Vector2(velocity.X, velocity.Y * delta);
            var collision = Body.Collides(nextVel, bodies);
            if (collision.direction.X != 0)
                nextVel.X = 0;
            if (collision.direction.Y != 0)
            {
                nextVel.Y = 0;
                velocity.Y = 0;
            }

            Body.position += nextVel;

        }

        public void Draw()
        {
            Raylib.DrawTextureEx(Texture.GrassTile, Body.position, 0, 1, Color.WHITE);
        }
    }
}
