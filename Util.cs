using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace Game
{
    static class Util
    {
        public static float MoveTowards(this float number, float target, float speed, float margin)
        {
            var dir = number != target ? (target - number) * speed / Math.Abs(target - number) : 0;
            if (Math.Abs(target - number) > margin)
                return number + dir;
            return number;
        }

        public static Vector2 MoveTowards(this Vector2 vector, Vector2 target, float speed, float margin)
        {
            var result = vector;
            var dir = target != vector ? Vector2.Normalize(target - vector) * speed : Vector2.Zero;
            
            if (Math.Abs(target.X - vector.X) > margin)
                result.X += dir.X;
            if (Math.Abs(target.Y - vector.Y) > margin)
                result.Y += dir.Y;

            return result;
        }

        public static float ToAngle(this Vector2 vector)
        {
            return MathF.Atan2(vector.Y, vector.X) * 57.295f;
        }

        public static Vector2 ToVector(this float angle)
        {
            angle /= 57.295f;
            return new Vector2(MathF.Sin(angle), MathF.Cos(angle));
        }
    }

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
            while(length < maxLength)
            {
                foreach (var body in bodies)
                    if (body.Body.Contains(EndPosition(length)))
                        return (position: EndPosition(length), length: length);
                length += 10;
            }
            return (EndPosition(length), maxLength);
        }

        public void Draw()
        {
            Raylib.DrawLineV(position, EndPosition(length), Color.RED);
        }

        Vector2 EndPosition(float length)
        {
            return position + angle.ToVector() * length;
        }
    }
}
