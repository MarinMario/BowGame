using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Game
{
    public enum MOType { GrassTile, WallTile, House }
    static class Texture
    {
        public static Texture2D Player = Raylib.LoadTexture("Content/Player.png");
        public static Texture2D Sword = Raylib.LoadTexture("Content/Sword.png");
        public static Texture2D Bow = Raylib.LoadTexture("Content/Bow.png");
        public static Texture2D Arrow = Raylib.LoadTexture("Content/Arrow.png");


        public static Dictionary<MOType, Texture2D> MOTexture = new Dictionary<MOType, Texture2D>
        {
            { MOType.GrassTile, Raylib.LoadTexture("Content/TileTest.png") },
            { MOType.WallTile, Raylib.LoadTexture("Content/TileTest2.png") },
             { MOType.House, Raylib.LoadTexture("Content/House.png") }
            //{ "eh", Raylib.LoadTexture("Content/TileTest3.png") },
            //{ "blea", Raylib.LoadTexture("Content/TileTest4.png") },
        };
    }
}
