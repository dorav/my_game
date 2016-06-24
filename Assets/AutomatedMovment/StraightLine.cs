using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace AutomatedMovment
{
    class StraightLineWalker : IPath
    {
        Vector3 start;
        Vector3 end;
        public float Speed { get; set; }

        public Vector3 LastPoint
        {
            get
            {
                return end;
            }
        }

        public float Distance
        {
            get
            {
                return Vector3.Distance(start, end);
            }
        }

        public Vector3 FirstPoint
        {
            get
            {
                return start;
            }
        }

        public StraightLineWalker(Vector3 start, Vector3 end, float speed)
        {
            this.start = start;
            this.end = end;
            this.Speed = speed;
        }

        public Vector3 GetPoint(float relativeTime)
        {
            return start + (end - start) * relativeTime;
        }

    }
}
