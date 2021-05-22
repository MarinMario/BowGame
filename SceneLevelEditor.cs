using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Game
{
    class SceneLevelEditor : IScene
    {
        Camera2D camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1);
        List<Vector2> tiles = new List<Vector2>();
        const int cellSize = 32;

        const int offset = 10;
        const int elemWidth = 150;
        const int elemHeight = 50;

        TextBox fileNameBox = new TextBox(Vector2.Zero, new Vector2(elemWidth, elemHeight), "test", GuiStyle.Default());

        bool cameraFollowMouse;
        Vector2 clickMousePos;
        Vector2 clickCameraPos;

        bool showGrid = true;

        public SceneLevelEditor()
        {

        }

        public void Update()
        {
            fileNameBox.position = new Vector2(0 * elemWidth + 1 * offset, offset);

            camera.offset = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()) / 2;
            camera.zoom += Raylib.GetMouseWheelMove() / 10;
            camera.zoom = Math.Clamp(camera.zoom, 0.1f, 5);


            //camera movement with middle button
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_MIDDLE_BUTTON))
            {
                cameraFollowMouse = true;
                clickMousePos = Raylib.GetMousePosition();
                clickCameraPos = camera.target;
            }
            if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_MIDDLE_BUTTON))
                cameraFollowMouse = false;

            if (cameraFollowMouse)
            {
                camera.target = clickCameraPos + (clickMousePos - Raylib.GetMousePosition()) / camera.zoom;
            }

            //add tile on mouse left pressed
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
            {
                var p = camera.target + (Raylib.GetMousePosition() - camera.offset) / camera.zoom;
                var tilePos = PointToTile(p);
                if (!tiles.Contains(tilePos))
                    tiles.Add(tilePos);
            }
            //remove tile on mouse right pressed
            if(Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
            {
                var p = camera.target + (Raylib.GetMousePosition() - camera.offset) / camera.zoom;
                var tilePos = PointToTile(p);
                tiles.Remove(tilePos);
            }


            //shortcut keys
            if(Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL))
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_G))
                    showGrid = !showGrid;
            }
        }

        public void Draw()
        {
            Raylib.BeginMode2D(camera);
            {
                var height = Raylib.GetScreenHeight();
                var width = Raylib.GetScreenWidth();
                foreach (var tile in tiles)
                    Raylib.DrawRectangleV(tile * cellSize, Vector2.One * cellSize, Color.RED);

                if (showGrid)
                {
                    var p = PointToTile(camera.target - camera.offset / camera.zoom - new Vector2(cellSize)) * cellSize;
                    for (var y = (int)p.Y; y < camera.target.Y + height / camera.zoom; y += cellSize)
                        for (var x = (int)p.X; x < camera.target.X + width / camera.zoom; x += cellSize)
                        {
                            Raylib.DrawLine(x, y, x + cellSize, y, Color.BLACK);
                            Raylib.DrawLine(x, y, x, y + cellSize, Color.BLACK);
                        }
                }

            }
            Raylib.EndMode2D();

            Raylib.DrawFPS(1180, 10);
            fileNameBox.Draw();
        }

        public Vector2 PointToTile(Vector2 p)
        {
            var result = new Vector2((int)(p.X / cellSize), (int)(p.Y / cellSize));
            if (p.X < 0)
                result.X -= 1;
            if (p.Y < 0)
                result.Y -= 1;
            return result;
        }
    }
}
