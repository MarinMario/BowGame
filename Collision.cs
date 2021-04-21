using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
namespace Game
{
    class CollisionBox
    {
        public Vector2 position;
        public Vector2 size;

        public CollisionBox(Vector2 position, Vector2 size)
        {
            this.position = position; this.size = size;
        }

        public void Move(Vector2 velocity, List<CollisionBox> boxes)
        {
            position.X += velocity.X;
            foreach (var box in boxes)
                if (Collides(box)) position.X -= velocity.X;

            position.Y += velocity.Y;
            foreach (var box in boxes)
                if (Collides(box)) position.Y -= velocity.Y;

        }

        public bool Collides(CollisionBox box)
        {
            return
                position.X + size.X > box.position.X && position.X < box.position.X + box.size.X &&
                position.Y + size.Y > box.position.Y && position.Y < box.position.Y + box.size.Y;
        }

        public void Draw(Color color)
        {
            Raylib.DrawRectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y, color);
        }
    }
}
