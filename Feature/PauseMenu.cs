using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using System.Numerics;

namespace Game.Feature
{
    class PauseMenu
    {
        public bool paused = false;
        public bool leaveButtonActive = false;
        Button resume = new Button(Vector2.Zero, new Vector2(300, 100), "resume", GuiStyle.Default());
        Button settings = new Button(Vector2.Zero, new Vector2(300, 100), "settings", GuiStyle.Default());
        Button leave = new Button(Vector2.Zero, new Vector2(300, 100), "leave", GuiStyle.Default());
        Button[] buttonList;

        public PauseMenu()
        {
            buttonList = new Button[] { resume, settings, leave };
        }

        public void Update()
        {

            if (Raylib.IsKeyPressed(Program.input.Pause))
            {
                paused = !paused;
            }

            if (!paused) return;

            var width = Raylib.GetScreenWidth();
            var height = Raylib.GetScreenHeight();

            for (var i = 0; i < buttonList.Length; i++)
            {
                buttonList[i].position = new Vector2(width, height) / 2 - buttonList[i].size / 2 + (i - buttonList.Length / 2) * new Vector2(0, 110);
            }

            var m = Raylib.GetMousePosition();
            if (resume.Active(m))
                paused = false;
            if (settings.Active(m)) ;

            leaveButtonActive = leave.Active(m);
        }

        public void Draw()
        {
            if (!paused) return;

            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color(0, 0, 0, 100));
            foreach (var button in buttonList)
                button.Draw(Raylib.GetMousePosition());
        }
    }
}
