using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;

namespace Game.Feature
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

    interface IMonster
    {
        int Health { get; set; }
        void TakeDamage(int damage, Vector2 velocity);
    }

    class Bat : IMonster, IBody
    {
        public CollisionBody Body { get; set; }
        public int Health { get; set; } = 100;
        bool alive = true;
        Raycast rayToGround = new Raycast(Vector2.Zero, 0, 1000);
        int distanceFromGround = 500;
        public Vector2 velocity = Vector2.Zero;
        float friction = 100;
        float speed = 1000;
        float attackSpeed = 5000;
        float gravity = 1200;
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
            var target = player.Body.position + player.Body.size / 2;
            var position = Body.position + Body.size / 2;
            var arrows = player.bow.arrows;
            var nextVel = velocity * delta;
            var collision = Body.Collides(nextVel, bodies);

            if (alive)
            {
                //every few seconds the attackTimer resets and chooses a target position so the bat can go towards it
                attackTimer += delta;
                if (attackTimer > 3 && attackTarget == null)
                {
                    attackTarget = target;
                    attackTimer = 0;
                    //velocity = Vector2.Zero;
                }

                if (attackTarget != null)
                {
                    //this moves the bat towards the target position in an attack and if it hits the floor or is very close to the target then it'll go back to a set distance above the ground
                    velocity += Vector2.Normalize((Vector2)attackTarget - velocity * friction * delta - position) * attackSpeed * delta;
                    if (collision.direction != Vector2.Zero || Vector2.Distance((Vector2)attackTarget, position) < 10)
                        attackTarget = null;
                }
                else
                {
                    velocity.X += Vector2.Normalize(target - velocity * friction * delta - position).X * speed * delta;
                    //this calculates y position, aka it keeps the bat a distance above the ground, the distance is the variable distanceFromGround
                    if (groundCast.length < distanceFromGround - 1 || groundCast.length > distanceFromGround + 1)
                    {
                        var targetY = groundCast.position.Y - distanceFromGround - Body.size.Y;
                        velocity.Y += Math.Sign(targetY - velocity.Y - position.Y) * 1000 * delta;
                    }
                }

                if (Vector2.Distance(target, position) < 100 && player.stopMovementTimer < 0)
                {
                    player.stopMovementTimer = 1f;
                    player.velocity = new Vector2(Vector2.Normalize(velocity).X * 1000, -500);
                }

                wingAngle.Play();

                if (Health <= 0)
                    alive = false;
            }
            else
            {
                velocity.Y += gravity * delta;
                velocity.X -= velocity.X * delta;
            }

            velocity.X = Math.Clamp(velocity.X, -1000, 1000);
            velocity.Y = Math.Clamp(velocity.Y, -1000, 1000);

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
        }

        public void Draw()
        {
            var size = new Rectangle(0, 0, 64, 64);
            Raylib.DrawTexturePro(Texture.BatWing, size, new Rectangle(Body.position.X + 32, Body.position.Y + 32, 64, 64), new Vector2(64, 0), wingAngle.value, Color.WHITE);
            var textureFlipped = Texture.BatWing;
            textureFlipped.width = -64;
            Raylib.DrawTexturePro(textureFlipped, size, new Rectangle(Body.position.X + 32, Body.position.Y + 32, 64, 64), Vector2.Zero, -wingAngle.value, Color.WHITE);
            Raylib.DrawTextureEx(alive ? Texture.BatHead : Texture.BatHeadDead, Body.position, 0, 1, Color.WHITE);
        }

        public void TakeDamage(int damage, Vector2 velocity)
        {
            Health -= damage;
            this.velocity = velocity;
        }
    }
}
