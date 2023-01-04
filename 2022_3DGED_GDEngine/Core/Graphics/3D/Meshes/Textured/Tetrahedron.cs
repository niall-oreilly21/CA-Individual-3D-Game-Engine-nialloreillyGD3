using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


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
                var dir = Vector3.Cross(b - a, c - a);
                var normal = Vector3.Normalize(dir);
                return normal;
            }
        }
    }

    public class Tetrahedron : TexturedMesh<VertexPositionNormalTexture>
    {
        public Tetrahedron(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            Initialize();
        }

        protected override void CreateGeometry()
        {
            #region Positions

            //Vector3 buttomLeft = new Vector3(0f, 0f, 0f);
            //Vector3 bottomRight = new Vector3(1f, 0f, 0f);
            //Vector3 back = new Vector3(0.5f, 0f, 0.8f);
            //Vector3 top = new Vector3(0.5f, 0.8f, 0.5f);

            Vector3 buttomLeft = new Vector3(-0.5f, -0.5f, -0.5f);
            Vector3 bottomRight = new Vector3(0.5f, -0.5f, -0.5f);
            Vector3 back = new Vector3(0f, -0.5f, 0.5f);
            Vector3 top = new Vector3(0f, 0.5f, 0f);

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

            Vector3 frontNormal = new Triangle(buttomLeft, top, bottomRight).Normal;
            Vector3 bottomNormal = new Triangle(buttomLeft, bottomRight, back).Normal;
            Vector3 leftNormal = new Triangle(buttomLeft, top, back).Normal;
            Vector3 rightNormal = new Triangle(bottomRight, back, top).Normal;


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
                new VertexPositionNormalTexture(buttomLeft,leftNormal,frontBottomLeft),
                new VertexPositionNormalTexture(top,frontNormal,frontBottomRight),
                new VertexPositionNormalTexture(back ,bottomNormal,frontTopLeft),
                


                // Back Surface                
                new VertexPositionNormalTexture(bottomRight,rightNormal,backBottomRight),
                //new VertexPositionNormalTexture(top,bottomNormal,backBottomLeft),
                //new VertexPositionNormalTexture(back,bottomNormal,backTopLeft),

                //// Left Surface
                //new VertexPositionNormalTexture(buttomLeft,leftNormal,leftBottomLeft),
                //new VertexPositionNormalTexture(top,leftNormal,leftTopLeft),
                //new VertexPositionNormalTexture(bottomRight,leftNormal,leftBottomRight),

                //// Right Surface
                //new VertexPositionNormalTexture(buttomLeft,rightNormal,rightBottomLeft),              
                //new VertexPositionNormalTexture(bottomRight,rightNormal,rightBottomRight),
                //new VertexPositionNormalTexture(back,rightNormal,rightTopLeft),

            };

            //Vector3 buttomLeft = new Vector3(0f, 0f, 0f); 0
            //Vector3 bottomRight = new Vector3(1f, 0f, 0f); 1
            //Vector3 back = new Vector3(0.5f, 0f, 0.8f); 2
            //Vector3 top = new Vector3(0.5f, 0.8f, 0.5f); 3

            indices = new ushort[] {
                0, 1, 2,
                3, 2, 1,
                0, 2, 3,
                0, 3, 1
       
            };
        }
    }
}
