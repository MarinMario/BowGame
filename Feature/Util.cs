using System;
using System.Numerics;
using Raylib_cs;
using System.Text.Json;


namespace Game.Feature
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

        public static Vector2 ScaledMousePosition(this Camera2D camera)
        {
            return camera.target + (Raylib.GetMousePosition() - camera.offset) / camera.zoom;
        }

        public static float ZoomToKeepRes(int resx, int resy)
        {
            var width = Raylib.GetScreenWidth();
            var height = Raylib.GetScreenHeight();
            var zoom = 1f;
            if (width < height)
                zoom = (float)width / (float)resx;
            else
                zoom = (float)height / (float)resy;

            return zoom;
        }

        public static void Save<T>(string fileName, T data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            System.IO.File.WriteAllText($"{fileName}", JsonSerializer.Serialize(data, options));
        }

        public static T Load<T>(string fileName)
        {
            var result = default(T);
            try
            {
                var file = System.IO.File.ReadAllText($"{fileName}");
                result = JsonSerializer.Deserialize<T>(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }
    }
}
