using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using Game.Engine;

namespace Game.Feature
{
    class Transition
    {
        Animation fadeIn = new Animation(new KeyFrame[] { new KeyFrame(0, 0.5f) }, 255);
        Animation fadeOut = new Animation(new KeyFrame[] { new KeyFrame(255, 0.5f) }, 0);
        bool playFadeOut = false;
        IScene nextScene;

        public void Update()
        {
            fadeIn.Play();

            if (playFadeOut)
                fadeOut.Play();

            if (fadeOut.finished)
                Program.scene = nextScene;
        }

        public void FadeOut(IScene nextScene)
        {
            this.nextScene = nextScene;
            playFadeOut = true;
        }

        public void Draw()
        {
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color(0, 0, 0, (int)fadeIn.value));
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), new Color(0, 0, 0, (int)fadeOut.value));
        }
    }
}
