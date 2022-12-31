using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GD.Engine
{
    public class Tetrahedron : TexturedMesh<VertexPositionNormalTexture>
    {
        public Tetrahedron(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            Initialize();
        }

        protected override void CreateGeometry()
        {
            #region Positions

            Vector3 buttomLeft = new Vector3(0f, 0f, 0f);
            Vector3 bottomRight = new Vector3(1f, 0f, 0f);
            Vector3 back = new Vector3(0.5f, 0f, 0.8f);
            Vector3 top = new Vector3(0.5f, 0.8f, 0.4f);



            #endregion Positions

            #region UVs

            Vector2 TtopLeftBack = new Vector2(0.0f, 0.0f);
            Vector2 TtopRightBack = new Vector2(1.0f, 0.0f);
            Vector2 TtopLeftFront = new Vector2(0.0f, 1.0f);
            Vector2 TtopRightFront = new Vector2(1.0f, 1.0f);

            Vector2 TbottomLeftBack = new Vector2(1.0f, 1.0f);
            Vector2 TbottomLeftFront = new Vector2(0.0f, 1.0f);
            Vector2 TbottomRightBack = new Vector2(1.0f, 0.0f);
            Vector2 TbottomRightFront = new Vector2(0.0f, 0.0f);

            Vector2 frontTopLeft = new Vector2(0.0f, 0.0f);
            Vector2 frontTopRight = new Vector2(1.0f, 0.0f);
            Vector2 frontBottomLeft = new Vector2(0.0f, 1.0f);
            Vector2 frontBottomRight = new Vector2(1.0f, 1.0f);

            Vector2 backTopLeft = new Vector2(0.0f, 0.0f);
            Vector2 backTopRight = new Vector2(1.0f, 0.0f);
            Vector2 backBottomLeft = new Vector2(0.0f, 1.0f);
            Vector2 backBottomRight = new Vector2(1.0f, 1.0f);

            Vector2 rightTopLeft = new Vector2(0.0f, 0.0f);
            Vector2 rightTopRight = new Vector2(1.0f, 0.0f);
            Vector2 rightBottomLeft = new Vector2(0.0f, 1.0f);
            Vector2 rightBottomRight = new Vector2(1.0f, 1.0f);

            Vector2 leftTopLeft = new Vector2(0.0f, 0.0f);
            Vector2 leftTopRight = new Vector2(1.0f, 0.0f);
            Vector2 leftBottomLeft = new Vector2(0.0f, 1.0f);
            Vector2 leftBottomRight = new Vector2(1.0f, 1.0f);

            #endregion UVs

            #region Normals

            Vector3 frontNormal = new Vector3(0, 1, -1);
            Vector3 bottomNormal = new Vector3(0, -1, 0);
            Vector3 leftNormal = new Vector3(-1, 0, 1);
            Vector3 rightNormal = new Vector3(1, 0, 1);


            #endregion Normals

            vertices = new VertexPositionNormalTexture[]
            {

            //Vector3 buttomLeftFront = new Vector3(0f, 0f, 0f);
            //Vector3 bottomRightBack = new Vector3(1f, 0f, 1f);
            //Vector3 topLeftBack = new Vector3(0f, 1f, 1f);
            //Vector3 topRightFront = new Vector3(1f, 1f, 0f);

            //  Vector3 buttomLeft = new Vector3(0f, 0f, 0f);
            //Vector3 bottomRight = new Vector3(1f, 0f, 0f);
            //Vector3 back = new Vector3(0.5f, 0f, 0.8f);
            //Vector3 top = new Vector3(0.5f, 0.8f, 0.4f);

                // Front Surface
                new VertexPositionNormalTexture(buttomLeft,frontNormal,frontBottomLeft),
                new VertexPositionNormalTexture(back ,frontNormal,frontTopLeft),
                new VertexPositionNormalTexture(top,frontNormal,frontBottomRight),


                // Back Surface                
                new VertexPositionNormalTexture(bottomRight,bottomNormal,backBottomRight),
                new VertexPositionNormalTexture(top,bottomNormal,backBottomLeft),
                new VertexPositionNormalTexture(back,bottomNormal,backTopLeft),

                // Left Surface
                new VertexPositionNormalTexture(buttomLeft,leftNormal,leftBottomLeft),
                new VertexPositionNormalTexture(top,leftNormal,leftTopLeft),
                new VertexPositionNormalTexture(bottomRight,leftNormal,leftBottomRight),

                // Right Surface
                new VertexPositionNormalTexture(buttomLeft,rightNormal,rightBottomLeft),              
                new VertexPositionNormalTexture(bottomRight,rightNormal,rightBottomRight),
                new VertexPositionNormalTexture(back,rightNormal,rightTopLeft),

            };

            indices = new ushort[] {
                0, 1, 2,
                3, 2, 1,
                0, 2, 3,
                0, 3, 1
       
            };
        }
    }
}
