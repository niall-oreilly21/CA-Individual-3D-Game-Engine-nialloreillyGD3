using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Engine
{
    class Triangle
    {
        private Vector3 a;
        private Vector3 b;
        private Vector3 c;

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public Vector3 Normal
        {
            get
            {
                var direction = Vector3.Cross(b - a, c - a);
                var normal = Vector3.Normalize(direction);
                return normal;
            }
        }
    }

}
