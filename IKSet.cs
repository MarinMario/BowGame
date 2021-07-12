using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using System;

namespace Game
{
    struct IKSegment
    {
        public Vector2 p1;
        public Vector2 p2;
        public float angle;
        public float length;

        public IKSegment(float angle, float length)
        {
            p1 = Vector2.Zero;
            p2 = Vector2.Zero;
            this.angle = angle;
            this.length = length;
        }

        public void CalcP2()
        {
            var r = angle.ToVector() * length + p1;
            p2 = r;
        }

        public void CalcP1()
        {
            var r = p2 - angle.ToVector() * length;
            p1 = r;
        }
    }

    class IKSet
    {
        public Vector2 position;
        public Vector2 targetPosition;
        public IKSegment[] set;

        public IKSet(Vector2 position, Vector2 targetPosition, IKSegment[] set)
        {
            this.position = position;
            this.targetPosition = targetPosition;
            this.set = set;
            FixAtPosition();
        }

        //this function simply moves all the segments so that the position of the first segment is equal to 'position' - the variable in this class
        void FixAtPosition()
        {
            set[0].p1 = position;
            set[0].CalcP2();
            for (var i = 1; i < set.Length; i++)
            {
                set[i].p1 = set[i - 1].p2;
                set[i].CalcP2();
            }
        }

        public void Update()
        {
            set[^1].angle = 90 - Util.ToAngle(targetPosition - set[^1].p1);
            set[^1].p2 = targetPosition;
            set[^1].CalcP1();

            for (var i = set.Length - 2; i >= 0; i--)
            {
                set[i].angle = 90 - Util.ToAngle(set[i + 1].p1 - set[i].p1);
                set[i].p2 = set[i + 1].p1;
                set[i].CalcP1();
            }

            FixAtPosition();
        }

        public float CalcTotalLength()
        {
            var result = 0f;
            foreach (var thing in set)
                result += thing.length;

            return result;
        }
    }
}
