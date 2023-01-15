using GD.App;
using GD.Engine.Events;
using GD.Engine.Globals;
using JigLibX.Collision;
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
        private Keys pressedKey = Keys.M;
        private bool pressed = false;
        private List<GameObject> cameras = new List<GameObject>();
        public SnakeCameraController()
        {
        }

        public override void Update(GameTime gameTime)
        {
            HandleKeyboardInput(gameTime);
        }


        private float totalTime = 2000f;
        private float currentTime = 0f;
        private void HandleKeyboardInput(GameTime gameTime)
        {


            //if (Input.Keys.WasJustPressed(pressedKey))
            //{
            //    System.Diagnostics.Debug.WriteLine("Pressed");
            //    pressed = false;

            //}

            //if (Input.Keys.IsPressed(Keys.Up))
            //{
 
            //        if (totalTime >= 2000f)
            //        {
            //            Application.CameraManager.SetActiveCamera(AppData.TOP_CAMERA_NAME);
            //            pressed = true;
            //            pressedKey = Keys.Up;
            //        }

            //        if (currentTime <= 0f)
            //        {

            //            Application.CameraManager.SetActiveCamera(AppData.BACK_CAMERA_NAME);
            //            pressed = true;
            //            pressedKey = Keys.Up;
            //        }
                    

            //        System.Diagnostics.Debug.WriteLine(pressedKey);
  
  
            //}


            //if(pressedKey == Keys.Up)
            //{
            //    if (Application.CameraManager.ActiveCameraName == AppData.FRONT_CAMERA_NAME)
            //    {
            //        Application.CameraManager.SetActiveCamera(AppData.TOP_CAMERA_NAME);
            //    }

            //    else if (Application.CameraManager.ActiveCameraName == AppData.TOP_CAMERA_NAME)
            //    {

            //        Application.CameraManager.SetActiveCamera(AppData.BACK_CAMERA_NAME);
            //    }
            //}




            //    //pressedKey = Keys.Up;
            //}

            //else if (Input.Keys.WasJustPressed(Keys.Down))
            //{

            //    if (Application.CameraManager.ActiveCameraName == AppData.FRONT_CAMERA_NAME)
            //    {
            //        Application.CameraManager.SetActiveCamera(AppData.BACK_CAMERA_NAME);
            //    }

            //}

            //if (Input.Keys.IsPressed(Keys.Left))
            //{
            //    pressedKey = Keys.Left;
            //}

            //if (Input.Keys.IsPressed(Keys.G))
            //{
            //    pressedKey = Keys.G;
            //}

            //if (Input.Keys.IsPressed(Keys.J))
            //{
            //    pressedKey = Keys.J;
            //}
            //if (transform.Translation.X > 25f && !isturned)
            //{

            //    //transform.Translate(0, 0, -0.3f);

            //    if (transform.Rotation.Y > 90)
            //    {
            //        isturned = true;
            //    }
            //}

            //if (!pressed)
            //{
            //    if (pressedKey == Keys.G)
            //    {
            //        turnRight(gameTime);
            //    }

            //    if(pressedKey == Keys.J)
            //    {
            //        GoUp(gameTime);
            //    }
            //}


        }

        //        //stored in the object you're rotating
        //        private float rotation = 0f;
        //        private float targetRotation = 90f;
        //        private float rotateSpeed = 0.1f;

        //        private float translation = 0f;
        //        private float targetTranslation = 40f;


        //        private void turnRight(GameTime gameTime)
        //        {

        ////inside our update for the object check the rotation to see if we're where we want to be
        //            float rotationAmt = (float)(rotateSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);

        //            float translationAmount = (float)(rotateSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);

        //            if (rotation < targetRotation)
        //            {
        //                    transform.Rotate(0, rotationAmt, 0);
        //                    rotation += rotationAmt;         
        //            }


        //            if(translation < targetTranslation)
        //            {

        //                if(transform.Rotation.Y >= 360)
        //                {
        //                    transform.SetRotation(transform.Rotation.X, 0.8f, transform.Rotation.Z);
        //                }

        //                //if (transform.Rotation.Y >= 180)
        //                //{
        //                //    transform.Translate(0, 0, translationAmount);
        //                //}
        //                else if(transform.Rotation.Y >= 0 && transform.Rotation.Y < 90)
        //                {
        //                    transform.Translate(translationAmount, 0, -translationAmount);
        //                }
        //                else if(transform.Rotation.Y >= 180 && transform.Rotation.Y < 270)
        //                {
        //                    transform.Translate(-translationAmount, 0, translationAmount);
        //                }
        //                else if (transform.Rotation.Y >= 270 && transform.Rotation.Y < 360)
        //                {
        //                    transform.Translate(translationAmount, 0, translationAmount);
        //                }
        //                else
        //                {
        //                    transform.Translate(-translationAmount, 0, -translationAmount);
        //                }

        //                translation += translationAmount;
        //            }


        //            if(rotation > targetRotation && translation > targetTranslation)
        //            {
        //                translation = 0;
        //                rotation = 0;
        //                pressed = true;
        //            }
        //            //if()
        //        }

        //        private void GoLeft(GameTime gameTime)
        //        {

        //            System.Diagnostics.Debug.WriteLine("here");
        //            //inside our update for the object check the rotation to see if we're where we want to be
        //            float rotationAmt = (float)(rotateSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);

        //            float translationAmount = (float)(rotateSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);

        //            if (rotation < targetRotation)
        //            {
        //                transform.Rotate(0, -rotationAmt, 0);
        //                rotation += rotationAmt;
        //            }


        //            if (translation < targetTranslation)
        //            {

        //                if (transform.Rotation.Y >= 360)
        //                {
        //                    transform.SetRotation(transform.Rotation.X, 0.8f, transform.Rotation.Z);
        //                }

        //                if (transform.Rotation.Y >= 180)
        //                {

        //                    transform.Translate(0, 0, -translationAmount);
        //                }
        //                else if (transform.Rotation.Y >= 0)
        //                {
        //                    transform.Translate(0, 0, translationAmount);
        //                }

        //                translation += translationAmount;
        //            }


        //            if (rotation > targetRotation && translation > targetTranslation)
        //            {
        //                translation = 0;
        //                rotation = 0;
        //                pressed = true;
        //            }
        //            //if()
        //        }

        //        private void GoUp(GameTime gameTime)
        //        {

        //            System.Diagnostics.Debug.WriteLine("here");
        //            //inside our update for the object check the rotation to see if we're where we want to be
        //            float rotationAmt = (float)(rotateSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);

        //            float translationAmount = (float)(rotateSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);

        //            if (rotation < targetRotation)
        //            {
        //                transform.Rotate(0, 0, -rotationAmt);
        //                rotation += rotationAmt;
        //            }

        //            if (transform.Rotation.Z >= 360 || transform.Rotation.Z <= -360)
        //            {
        //                transform.SetRotation(transform.Rotation.X, transform.Rotation.Y, 0f);
        //            }


        //            if (rotation > targetRotation)
        //            {
        //                translation = 0;
        //                rotation = 0;
        //                pressed = true;
        //            }
        //            //if()
        //        }
        //    }
    }
}
