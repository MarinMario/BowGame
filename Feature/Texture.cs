using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Game.Feature
{
    static class Texture
    {
        public static Texture2D Player = Raylib.LoadTexture("Content/Player.png");
        public static Texture2D Sword = Raylib.LoadTexture("Content/Sword.png");
        public static Texture2D Bow = Raylib.LoadTexture("Content/Bow.png");
        public static Texture2D Arrow = Raylib.LoadTexture("Content/Arrow.png");

        public static Texture2D BatHead = Raylib.LoadTexture("Content/BatHead.png");
        public static Texture2D BatHeadDead = Raylib.LoadTexture("Content/BatHeadDead.png");
        public static Texture2D BatWing = Raylib.LoadTexture("Content/BatWing.png");

        public static Texture2D Earth1 = Raylib.LoadTexture("Content/Tile/Earth1.png");
        public static Texture2D Earth2 = Raylib.LoadTexture("Content/Tile/Earth2.png");
        public static Texture2D Earth3 = Raylib.LoadTexture("Content/Tile/Earth3.png");
    }
}
