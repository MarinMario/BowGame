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
        int currentFrame = 1;

        public Animation(KeyFrame[] keyFrames)
        {
            this.keyFrames = keyFrames;
            value = keyFrames[0].value;
            distance = Math.Abs(keyFrames[currentFrame].value - value);
        }

        public void Update()
        {
            var delta = Raylib.GetFrameTime();
            timer += delta;

            value = value.MoveTowards(keyFrames[currentFrame].value, distance * delta / keyFrames[currentFrame].speed, 1);
            if (timer > keyFrames[currentFrame].speed)
            {
                Console.WriteLine($"timer: {timer}, value: {value}, target: {keyFrames[currentFrame].value}");
                timer = 0;

                currentFrame += 1;
                if (currentFrame == keyFrames.Length)
                {
                    currentFrame = 1;
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
