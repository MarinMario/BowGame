using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Game
{
    //public enum MOType { GrassTile, WallTile, House, Monster }
    public enum TileName {  Earth1, Earth2, Earth3 }
    static class Texture
    {
        public static Texture2D Player = Raylib.LoadTexture("Content/Player.png");
        public static Texture2D Sword = Raylib.LoadTexture("Content/Sword.png");
        public static Texture2D Bow = Raylib.LoadTexture("Content/Bow.png");
        public static Texture2D Arrow = Raylib.LoadTexture("Content/Arrow.png");

        public static Texture2D BatHead = Raylib.LoadTexture("Content/BatHead.png");
        public static Texture2D BatHeadDead= Raylib.LoadTexture("Content/BatHeadDead.png");
        public static Texture2D BatWing = Raylib.LoadTexture("Content/BatWing.png");

        public static Dictionary<TileName, TTT> Tile = new Dictionary<TileName, TTT>
        {
            { TileName.Earth1, new TTT(TileType.Collision, Raylib.LoadTexture("Content/Tile/Earth1.png")) },
            { TileName.Earth2, new TTT(TileType.Decor, Raylib.LoadTexture("Content/Tile/Earth2.png")) },
            { TileName.Earth3, new TTT(TileType.Decor, Raylib.LoadTexture("Content/Tile/Earth3.png")) },

        };
    }

    //TTT = TypeTextureTuple
    struct TTT
    {
        public TileType type;
        public Texture2D texture;
        public TTT(TileType type, Texture2D texture)
        {
            this.type = type;
            this.texture = texture;
        }
    }
}
