using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

// ref: https://github.com/jamesfarrell97/Assess/blob/51e2c5799400fd94fa77994488a4e23f5ab88aa2/GDLibrary/GDLibrary/Factory/VertexFactory.cs#L70

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
            int slices = 12;
            float radius = 0.5f;
            int numberOfVertices = (slices + 1) * (slices + 1);
            int numberOfIndices = 6 * slices * (slices + 1);

            indices = new ushort[numberOfIndices];
            vertices = new VertexPositionNormalTexture[numberOfVertices];

            float thetaStep = MathHelper.Pi / slices;
            float phiStep = MathHelper.TwoPi / slices;

            int iIndex = 0;
            int iVertex = 0;
            int iVertexTwo = 0;

            for (int sliceTheta = 0; sliceTheta < slices + 1; sliceTheta++)
            {
                float r = (float)Math.Sin(sliceTheta * thetaStep);
                float y = (float)Math.Cos(sliceTheta * thetaStep);

                for (int slicePhi = 0; slicePhi < (slices + 1); slicePhi++)
                {
                    float x = r * (float)Math.Sin(slicePhi * phiStep);
                    float z = r * (float)Math.Cos(slicePhi * phiStep);


                    vertices[iVertex].Position = new Vector3(x, y, z) * radius;
                    vertices[iVertex].Normal = new SphereSegment(x, y, z).Normal;
                    vertices[iVertex].TextureCoordinate = new Vector2((float)slicePhi / slices, (float)sliceTheta / slices);
                    iVertex++;

                    if (sliceTheta != (slices - 1))
                    {
                        indices[iIndex++] = (ushort)(iVertexTwo + (slices + 1));
                        indices[iIndex++] = (ushort)(iVertexTwo + 1);
                        indices[iIndex++] = (ushort)(iVertexTwo);
                        indices[iIndex++] = (ushort)(iVertexTwo + (slices));
                        indices[iIndex++] = (ushort)(iVertexTwo + (slices + 1));
                        indices[iIndex++] = (ushort)(iVertexTwo);
                        iVertexTwo++;
                    }
                }
            }
        }

    }
}
