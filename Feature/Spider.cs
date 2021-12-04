using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using Game.Engine;


namespace Game.Feature
{
    class Spider
    {
        public CollisionBody Body = new CollisionBody(new Vector2(400, -400), Vector2.One * 100);
        int distanceFromGround = 100;
        Raycast yPosRay = new Raycast(Vector2.Zero, 0, 300);

        SpiderLeg[] legs = new SpiderLeg[]
        {
            new SpiderLeg(150, 200, 300), new SpiderLeg(-150, 200, 300)
        };

        public void Update(List<IBody> bodies)
        {
            var delta = Raylib.GetFrameTime();

            ////movement
            var dir = Vector2.Zero;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
                dir.X = 1;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
                dir.X = -1;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
                dir.Y = -1;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
                dir.Y = 1;
            if (dir != Vector2.Zero)
                dir = Vector2.Normalize(dir);

            Body.position += dir * 300 * Raylib.GetFrameTime();


            yPosRay.position = Body.position + new Vector2(Body.size.X / 2, Body.size.Y);
            var r = yPosRay.Cast(bodies);
            if (r.length < distanceFromGround - 1 || r.length > distanceFromGround + 1)
            {
                var target = r.position.Y - distanceFromGround - Body.size.Y;
                var direction = target - Body.position.Y;
                if (direction != 0)
                    Body.position.Y += direction / Math.Abs(direction) * 300 * delta;
            }

            var centerX = Body.position + new Vector2(Body.size.X / 2, -100);
            var center = Body.position + Body.size / 2;
            for (var i = 0; i < legs.Length; i++)
            {
                legs[i].Update(centerX, center, dir, bodies);
            }

        }

        public void Draw()
        {
            Raylib.DrawCircleV(Body.position + Body.size / 2, 50, Color.BLACK);
            foreach (var leg in legs)
                leg.Draw();
            //yPosRay.Draw();
        }

    }

    class SpiderLeg
    {
        public float rayOffset;
        public Raycast targetRay = new Raycast(Vector2.Zero, 0, 1000);
        public IKSet leg;

        Vector2 nextPosition;
        Vector2 targetPosition;

        public SpiderLeg(float rayOffset, float length1, float length2)
        {
            this.rayOffset = rayOffset;
            leg = new IKSet(Vector2.Zero, Vector2.Zero, new IKSegment[] {
                new IKSegment(0, length1), new IKSegment(0, length2)
            });
        }

        public void Update(Vector2 rayPosition, Vector2 legPosition, Vector2 dir, List<IBody> bodies)
        {
            var delta = Raylib.GetFrameTime();

            targetRay.position = rayPosition + new Vector2(rayOffset, 0);
            leg.position = legPosition;
            leg.Update();
            var cast = targetRay.Cast(bodies);

            if (targetPosition == Vector2.Zero)
                leg.targetPosition = rayPosition + new Vector2(rayOffset * 2, -500);
            if (Vector2.Distance(cast.position, leg.targetPosition) > Math.Abs(rayOffset)
            && Vector2.Distance(cast.position, leg.position) < leg.CalcTotalLength())
            {
                nextPosition = (cast.position + leg.targetPosition) / 2 - new Vector2(0, Math.Abs(rayOffset) * 0.3f);
                targetPosition = cast.position;
            }
            if (Vector2.Distance(nextPosition, leg.targetPosition) > 5)
                leg.targetPosition += Vector2.Normalize(nextPosition - leg.targetPosition) * 1000 * delta;
            else
                nextPosition = targetPosition;

            //if (leg.set[0].p2.Y > leg.position.Y + 50)
            //{
            //    leg.set[0].p2.Y = leg.position.Y + 50;
            //    leg.set[1].p1 = leg.set[0].p2;
            //    leg.set[1].CalcP2();
            //    //leg.targetPosition = leg.set[0].p2 + new Vector2(0, 5);
            //}

            if (dir.X > 0)
                targetRay.angle = rayOffset < 0 ? 10 : 20;
            if (dir.X < 0)
                targetRay.angle = rayOffset > 0 ? -10 : -20;

        }

        public void Draw()
        {
            foreach (var segment in leg.set)
                Raylib.DrawLineEx(segment.p1, segment.p2, 20, Color.BLACK);
            targetRay.Draw();
        }
    }
}
