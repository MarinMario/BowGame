using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;
using Game.Engine;

namespace Game.Scene
{
    class Settings : Feature.IScene
    {
        Button back = new Button(Vector2.Zero, new Vector2(300, 100), "back", GuiStyle.Default());
        Feature.Transition transition = new Feature.Transition();
        Feature.Input input;

        int selected = -1;

        //enum where each value corresponds to a property in Input.cs
        enum Key { Jump, Left, Right }

        List<(Key key, Button button)> keys = new List<(Key, Button)>();

        public Settings()
        {
            //loading keybindings struct
            input = Util.Load<Feature.Input>("inputTest.json");

            //initalizing button names based on the loaded data
            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                var keyName = key.ToString();
                var keyVal = typeof(Feature.Input).GetProperty(key.ToString()).GetValue(input).ToString().Remove(0, 4);
                keys.Add((key, new Button(Vector2.Zero, new Vector2(300, 100), keyName + ": " + keyVal, GuiStyle.Default())));
            }
        }

        public void Update()
        {
            var m = Raylib.GetMousePosition();
            var width = Raylib.GetScreenWidth();
            var height = Raylib.GetScreenHeight();

            //setting position of keybiding buttons based on screensize and marking the last pressed button as selected
            for (var i = 0; i < keys.Count; i++)
            {
                keys[i].button.position = new Vector2(width, height) / 2 - keys[i].button.size / 2 + (i - 1) * new Vector2(0, 110);
                if(keys[i].button.Active(m))
                {
                    if (selected != -1) keys[selected].button.selected = false;
                    selected = i;
                    if (selected != -1) keys[selected].button.selected = true;
                }
            }

            //if a button is selected the next key pressed will be saved as keybinding for the key that is represented by the button
            if (selected != -1)
            {
                var key = (KeyboardKey)Raylib.GetKeyPressed();
                if (key != 0)
                {
                    typeof(Feature.Input).GetProperty(keys[selected].key.ToString()).SetValue(input, key);
                    var keyVal = typeof(Feature.Input).GetProperty(keys[selected].key.ToString()).GetValue(input).ToString().Remove(0, 4);
                    keys[selected].button.text = keys[selected].key.ToString() + ": " + keyVal;
                    keys[selected].button.selected = false;
                    selected = -1;
                    Util.Save("inputTest.json", input);
                }
            }


            back.position.X = width - back.size.X;

            if (back.Active(m))
                transition.FadeOut(new MainMenu());

            transition.Update();
        }

        public void Draw()
        {
            var m = Raylib.GetMousePosition();
            back.Draw(m);
            foreach (var key in keys)
                key.button.Draw(m);

            transition.Draw();
        }
    }
}
