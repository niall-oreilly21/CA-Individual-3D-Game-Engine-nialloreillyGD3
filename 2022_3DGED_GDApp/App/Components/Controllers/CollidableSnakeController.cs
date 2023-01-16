﻿using GD.App;
using GD.Engine;
using GD.Engine.Events;
using GD.Engine.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Engine
{
    class CollidableSnakeController : SnakeController
    {
        private bool pressed;
        private Keys pressedKey;
        private Keys previousKey;
        private Vector3 direction;
        private Dictionary<string, CameraKeys> cameraKeysDictionary;
        private CameraKeys activeCameraKeys;

        #region Properties
        public Vector3 Direction
        {
            get 
            { 
                return direction; 
            }
            set
            {
                direction = value;
            }
        }
        #endregion Properties

        public CollidableSnakeController(Dictionary<string, CameraKeys> cameraKeysDictionary)
        {
            pressed = false;
            direction = new Vector3(AppData.SCALE_AMOUNT, 0, 0);
            this.cameraKeysDictionary = cameraKeysDictionary;
            this.activeCameraKeys = cameraKeysDictionary[AppData.FRONT_CAMERA_NAME];
        }


        #region Actions - Update, Input

        public override void Update(GameTime gameTime)
        {
            HandleKeyboardInput(gameTime);
            HandleCameraSwitch();
        }

        private void HandleCameraSwitch()
        {
            if(Application.CameraManager.ActiveCamera.gameObject.Name != AppData.INTRO_CURVE_CAMERA_NAME)
            {
                this.activeCameraKeys = cameraKeysDictionary[Application.CameraManager.ActiveCamera.gameObject.Name];
            }
            
        }

        public virtual void HandleKeyboardInput(GameTime gameTime)
        {
            if (Input.Keys.WasJustPressed(pressedKey))
            {
                pressed = false;
            }

            translation = Vector3.Zero;

            if (Input.Keys.IsPressed(activeCameraKeys.Forward))
            {
                if (!pressed)
                {             
                    pressedKey = activeCameraKeys.Forward;
                    pressed = true;
                    if (previousKey != activeCameraKeys.Backward)
                    {
                        previousKey = activeCameraKeys.Forward;
                        transform.SetRotation(0, 0, 0);
                        direction = transform.World.Forward;                        
                    }

                }
            }

            else if (Input.Keys.IsPressed(activeCameraKeys.Backward))
            {
                if (!pressed)
                {                 
                    pressedKey = activeCameraKeys.Backward;
                    pressed = true;
                    if (previousKey != activeCameraKeys.Forward)
                    {
                        previousKey = activeCameraKeys.Backward;
                        transform.SetRotation(0, 0, 0);
                        direction = transform.World.Backward;
                    }

                }
            }
            else if (Input.Keys.IsPressed(activeCameraKeys.Left))
            {
                if (!pressed)
                {
                    
                    pressedKey = activeCameraKeys.Left;
                    pressed = true;
                    if (previousKey != activeCameraKeys.Right)
                    {                   
                        previousKey = activeCameraKeys.Left;
                        transform.SetRotation(0, 0, 0);
                        direction = transform.World.Left;
                        transform.SetRotation(0, 90, 0);
                    }
                    
                }
                
            }
            else if (Input.Keys.IsPressed(activeCameraKeys.Right))
            {
                if (!pressed)
                {
                    
                    pressedKey = activeCameraKeys.Right;
                    pressed = true;
                    if (previousKey != activeCameraKeys.Left)
                    {
                        previousKey = activeCameraKeys.Right;
                        transform.SetRotation(0, 0, 0);
                        direction = transform.World.Right;
                        transform.SetRotation(0, 90, 0);
                    }                  
                }

            }
            else if (Input.Keys.IsPressed(Keys.Q))
            {
                if (!pressed)
                {
                    
                    pressedKey = Keys.Q;
                    pressed = true;
                    if (previousKey != Keys.E)
                    {
                        previousKey = Keys.Q;
                        transform.SetRotation(0, 0, 0);
                        direction = transform.World.Down;
                        transform.SetRotation(0, 90, 0);
                    }
                }
            }

            else if (Input.Keys.IsPressed(Keys.E))
            {
                if (!pressed)
                {
                    
                    pressedKey = Keys.E;
                    pressed = true;
                    if (previousKey != Keys.Q)
                    {
                        previousKey = Keys.E;
                        transform.SetRotation(0, 0, 0);
                        direction = transform.World.Up;
                        transform.SetRotation(0, 90, 0);
                    }
                }
            }

            object[] parameters = { direction, gameTime};
            EventDispatcher.Raise(new EventData(EventCategoryType.SnakeManager,
            EventActionType.OnMove, parameters));

            object[] parametersTwo = {direction};
            EventDispatcher.Raise(new EventData(EventCategoryType.SnakeManager,
            EventActionType.MoveTongue, parametersTwo));

        }
        #endregion Actions - Update, Input
    }
}
