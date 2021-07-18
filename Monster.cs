using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;


namespace Game
{
    //class Monster : IBody
    //{
    //    public CollisionBody Body { get; set; }
    //    float speed = 300;
    //    float gravity = 980;
    //    Vector2 velocity = Vector2.Zero;
    //    float jumpForce = 700;

    //    public Monster(Vector2 position)
    //    {
    //        Body = new CollisionBody(position, Vector2.One * 100);
    //    }

    //    public void Update(List<IBody> bodies)
    //    {
    //        var delta = Raylib.GetFrameTime();
    //        velocity.Y += gravity * delta;
    //        velocity.Y = Math.Clamp(velocity.Y, -1000, 1000);

    //        var nextVel = new Vector2(velocity.X, velocity.Y * delta);
    //        var collision = Body.Collides(nextVel, bodies);
    //        if (collision.direction.X != 0)
    //            nextVel.X = 0;
    //        if (collision.direction.Y != 0)
    //        {
    //            nextVel.Y = 0;
    //            velocity.Y = 0;
    //        }

    //        Body.position += nextVel;

    //    }

    //    public void Draw()
    //    {
    //        Raylib.DrawTextureEx(Texture.GrassTile, Body.position, 0, 1, Color.WHITE);
    //    }
    //}

    interface Monster
    {
        CollisionBody Body { get; set; }
        int Health { get; set; }
    }

    class Bat : Monster
    {
        public CollisionBody Body { get; set; }
        public int Health { get; set; }
        bool alive = true;
        Raycast rayToGround = new Raycast(Vector2.Zero, 0, 1000);
        int distanceFromGround = 500;
        Vector2 velocity = Vector2.Zero;
        Animation wingAngle = new Animation(new KeyFrame[]
        {
            new KeyFrame(20, 0), new KeyFrame(70, 0.3f), new KeyFrame(20, 0.3f)
        }, 0);
        float attackTimer = 0;
        Vector2? attackTarget = null;


        public Bat(Vector2 position)
        {
            Body = new CollisionBody(position, new Vector2(64));
            Health = 100;
        }

        public void Update(Player player, List<IBody> bodies)
        {
            var delta = Raylib.GetFrameTime();
            rayToGround.position = Body.position + Body.size / 2;
            var groundCast = rayToGround.Cast(bodies);
            var target = player.Body.position;
            var arrows = player.bow.arrows;

            if (alive)
            {
                attackTimer += delta;
                if (attackTimer > 3)
                {
                    attackTarget = target;
                    attackTimer = 0;
                }
                if (attackTarget != null)
                {
                    velocity += Vector2.Normalize((Vector2)attackTarget - Body.position) * 5000 * delta;
                    if (groundCast.length < 50)
                        attackTarget = null;
                }
                else
                {

                    velocity.Y = 0;
                    velocity.X += Vector2.Normalize(target - Body.position).X * 1000 * delta;
                    //this calculates y position, aka it keeps the body a distance above the ground, the distance is the variable distanceFromGround
                    if (groundCast.length < distanceFromGround - 1 || groundCast.length > distanceFromGround + 1)
                    {
                        var targetY = groundCast.position.Y - distanceFromGround - Body.size.Y;
                        Body.position.Y = Body.position.Y.MoveTowards(targetY, 300 * delta, 1);
                    }
                }

                for (var i = arrows.Count - 1; i >= 0; i--)
                {
                    if (Body.Contains(arrows[i].position))
                    {
                        Health -= 25;
                        attackTimer = 0;
                        var dir = arrows[i].velocity == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(arrows[i].velocity);
                        //attackTarget = Body.position + dir * 200;
                        velocity = arrows[i].velocity;
                        arrows.RemoveAt(i);
                    }
                }

                if (Vector2.Distance(target, Body.position) < 100 && player.stopMovementTimer < 0)
                {
                    Console.WriteLine("UESSSS");
                    player.stopMovementTimer = 1f;
                    player.velocity = new Vector2(Vector2.Normalize(velocity).X * 1000, -500);
                }

                wingAngle.Play();
            }
            else
            {
                if (groundCast.length > 10)
                {
                    velocity.Y += 1000 * delta;
                    velocity.X = velocity.X.MoveTowards(0, 500 * delta, 1);
                }
                else
                    velocity.Y = 0;
            }

            velocity.X = Math.Clamp(velocity.X, -1000, 1000);
            velocity.Y = Math.Clamp(velocity.Y, -1000, 1000);

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

            if (Health <= 0 && alive)
            {
                alive = false;
            }
        }

        public void Draw()
        {
            //rayToGround.Draw();
            //Raylib.DrawCircleV(Body.position + Body.size / 2, Body.size.X / 2, Color.RED);
            var size = new Rectangle(0, 0, 64, 64);
            Raylib.DrawTexturePro(Texture.BatWing, size, new Rectangle(Body.position.X + 32, Body.position.Y + 32, 64, 64), new Vector2(64, 0), wingAngle.value, Color.WHITE);
            var textureFlipped = Texture.BatWing;
            textureFlipped.width = -64;
            Raylib.DrawTexturePro(textureFlipped, size, new Rectangle(Body.position.X + 32, Body.position.Y + 32, 64, 64), Vector2.Zero, -wingAngle.value, Color.WHITE);
            Raylib.DrawTextureEx(alive ? Texture.BatHead : Texture.BatHeadDead, Body.position, 0, 1, Color.WHITE);
        }
    }
}
