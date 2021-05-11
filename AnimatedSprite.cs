using System;
using Raylib_cs;
using System.Numerics;

namespace Game
{
    class AnimatedSprite
    {
        Texture2D texture;
        int columnCount;
        int rowCount;
        public Vector2 position;
        public float speed;

        float timer = 0;
        int currentRow = 0;
        int currentColumn = 0;

        public AnimatedSprite(Texture2D texture, int columnCount, int rowCount, Vector2 position, float speed)
        {
            this.texture = texture;
            this.columnCount = columnCount;
            this.rowCount = rowCount;
            this.position = position;
            this.speed = speed;
        }

        public void Update()
        {
            timer += Raylib.GetFrameTime();
            if(timer > speed)
            {
                timer = 0;
                currentColumn += 1;
                if(currentColumn > columnCount - 1)
                {
                    currentColumn = 0;
                    currentRow += 1;
                    if (currentRow > rowCount - 1)
                        currentRow = 0;
                }
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
