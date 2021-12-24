using System;
using Raylib_cs;
using System.Numerics;

namespace Game.Feature
{
    class AnimatedSprite
    {
        Texture2D texture;
        int columnCount;
        int rowCount;
        int spriteCount;
        public Vector2 position;
        public float frameSpeed;

        float frameTimer = 0;
        int currentRow = 0;
        int currentColumn = 0;

        public AnimatedSprite(Texture2D texture, int columnCount, int rowCount, int spriteCount, Vector2 position, float frameSpeed)
        {
            this.texture = texture;
            this.columnCount = columnCount;
            this.rowCount = rowCount;
            this.spriteCount = spriteCount;
            this.position = position;
            this.frameSpeed = frameSpeed;
        }

        public void Update()
        {
            frameTimer += Raylib.GetFrameTime();
            if(frameTimer > frameSpeed)
            {
                frameTimer = 0;
                currentColumn += 1;
                if(currentColumn > columnCount - 1)
                {
                    currentColumn = 0;
                    currentRow += 1;
                    if (currentRow > rowCount - 1)
                        currentRow = 0;
                }
            }

            if(currentColumn * rowCount + currentRow > spriteCount - 1)
            {
                currentColumn = 0;
                currentRow = 0;
            }
        }

        public void Draw()
        {
            var width = texture.width / columnCount;
            var height = texture.height / rowCount;
            Raylib.DrawTextureRec(texture, new Rectangle(width * currentColumn, height * currentRow, width, height), position, Color.WHITE);
        }
    }
}
