using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using Game.Feature;

namespace Game.Scene
{
    class World : IScene
    {
        List<IBody> envBodies = new List<IBody>();
        List<IBody> monsterBodies = new List<IBody>();
        Player player = new Player();
        Camera2D camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1);
        List<MapObject> mapData = LevelEditor.Load("test");
        Transition transition = new Transition();
        List<Bat> bats = new List<Bat>();
        Bat bat = new Bat(new Vector2(300, -600));
        PauseMenu pauseMenu = new PauseMenu();

        public World()
        {
            //for (var i = 0; i < 1; i++)
            //    bats.Add(new Bat(new Vector2(i * 300, -600)));

            //bodies.Add(player);
            camera.target = player.Body.position;
            foreach (var obj in mapData)
            {
                var type = Tile.T[obj.Name].type;
                if (type == TileType.Collision)
                {
                    var pos = new Vector2(obj.X, obj.Y);
                    var texture = Tile.T[obj.Name].texture;
                    var size = new Vector2(texture.width, texture.height);
                    envBodies.Add(new SimpleBody(new CollisionBody(pos, size)));
                }
            }

            monsterBodies.Add(bat);
        }

        public void Update()
        {
            
            pauseMenu.Update();
            if(pauseMenu.leaveButtonActive) 
                transition.FadeOut(new MainMenu());
            transition.Update();

            //everything after this statement won't be update when the game is paused
            if (pauseMenu.paused) return;

            player.Update(envBodies, monsterBodies, camera);
            camera.offset = new Vector2(Raylib.GetScreenWidth() / 2 - player.Body.size.X / 2, Raylib.GetScreenHeight() / 2 - player.Body.size.Y / 2);
            //camera.target = camera.target.MoveTowards(player.Body.position, Raylib.GetFrameTime() * 500, 1);
            camera.target = player.Body.position - new Vector2(0, 200);
            camera.zoom = Util.ZoomToKeepRes(Program.res.x, Program.res.y);

            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL) && Raylib.IsKeyPressed(KeyboardKey.KEY_L))
                transition.FadeOut(new LevelEditor());

            foreach (var b in bats)
                b.Update(player, envBodies);

            bat.Update(player, envBodies);
        }

        public void Draw()
        {
            Raylib.BeginMode2D(camera);
            
            foreach (var o in mapData)
                Raylib.DrawTextureEx(Tile.T[o.Name].texture, new Vector2(o.X, o.Y), 0, 1, Color.WHITE);

            bat.Draw();

            player.Draw();
            foreach (var b in bats)
                b.Draw();

            //foreach (var body in bodies) body.Body.Draw(new Color(100, 0, 0, 100));

            Raylib.EndMode2D();

            pauseMenu.Draw();
            transition.Draw();
            Raylib.DrawFPS(100, 0);
            //Raylib.DrawText(bat.Health.ToString(), 0, 0, 20, Color.BLACK);

        }


    }
}
