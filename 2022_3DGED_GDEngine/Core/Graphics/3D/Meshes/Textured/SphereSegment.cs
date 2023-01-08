using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Engine
{
    public class SphereSegment
    {
        private float x;
        private float y;
        private float z;

        public SphereSegment(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 Normal
        {
            
            get
            {
                float lengthOfSegment = FindLengthOfSegment();
                var normal = new Vector3(x / lengthOfSegment, y / lengthOfSegment, z / lengthOfSegment);
                return normal;
            }
        }

        private float FindLengthOfSegment()
        {
            float lengthOfSegment = (float)MathF.Sqrt(MathF.Pow(x, 2) + MathF.Pow(y, 2) + MathF.Pow(z, 2));          
            return lengthOfSegment;
        }
    }
}
