﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GD.Engine
{
    public class QuadMesh : TexturedMesh<VertexPositionNormalTexture>
    {
        public QuadMesh(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            Initialize();
        }

        protected override void CreateGeometry()
        {
            vertices = new VertexPositionNormalTexture[]
            {
                new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, 0), Vector3.UnitZ, new Vector2(0,0)), //0
                new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, 0), Vector3.UnitZ, new Vector2(1,0)), //1
                new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0), Vector3.UnitZ, new Vector2(0,1)), //2
                new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, 0), Vector3.UnitZ, new Vector2(1,1)), //3

                new VertexPositionNormalTexture(new Vector3(0f, 0f, 0.5f), new Vector3(1,0,1), new Vector2(1,1)), //4
            };

            indices = new ushort[]
            {
                0,1,2, //top left
                2,1,3,  //bottom right
                2,4,0
            };
        }
    }
}