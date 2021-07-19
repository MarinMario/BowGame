using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;

namespace Game
{
    class Bow
    {
        public Vector2 position;
        public float rotation;
        public List<Arrow> arrows = new List<Arrow>();
        Arrow currentArrow;
        float speedTimer = 0;

        public Bow(Vector2 position, float rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public void Update(bool charge, List<IBody> envBodies, List<IBody> monsterBodies)
        {
            var delta = Raylib.GetFrameTime();

            foreach (var arrow in arrows)
            {
                var update = true;
                foreach (var body in envBodies)
                    if (body.Body.Contains(arrow.position))
                        update = false;
                        
                if(update) arrow.Update();
            }


            if (charge)
            {
                speedTimer += delta;
                speedTimer = Math.Clamp(speedTimer, 0, 1);
                var r = (90 - rotation).ToVector();
                currentArrow = new Arrow(position + r * 50 - r * 25 * speedTimer, rotation, speedTimer * 3000, 100, 2000);
            }
            else if (currentArrow != null)
            {
                arrows.Add(currentArrow);
                currentArrow = null;
                speedTimer = 0;
            }

            foreach (var body in monsterBodies)
                for (var i = arrows.Count - 1; i >= 0; i--)
                    if (body.Body.Contains(arrows[i].position) && (body as IMonster).Health > 0)
                    {
                        (body as IMonster).TakeDamage(50, arrows[i].velocity);
                        arrows.RemoveAt(i);
                    }
        }

        public void Draw()
        {
            Raylib.DrawTexturePro(
                Texture.Bow,
                new Rectangle(0, 0, Texture.Bow.width, Texture.Bow.height),
                new Rectangle(position.X, position.Y, Texture.Bow.width, Texture.Bow.height),
                new Vector2(0, Texture.Bow.height / 2), rotation, Color.WHITE
            );

            foreach (var arrow in arrows)
                arrow.Draw();

            if (currentArrow != null)
                currentArrow.Draw();
        }
    }

    class Arrow
    {
        public Vector2 position;
        float rotation;
        public Vector2 velocity;
        Vector2 lastPos;
        float friction;
        float fallSpeed;

        public Arrow(Vector2 position, float rotation, float speed, float friction, float fallSpeed)
        {
            this.position = position;
            this.rotation = rotation;
            this.velocity = (90 - rotation).ToVector() * speed;
            this.lastPos = position;
            this.friction = friction;
            this.fallSpeed = fallSpeed;
        }

        public void Update()
        {
            var delta = Raylib.GetFrameTime();

            velocity.X += -Math.Sign(velocity.X) * friction * delta;
            velocity.Y += fallSpeed * delta;

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
                rotation, Color.WHITE
            );

            Raylib.DrawCircleV(position, 5, Color.BLUE);
        }
    }
}
