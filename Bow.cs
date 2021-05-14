﻿using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;

namespace Game
{
    class Bow
    {
        public Vector2 position;
        public float rotation;
        List<Arrow> arrows = new List<Arrow>();
        Arrow currentArrow;
        float speedTimer = 0;

        public Bow(Vector2 position, float rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public void Update(bool charge)
        {
            var delta = Raylib.GetFrameTime();

            foreach (var arrow in arrows)
                arrow.Update();

            if (charge)
            {
                speedTimer += delta;
                speedTimer = Math.Clamp(speedTimer, 0, 1);
                var r = (MathF.PI / 2 - rotation).ToVector();
                currentArrow = new Arrow(position + r * 50 - r * 25 * speedTimer, rotation, speedTimer * 2000);
            }
            else if (currentArrow != null)
            {
                arrows.Add(currentArrow);
                currentArrow = null;
                speedTimer = 0;
            }
        }

        public void Draw()
        {
            Raylib.DrawTexturePro(
                Texture.Bow,
                new Rectangle(0, 0, Texture.Bow.width, Texture.Bow.height),
                new Rectangle(position.X, position.Y, Texture.Bow.width, Texture.Bow.height),
                new Vector2(0, Texture.Bow.height / 2), rotation * 57.2f, Color.WHITE
            );

            foreach (var arrow in arrows)
                arrow.Draw();

            if (currentArrow != null)
                currentArrow.Draw();
        }
    }

    class Arrow
    {
        Vector2 position;
        float rotation;
        Vector2 velocity;
        Vector2 lastPos;

        public Arrow(Vector2 position, float rotation, float speed)
        {
            this.position = position;
            this.rotation = rotation;
            this.velocity = (MathF.PI / 2 - rotation).ToVector() * speed;
            this.lastPos = position;
        }

        public void Update()
        {
            var delta = Raylib.GetFrameTime();

            velocity.X += -Math.Sign(velocity.X) * 100 * delta;
            velocity.Y += 1000 * delta;

            lastPos = position;
            position += velocity * delta;

            rotation = (position - lastPos).ToAngle();
        }

        public void Draw()
        {
            Raylib.DrawTexturePro(
                Texture.Arrow,
                new Rectangle(0, 0, Texture.Arrow.width, Texture.Arrow.height),
                new Rectangle(position.X, position.Y, Texture.Arrow.width, Texture.Arrow.height),
                new Vector2(Texture.Arrow.width, Texture.Arrow.height) / 2,
                rotation * 57.2f, Color.WHITE
            );
        }
    }
}
