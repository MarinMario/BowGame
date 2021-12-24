using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace Game.Feature
{
    struct Raycast
    {
        public Vector2 position;
        public float angle;
        public float maxLength;
        float length;

        public Raycast(Vector2 position, float angle, float maxLength)
        {
            this.position = position;
            this.angle = angle;
            this.maxLength = maxLength;
            length = 0;
        }

        public (Vector2 position, float length) Cast(List<IBody> bodies)
        {
            length = 0;
            while (length < maxLength)
            {
                foreach (var body in bodies)
                    if (body.Body.Contains(EndPosition(length)))
                        return (position: EndPosition(length), length: length);
                length += 50;
            }
            return (EndPosition(length), maxLength);
        }

        public void Draw()
        {
            Raylib.DrawLineEx(position, EndPosition(length), 2, Color.RED);
            Raylib.DrawCircleV(EndPosition(length), 10, Color.BLUE);
        }

        Vector2 EndPosition(float length)
        {
            return position + angle.ToVector() * length;
        }
    }
}
