using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Engine
{
    public class OctahedronMesh : TexturedMesh<VertexPositionNormalTexture>
    {
            public OctahedronMesh(GraphicsDevice graphicsDevice)
                : base(graphicsDevice)
            {
                Initialize();
            }

            protected override void CreateGeometry()
            {
            float halfSideLength = 0.5f;
            float offset = 0.0f;

            #region Positions
            Vector3 top = new Vector3(offset, halfSideLength + offset, offset);
            Vector3 bottom = new Vector3(offset, -halfSideLength + offset, offset);
            Vector3 back = new Vector3(offset, offset, -halfSideLength + offset);
            Vector3 front = new Vector3(offset, offset, halfSideLength + offset);
            Vector3 left = new Vector3(-halfSideLength + offset, offset, offset);
            Vector3 right = new Vector3(halfSideLength + offset, offset, offset);
            #endregion Positions

            #region UVs
            Vector2 uvTop = new Vector2(0.5f, 0);
            Vector2 uvBottom = new Vector2(0.5f, 1);
            Vector2 uvLeft = new Vector2(0.25f, 0.5f);
            Vector2 uvRight = new Vector2(0.75f, 0.5f);
            Vector2 uvFront = new Vector2(0.5f, 0.5f);
            Vector2 uvLeftBack = new Vector2(0.0f, 0.5f);
            Vector2 uvRightBack = new Vector2(1.0f, 0.5f);
            #endregion UVs


            vertices = new VertexPositionNormalTexture[]
                {
                    new VertexPositionNormalTexture(top, Vector3.UnitY, uvTop),
                    new VertexPositionNormalTexture(bottom, -Vector3.UnitY, uvBottom),
                    new VertexPositionNormalTexture(left, -Vector3.UnitX, uvLeft),
                    new VertexPositionNormalTexture(front, Vector3.UnitZ, uvFront),
                    new VertexPositionNormalTexture(right, Vector3.UnitX, uvRight),
                    new VertexPositionNormalTexture(back, -Vector3.UnitZ, uvLeftBack),
                    new VertexPositionNormalTexture(back, -Vector3.UnitZ, uvRightBack),
                };

                indices = new ushort[]
                {
                3,1,2,
                4,1,3,
                6,1,4,
                2,1,5,
                0,3,2,
                0,4,3,
                0,6,4,
                0,2,5
                };
            }
    }
}

