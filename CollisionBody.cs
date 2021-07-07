using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using System;

namespace Game
{
    class CollisionBody
    {
        public Vector2 position;
        public Vector2 size;

        public CollisionBody(Vector2 position, Vector2 size)
        {
            this.position = position; this.size = size;
        }

        public CollidingBody Collides(Vector2 velocity, List<IBody> bodies)
        {
            var returnVal = new CollidingBody();

            var bodyX = new CollisionBody(new Vector2(position.X + velocity.X, position.Y), size);
            foreach (var cBody in bodies)
                if (bodyX.Overlaps(cBody.Body) && cBody.Body != this)
                {
                    returnVal.bodies.Add(cBody);
                    //returnVal.direction.X = Math.Sign(velocity.X);
                    if (bodyX.position.X > position.X)
                        returnVal.direction.X = 1;
                    if (bodyX.position.X < position.X)
                        returnVal.direction.X = -1;

                }

            var bodyY = new CollisionBody(new Vector2(position.X, position.Y + velocity.Y), size);
            foreach (var cBody in bodies)
                if (bodyY.Overlaps(cBody.Body) && cBody.Body != this)
                {
                    returnVal.bodies.Add(cBody);
                    //returnVal.direction.Y = Math.Sign(velocity.Y);
                    if (bodyY.position.Y > position.Y)
                        returnVal.direction.Y = 1;
                    if (bodyY.position.Y < position.Y)
                        returnVal.direction.Y = -1;
                }

            return returnVal;
        }

        public bool Overlaps(CollisionBody body)
        {
            return
                position.X + size.X > body.position.X && 
                position.Y + size.Y > body.position.Y && 
                position.X < body.position.X + body.size.X &&
                position.Y < body.position.Y + body.size.Y;
        }

        public bool Contains(Vector2 point)
        {
            return
                position.X + size.X > point.X && position.X < point.X &&
                position.Y + size.Y > point.Y && position.Y < point.Y;
        }

        public void Draw(Color color)
        {
            Raylib.DrawRectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y, color);
        }
    }

    class CollidingBody
    {
        public Vector2 direction = Vector2.Zero;
        public List<IBody> bodies = new List<IBody>();
    }

    interface IBody
    {
        CollisionBody Body { get; set; }
    }

    class SimpleBody : IBody
    {
        public CollisionBody Body { get; set; }
        public SimpleBody(CollisionBody Body) { this.Body = Body; }
    }
}
