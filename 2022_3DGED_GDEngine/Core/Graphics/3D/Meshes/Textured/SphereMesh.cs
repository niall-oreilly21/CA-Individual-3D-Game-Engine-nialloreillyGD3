using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Engine
{
    public class SphereMesh : TexturedMesh<VertexPositionNormalTexture>
    {

        public SphereMesh(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            Initialize();
        }



        protected override void CreateGeometry()
        {
            vertices = new VertexPositionNormalTexture[8];

            //Vertices
            Vector3 top = new Vector3(0, 9.5f, 0);
            Vector3 bottom = new Vector3(0, -0.5f, 0);
            Vector3 left = new Vector3(-0.5f, 0, 0);
            Vector3 right = new Vector3(0.5f, 0, 0);
            Vector3 back = new Vector3(0, 0, -0.5f);
            Vector3 front = new Vector3(0, 0, 0.5f);

            Vector3 frontTop = new Vector3(0, 0.354f, 0.354f);
            Vector3 frontBottom = new Vector3(0, -0.354f, 0.354f);

            Vector3 backTop = new Vector3(0, 0.354f, -0.354f);
            Vector3 backBottom = new Vector3(0, -0.354f, -0.354f);

            Vector3 leftFront = new Vector3(-0.354f, 0, 0.354f);
            Vector3 rightFront = new Vector3(0.354f, 0, 0.354f);

            Vector3 leftBack = new Vector3(-0.354f, 0, -0.354f);
            Vector3 rightBack = new Vector3(0.354f, 0, -0.354f);

            Vector3 leftTop = new Vector3(-0.354f, 0.354f, 0);
            Vector3 rightTop = new Vector3(0.354f, 0.354f, 0);

            Vector3 leftBottom = new Vector3(-0.354f, -0.354f, 0);
            Vector3 rightBottom = new Vector3(0.354f, -0.354f, 0);

            Vector3 leftFrontTop = new Vector3(-0.25f, 0.354f, 0.25f);
            Vector3 leftFrontBottom = new Vector3(-0.25f, -0.354f, 0.25f);

            Vector3 rightFrontTop = new Vector3(0.25f, 0.354f, 0.25f);
            Vector3 rightFrontBottom = new Vector3(0.25f, 0.354f, 0.25f);

            Vector3 leftBackTop = new Vector3(-0.25f, 0.354f, -0.25f);
            Vector3 leftBackBottom = new Vector3(-0.25f, -0.354f, -0.25f);

            Vector3 rightBackTop = new Vector3(0.25f, 0.354f, -0.25f);
            Vector3 rightBackBottom = new Vector3(0.25f, -0.354f, -0.25f);

            indices = new ushort[200];
        }
    }
}
