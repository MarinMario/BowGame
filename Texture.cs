using System;
using Raylib_cs;
using System.Numerics;

namespace Game
{
    static class Texture
    {
        public static Texture2D Player = Raylib.LoadTexture("Content/Player.png");
        public static Texture2D Sword = Raylib.LoadTexture("Content/Sword.png");
        public static Texture2D Bow = Raylib.LoadTexture("Content/Bow.png");
        public static Texture2D Arrow = Raylib.LoadTexture("Content/Arrow.png");
    }
}
