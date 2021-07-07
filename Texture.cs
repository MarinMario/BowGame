using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Game
{
    //public enum MOType { GrassTile, WallTile, House, Monster }
    public enum MOName { Grass, Wall, House, Monster }
    static class Texture
    {
        public static Texture2D Player = Raylib.LoadTexture("Content/Player.png");
        public static Texture2D Sword = Raylib.LoadTexture("Content/Sword.png");
        public static Texture2D Bow = Raylib.LoadTexture("Content/Bow.png");
        public static Texture2D Arrow = Raylib.LoadTexture("Content/Arrow.png");


        public static Texture2D GrassTile = Raylib.LoadTexture("Content/TileTest.png");
        public static Texture2D WallTile = Raylib.LoadTexture("Content/TileTest2.png");
        public static Texture2D House = Raylib.LoadTexture("Content/House.png");

        public static Dictionary<MOName, TTT> MOTexture = new Dictionary<MOName, TTT>
        {
            { MOName.Grass, new TTT(MOType.CollisionTile, Raylib.LoadTexture("Content/TileTest.png")) },
            { MOName.Wall, new TTT(MOType.CollisionTile, Raylib.LoadTexture("Content/TileTest2.png")) },
            { MOName.House, new TTT (MOType.Object, Raylib.LoadTexture("Content/House.png")) },
            { MOName.Monster, new TTT(MOType.Monster, Player) }
            //{ "eh", Raylib.LoadTexture("Content/TileTest3.png") },
            //{ "blea", Raylib.LoadTexture("Content/TileTest4.png") },
        };
    }

    //TTT = TypeTextureTuple
    struct TTT
    {
        public MOType type;
        public Texture2D texture;
        public TTT(MOType type, Texture2D texture)
        {
            this.type = type;
            this.texture = texture;
        }
    }
}
