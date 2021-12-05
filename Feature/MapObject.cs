
namespace Game.Feature
{
    struct MapObject
    {
        public TileName Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public MapObject(TileName name, float x, float y)
        {
            Name = name;
            X = x;
            Y = y;
        }
    }
}
