using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace Game
{
    class World : IScene
    {
        List<IBody> bodies = new List<IBody>();
        Player player = new Player();
        Camera2D camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1);
        List<MapObject> mapData = LevelEditor.Load("test");
        List<Monster> monsters = new List<Monster>();


        public World()
        {
            //bodies.Add(player);
            camera.target = player.Body.position;

            foreach (var obj in mapData)
            {
                var type = Texture.Tile[obj.Name].type;
                if (type == TileType.Collision)
                {
                    var pos = new Vector2(obj.X, obj.Y);
                    var texture = Texture.Tile[obj.Name].texture;
                    var size = new Vector2(texture.width, texture.height);
                    bodies.Add(new SimpleBody(new CollisionBody(pos, size)));
                }
            }
            
        }

        public void Update()
        {
            player.Update(bodies, camera.target - camera.offset);
            camera.offset = new Vector2(Raylib.GetScreenWidth() / 2 - player.Body.size.X / 2, Raylib.GetScreenHeight());
            camera.target = camera.target.MoveTowards(new Vector2(player.Body.position.X, 0), Raylib.GetFrameTime() * 500, 1);

            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_L))
                Program.scene = new LevelEditor();

            foreach (var m in monsters)
                m.Update(bodies);

        }

        public void Draw()
        {
            Raylib.BeginMode2D(camera);
            
            foreach (var o in mapData)
                Raylib.DrawTextureEx(Texture.Tile[o.Name].texture, new Vector2(o.X, o.Y), 0, 1, Color.WHITE);

            foreach (var m in monsters)
                m.Draw();

            player.Draw();

            //foreach (var body in bodies) body.Body.Draw(new Color(100, 0, 0, 100));

            Raylib.EndMode2D();
        }


    }
}
