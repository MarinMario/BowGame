using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;

namespace Game
{
    static class Gui
    {
        public static void Button(Vector2 position, Vector2 size, string text, GuiStyle style)
        {
            var m = Raylib.GetMousePosition();
            var hover = m.X > position.X && m.Y > position.Y && m.X < position.X + size.X && m.Y < position.Y + size.Y;
            var active = Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON) && hover;
            var s = active ? style.active : hover ? style.hover : style.normal;

            var textSize = Raylib.MeasureTextEx(s.font, text, s.fontSize, s.spacing);
            var textPos = position + size / 2 - textSize / 2;

            Raylib.DrawRectangleV(position, size, s.backgroundColor);
            Raylib.DrawRectangleLinesEx(new Rectangle(position.X, position.Y, size.X, size.Y), s.borderThickness, s.borderColor);
            Raylib.DrawTextEx(s.font, text, textPos, s.fontSize, s.spacing, s.fontColor);
        }
    }

    class GuiStyle
    {
        public Style normal = new Style();
        public Style hover = new Style();
        public Style active = new Style();

        public static GuiStyle Default()
        {
            var s = new GuiStyle();

            var bgColor = new Color(255, 226, 104, 255);
            var borderColor = new Color(54, 69, 71, 255);
            var borderColorHover = new Color(35, 45, 46, 255);
            var bgColorActive = new Color(255, 176, 55, 255);

            s.normal.fontColor = borderColor;
            s.normal.backgroundColor = bgColor;
            s.normal.borderColor = borderColor;

            s.hover.fontColor = borderColorHover;
            s.hover.backgroundColor = bgColor;
            s.hover.borderColor = borderColorHover;

            s.active.fontColor = borderColorHover;
            s.active.backgroundColor = bgColorActive;
            s.active.borderColor = borderColorHover;

            return s;
        }

        public class Style
        {
            public Font font = Raylib.GetFontDefault();
            public Color fontColor = Color.BLACK;
            public int fontSize = 20;
            public int spacing = 1;
            public Color backgroundColor = Color.LIGHTGRAY;
            public Color borderColor = Color.BLACK;
            public int borderThickness = 5;
        }
    }
}
