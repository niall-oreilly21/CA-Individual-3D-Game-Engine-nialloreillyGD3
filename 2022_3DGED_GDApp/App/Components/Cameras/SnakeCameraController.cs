using GD.Engine.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace GD.Engine
{
    public class SnakeCameraController : Component
    {
        private static readonly int ROUND_PRECISION = 4;
        private Vector3 rotationAxis;
        private float maxAngleInDegrees;
        private float angularSpeedMultiplier;
        private TurnDirectionType turnDirectionType;
        private Keys pressedKey;
        private bool pressed = true;

        public SnakeCameraController(
            Vector3 rotationAxis,
            float maxAngleInDegrees,
            float angularSpeedMultiplier,
            TurnDirectionType turnDirectionType)
        {
            this.rotationAxis = Vector3.Normalize(rotationAxis);
            this.maxAngleInDegrees = maxAngleInDegrees;
            this.angularSpeedMultiplier = angularSpeedMultiplier;
            this.turnDirectionType = turnDirectionType;
        }
        private bool isturned;
        public override void Update(GameTime gameTime)
        {
            HandleKeyboardInput(gameTime);
            base.Update(gameTime);
        }

        private float totalTime = 2000f;
        private float currentTime;
        private void HandleKeyboardInput(GameTime gameTime)
        {


            if (Input.Keys.WasJustPressed(pressedKey))
            {
                pressed = false;
            }


            if (Input.Keys.IsPressed(Keys.Up))
            {
                transform.Rotate(0, 0.7f, 0);
                pressedKey = Keys.Up;
            }

            if (Input.Keys.IsPressed(Keys.Down))
            {
                transform.Rotate(0, 0.7f, 0);

                pressedKey = Keys.Down;
            }

            if (Input.Keys.IsPressed(Keys.Left))
            {
                transform.Rotate(0, 0.7f, 0);
                pressedKey = Keys.Left;
            }

            if (Input.Keys.IsPressed(Keys.G))
            {
                pressedKey = Keys.G;
            }
            //if (transform.Translation.X > 25f && !isturned)
            //{

            //    //transform.Translate(0, 0, -0.3f);

            //    if (transform.Rotation.Y > 90)
            //    {
            //        isturned = true;
            //    }
            //}

            if(!pressed)
            {
                if (pressedKey == Keys.G)
                {
                    turnRight(gameTime);

                }
            }

            
        }

        //stored in the object you're rotating
        private float rotation = 0;
        private float targetRotation = 90f;
        private float rotateSpeed = 0.05f;

        private float translation = 0;
        private float targetTranslation = 5f;
     

        private void turnRight(GameTime gameTime)
        {
            System.Diagnostics.Debug.WriteLine(rotation);

//inside our update for the object check the rotation to see if we're where we want to be
            float rotationAmt = (float)(rotateSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);

            float translationAmount = (float)(rotateSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);

            if (rotation < targetRotation)
            {
                    transform.Rotate(0, rotationAmt, 0);
                    rotation += rotationAmt;         
            }




           else  if(rotation > targetRotation)
            {
                System.Diagnostics.Debug.WriteLine(translation);
                rotation = 0;
                if (translation < targetTranslation)
                {
                    transform.Translate(translationAmount, 0, -translationAmount);
                    translation = translationAmount;
                }
                else if(translation > targetTranslation)
                {
                    translation = 0;

                    pressed = true;
                }

              
            }



            //if()


        }
    }
}
