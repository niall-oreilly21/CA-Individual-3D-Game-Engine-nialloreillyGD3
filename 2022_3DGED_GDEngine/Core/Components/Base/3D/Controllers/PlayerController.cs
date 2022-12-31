using GD.Engine.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GD.Engine
{
    /// <summary>
    /// Adds simple non-collidable player controller using keyboard
    /// </summary>
    public class PlayerController : Component
    {
        #region Fields

        protected float moveSpeed = 0.05f;
        protected float strafeSpeed = 0.025f;
        protected Vector2 rotationSpeed;
        private bool isGrounded;
        private bool pressed = false;
        private Keys pressedKey;
        private long keyPressedTime = 0;
        
        #endregion Fields

        #region Temps

        protected Vector3 translation = Vector3.Zero;
        protected Vector3 rotation = Vector3.Zero;

        #endregion Temps

        #region Constructors

        public PlayerController(float moveSpeed, float strafeSpeed, float rotationSpeed, bool isGrounded = true)
    : this(moveSpeed, strafeSpeed, rotationSpeed * Vector2.One, isGrounded)
        {
        }

        public PlayerController(float moveSpeed, float strafeSpeed, Vector2 rotationSpeed, bool isGrounded = true)
        {
            this.moveSpeed = moveSpeed;
            this.strafeSpeed = strafeSpeed;
            this.rotationSpeed = rotationSpeed;
            this.isGrounded = isGrounded;
        }

        #endregion Constructors

        #region Actions - Update, Input

        public override void Update(GameTime gameTime)
        {
            HandleKeyboardInput(gameTime);
        }

        public virtual void HandleKeyboardInput(GameTime gameTime)
        {
            if(Input.Keys.WasJustPressed(pressedKey))
            {
                System.Diagnostics.Debug.WriteLine("what the hell");
                pressed = false;
            }


            translation = Vector3.Zero;
            if (Input.Keys.IsPressed(Keys.W))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.W;
                pressed = true;
                translation.Z = 1;
            }    
                
            else if (Input.Keys.IsPressed(Keys.S))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.S;
                pressed = true;
                translation.Z = -1;
            }

            else if (Input.Keys.IsPressed(Keys.A))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.A;
                pressed = true;
                if (this.gameObject.Transform.Translation.X > 0)
                {
                    translation.X = -1;
                }
                else
                {
                    translation.X = 9;
                }
            }
            else if (Input.Keys.IsPressed(Keys.D))
            {
                
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.D;
                pressed = true;
                if(this.gameObject.Transform.Translation.X < 9)
                    {
                    translation.X = 1;
                }
                else
                {
                    translation.X = -9;
                }

            }

            else if (Input.Keys.IsPressed(Keys.Q))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.Q;
                pressed = true;
                translation.Y = 1;
            }

            else if (Input.Keys.IsPressed(Keys.R))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.R;
                pressed = true;
                translation.Y = -1;
            }

            //if (Input.Keys.IsPressed(Keys.H))
            //   // transform.Rotate();
            //else if (Input.Keys.IsPressed(Keys.K))
            //   // transform.Rotate();

            //if (isGrounded)
              //  translation.Y = 0;

            //actually apply the movement

            transform.Translate(translation);
        }

        #endregion Actions - Update, Input
    }
}