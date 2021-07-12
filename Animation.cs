using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace Game
{
    class Animation
    {
        public KeyFrame[] keyFrames;
        public float value;
        float distance;

        float timer = 0;
        public int currentFrame = 0;
        public bool finished = false;

        public Animation(KeyFrame[] keyFrames, float initialValue)
        {
            this.keyFrames = keyFrames;
            value = initialValue;
            distance = Math.Abs(keyFrames[currentFrame].value - value);
        }

        public void Play()
        {
            var delta = Raylib.GetFrameTime();
            timer += delta;

            if (keyFrames[currentFrame].speed != 0)
                value = value.MoveTowards(keyFrames[currentFrame].value, distance * delta / keyFrames[currentFrame].speed, 1);
            if (timer > keyFrames[currentFrame].speed)
            {
                timer = 0;
                currentFrame += 1;
                finished = false;
                if (currentFrame == keyFrames.Length)
                {
                    finished = true;
                    currentFrame = 0;
                    value = keyFrames[0].value;
                }
                distance = Math.Abs(keyFrames[currentFrame].value - value);
            }
        }
    }

    struct KeyFrame
    {
        public float value;
        public float speed;

        public KeyFrame(float value, float speed)
        {
            this.value = value;
            this.speed = speed;
        }
    }
}
