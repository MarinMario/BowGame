using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace Game
{
    static class Util
    {
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

    }
}
