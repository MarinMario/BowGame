using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;
using System;
using Game.Engine;

namespace Game.Feature
{
    class Platform : IBody
    {
        public CollisionBody Body { get; set; }

        public Platform(Vector2 position, Vector2 size)
        {
            Body = new CollisionBody(position, size);
        }

    }

    class MovingPlatform : IBody
    {
        public CollisionBody Body { get; set; }
        public Vector2 position1;
        public Vector2 position2;
        public float speed;
        public Vector2 velocity = Vector2.Zero;
        Vector2 targetPosition;

        public MovingPlatform(Vector2 position1, Vector2 position2, Vector2 size, float speed)
        {
            this.position1 = position1;
            this.position2 = position2;
            this.targetPosition = position2;
            this.speed = speed;
            Body = new CollisionBody(position1, size);
        }

        public void Update(List<IBody> bodies)
        {
            var dir = targetPosition - Body.position;
            if (MathF.Abs(dir.X) < 1 && MathF.Abs(dir.Y) < 1)
                targetPosition = targetPosition == position1 ? position2 : position1;

            if (dir != Vector2.Zero)
                dir = Vector2.Normalize(dir);

            velocity = dir * speed * Raylib.GetFrameTime();
            Body.position += velocity;
        }

    }
}
