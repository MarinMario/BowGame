using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using System.Text.Json;
using Game.Engine;

namespace Game.Scene
{
    class LevelEditor : IScene
    {
        Camera2D camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1);
        //List<TileName> tiles = new List<TileName>();
        List<MapObject> objects = new List<MapObject>();
        const int cellSize = 32;

        const int offset = 10;
        const int elemWidth = 150;
        const int elemHeight = 50;
        const int bannedX = 80;
        const int bannedY = 70;

        TextBox fileNameBox = new TextBox(Vector2.Zero, new Vector2(elemWidth, elemHeight), "name", GuiStyle.Default());
        List<TextureButton> tileButtons = new List<TextureButton>();

        bool cameraFollowMouse;
        Vector2 clickMousePos;
        Vector2 clickCameraPos;

        bool showGrid = true;

        TileName selectedObject = TileName.Earth1;

        public LevelEditor()
        {
            foreach (var mo in Enum.GetValues(typeof(TileName)))
            {
                var texture = Asset.Tile[(TileName)mo].texture;
                var scale = (float)cellSize / texture.width;
                tileButtons.Add(new TextureButton(Vector2.Zero, scale, Color.WHITE, texture));
            }
        }

        public void Update()
        {
            fileNameBox.position = new Vector2(0 * elemWidth + 1 * offset, bannedY / 2 - elemHeight / 2);
            camera.offset = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()) / 2;

            //side buttons for tile selecting
            for (var i = 0; i < tileButtons.Count; i++)
            {
                var b = tileButtons[i].button;
                tileButtons[i].button.position = new Vector2(Raylib.GetScreenWidth() - bannedX / 2 - b.size.X / 2, i * 70 + bannedY + 10);
                tileButtons[i].tint = i == (int)selectedObject ? Color.WHITE : Color.GRAY;
                if (b.Active())
                    selectedObject = (TileName)i;
            }

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


            var mouse = Raylib.GetMousePosition();
            if (mouse.X < Raylib.GetScreenWidth() - bannedX && mouse.Y > bannedY)
            {
                //add tile on mouse left pressed
                if ((Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON) && showGrid) || Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    var p = camera.target + (mouse - camera.offset) / camera.zoom;
                    var texture = Asset.Tile[selectedObject].texture;
                    var pos = showGrid ? PointToTile(p) : p - new Vector2(texture.width, texture.height) / 2;
                    var mapObject = new MapObject(selectedObject, pos.X, pos.Y);
                    var contains = false;
                    foreach (var o in objects)
                        if (o.X == mapObject.X && o.Y == mapObject.Y)
                            contains = true;
                    if (!contains)
                        objects.Add(mapObject);
                }

                //remove tile on mouse right pressed
                if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
                {
                    var p = camera.target + (mouse - camera.offset) / camera.zoom;
                    for (var i = objects.Count - 1; i >= 0; i--)
                    {
                        var o = objects[i];
                        var t = Asset.Tile[o.Name].texture;
                        if(p.X > o.X && p.Y > o.Y && p.X < o.X + t.width && p.Y < o.Y + t.height)
                            objects.RemoveAt(i);
                    }
                }

                //camera zoom
                camera.zoom += Raylib.GetMouseWheelMove() / 10;
                camera.zoom = Math.Clamp(camera.zoom, 0.1f, 5);
            }


            //shortcut keys
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL))
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_G))
                    showGrid = !showGrid;

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_S))
                    Save();

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_L))
                    objects = Load(fileNameBox.text);

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_P))
                    Program.scene = new World();
            }
        }

        public void Draw()
        {
            var height = Raylib.GetScreenHeight();
            var width = Raylib.GetScreenWidth();
            Raylib.BeginMode2D(camera);
            {
                foreach (var o in objects)
                    Raylib.DrawTextureEx(Asset.Tile[o.Name].texture, new Vector2(o.X, o.Y), 0, 1, Color.WHITE);
                   
                if (showGrid)
                {
                    var p = PointToTile(camera.target - camera.offset / camera.zoom - new Vector2(cellSize));
                    for (var y = (int)p.Y; y < camera.target.Y + (height - camera.offset.Y) / camera.zoom; y += cellSize)
                        for (var x = (int)p.X; x < camera.target.X + (width - camera.offset.X) / camera.zoom; x += cellSize)
                        {
                            if (y == 0 || y % 1080 == 0)
                                Raylib.DrawLineEx(new Vector2(x, y), new Vector2(x + cellSize, y), 5, Color.BLACK);
                            if (x == 0)
                                Raylib.DrawLineEx(new Vector2(x, y), new Vector2(x, y + cellSize), 5, Color.BLACK);
                            Raylib.DrawLine(x, y, x + cellSize, y, Color.BLACK);
                            Raylib.DrawLine(x, y, x, y + cellSize, Color.BLACK);
                        }
                }

            }
            Raylib.EndMode2D();

            Raylib.DrawRectangle(0, 0, width - bannedX, bannedY, new Color(0, 0, 0, 100));
            Raylib.DrawRectangle(width - bannedX, 0, bannedX, height, new Color(0, 0, 0, 100));

            fileNameBox.Draw();
            foreach (var button in tileButtons)
                button.Draw();
        }

        Vector2 PointToTile(Vector2 p)
        {
            var result = new Vector2((int)(p.X / cellSize), (int)(p.Y / cellSize));
            if (p.X < 0)
                result.X -= 1;
            if (p.Y < 0)
                result.Y -= 1;
            return result * cellSize;
        }

        void Save()
        {
            System.IO.File.WriteAllText($"Content/{fileNameBox.text}.json", JsonSerializer.Serialize(objects));
        }

        public static List<MapObject> Load(string mapName)
        {
            var result = new List<MapObject>();
            try
            { 
                var file = System.IO.File.ReadAllText($"Content/{mapName}.json");
                result = JsonSerializer.Deserialize<List<MapObject>>(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return result;
        }
    }

    
    struct MapObject
    {
        public TileName Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public MapObject(TileName name, float x, float y)
        {
            Name = name;
            X = x;
            Y = y;
        }
    }
}
