using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Game.Feature
{
    enum TileType { Decor, Collision }
    enum TileName { Earth1, Earth2, Earth3 }
    static class Tile
    {
        public static Dictionary<TileName, TTT> T = new Dictionary<TileName, TTT>
        {
            { TileName.Earth1, new TTT(TileType.Collision, Texture.Earth1) },
            { TileName.Earth2, new TTT(TileType.Decor, Texture.Earth2) },
            { TileName.Earth3, new TTT(TileType.Decor, Texture.Earth3) },
        };

        //TTT = TyleTypeTuple
        public struct TTT
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
}
