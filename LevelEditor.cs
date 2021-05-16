using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Game
{
    class LevelEditor
    {
        const int offset = 10;
        const int elemWidth = 150;
        const int elemHeight = 50;

        TextBox fileNameBox = new TextBox(Vector2.Zero, new Vector2(elemWidth, elemHeight), "test", GuiStyle.Default());
        Button saveButton   = new Button(Vector2.Zero, new Vector2(elemWidth, elemHeight), "save", GuiStyle.Default());
        Button loadButton   = new Button(Vector2.Zero, new Vector2(elemWidth, elemHeight), "load", GuiStyle.Default());

        public LevelEditor()
        {

        }

        public void Update()
        {
            fileNameBox.position = new Vector2(Raylib.GetScreenWidth() - 3 * elemWidth - 3 * offset, offset);
            saveButton.position = new Vector2(Raylib.GetScreenWidth() - 2 * elemWidth - 2 * offset, offset);
            loadButton.position = new Vector2(Raylib.GetScreenWidth() - 1 * elemWidth - 1 * offset, offset);
        }

        public void Draw()
        {
            fileNameBox.Draw();
            saveButton.Draw();
            loadButton.Draw();
        }
    }
}
