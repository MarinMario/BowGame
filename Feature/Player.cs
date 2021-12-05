using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;
using Game.Engine;


namespace Game.Feature
{
    class Player : IBody
    {
        float speed = 400;
        float gravity = 2000;
        public Vector2 velocity = Vector2.Zero;
        float jumpForce = 1400;
        int maxJumps = 999;
        int currentJumps = 0;
        public float stopMovementTimer = 0;

        public CollisionBody Body { get; set; }
        public Bow bow = new Bow(Vector2.Zero, 0);
        Animation walkAnim = new Animation(new KeyFrame[]
        {
            new KeyFrame(-30, 0), new KeyFrame(30, 0.4f), new KeyFrame(-30, 0.4f)
        }, -30);
        Animation jumpAnim = new Animation(new KeyFrame[]
        {
            new KeyFrame(0, 0), new KeyFrame(30, 0.1f)
        }, 0);

        float legAngle = -30;
        Raycast walkingRay = new Raycast(Vector2.Zero, 0, 150);

        public Player()
        {
            Body = new CollisionBody(new Vector2(200, -400), new Vector2(30, 90));
        }

        public void Update(List<IBody> envBodies, List<IBody> monsterBodies, Camera2D camera)
        {
            PlatformerMovement(envBodies);
            walkingRay.position = Body.position + Body.size / 2;
            var walkingCast = walkingRay.Cast(envBodies);

            //animations
            if (velocity.Y < 0 && !jumpAnim.finished)
            {
                jumpAnim.Play();
                if (jumpAnim.value != jumpAnim.keyFrames[0].value)
                    legAngle = jumpAnim.value;
                walkAnim = new Animation(walkAnim.keyFrames, -legAngle);
            }
            else if (walkingCast.length < walkingRay.maxLength && stopMovementTimer < 0)
            {
                if (velocity.X != 0)
                {
                    walkAnim.Play();
                    legAngle = walkAnim.value;
                }
                jumpAnim = new Animation(jumpAnim.keyFrames, legAngle > 0 ? legAngle : -legAngle);
            }

            bow.position = Body.position + Body.size / 2 - new Vector2(0, 30);
            bow.rotation = (camera.ScaledMousePosition() - bow.position).ToAngle();
            bow.Update(Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON), envBodies, monsterBodies);
        }

        public void Draw()
        {
            var center = Body.position + Body.size / 2;
            var leg1 = legAngle.ToVector() * 50;
            var leg2 = (-legAngle).ToVector() * 50;
            var armPos = center - new Vector2(0, 30);
            var headPos = armPos - new Vector2(0, 25);
            var legColor = new Color(56, 124, 109, 255);
            var armColor = new Color(233, 137, 106, 255);
            var headColor = new Color(248, 245, 241, 255);
            Raylib.DrawLineEx(center, center + leg1, 10, legColor);
            Raylib.DrawLineEx(center, center + leg2, 10, legColor);
            Raylib.DrawLineEx(center, armPos, 10, armColor);
            Raylib.DrawLineEx(armPos, headPos, 10, headColor);
            Raylib.DrawCircleV(headPos, 20, headColor);
            bow.Draw();
            Raylib.DrawLineEx(armPos, armPos + (90 - bow.rotation).ToVector() * 40, 8, armColor);
            walkingRay.Draw();
        }

        public void PlatformerMovement(List<IBody> bodies)
        {
            var delta = Raylib.GetFrameTime();

            var xdir = 0;
            if (Raylib.IsKeyDown(Program.input.Right))
                xdir = 1;
            if (Raylib.IsKeyDown(Program.input.Left))
                xdir = -1;

            velocity.Y += gravity * delta;
            velocity.Y = Math.Clamp(velocity.Y, -1000, 1000);


            stopMovementTimer -= delta;
            if (stopMovementTimer <= 0)
            {
                velocity.X = xdir * speed;
                if (Raylib.IsKeyPressed(Program.input.Jump) && currentJumps < maxJumps)
                {
                    velocity.Y = -jumpForce;
                    currentJumps += 1;
                }
            }
            else
                velocity.X = velocity.X.MoveTowards(0, 500 * delta, 1);


            var nextVel = velocity * delta;
            var collision = Body.Collides(nextVel, bodies);

            if (collision.direction.X != 0)
            {
                nextVel.X = 0;
            }
            if (collision.direction.Y != 0)
            {
                nextVel.Y = 0;
                velocity.Y = 0;
            }
            Body.position += nextVel;

            if (collision.direction.Y == 1)
            {
                velocity.Y = 0;
                currentJumps = 0;
            }
        }

        public void TopDownMovement(List<IBody> bodies)
        {
            var dir = Vector2.Zero;
            if (Raylib.IsKeyDown(Program.input.Right))
                dir.X = 1;
            if (Raylib.IsKeyDown(Program.input.Left))
                dir.X = -1;
            if (Raylib.IsKeyDown(Program.input.Jump))
                dir.Y = -1;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
                dir.Y = 1;
            if (dir != Vector2.Zero)
                dir = Vector2.Normalize(dir);

            velocity = dir * speed * Raylib.GetFrameTime();
            var collision = Body.Collides(velocity, bodies);

            if (collision.direction.X == 0)
                Body.position.X += velocity.X;
            if (collision.direction.Y == 0)
                Body.position.Y += velocity.Y;
        }

    }
}
