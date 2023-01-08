using GD.Engine.Events;
using GD.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GD.Engine.Globals;
using GD.App;
using GD.Engine.Managers;
using SharpDX.MediaFoundation.DirectX;
using System;

namespace App.Managers
{
    public enum ItemType : sbyte
    {
        Story,
        Health,
        Ammo,
        Quest,
        Prop
    }

    public class InventoryItem
    {
        public string uniqueID;
        public string name;
        public ItemType itemType;
        public string description;
        public int value;
        public string cueName;  //"boom"
    }

    /// <summary>
    /// Countdown/up timer and we need an inventory system
    /// </summary>
    public class MyStateManager : GameComponent
    {
        private float maxTimeInMS;
        private float totalElapsedTimeMS;
        private float totalElapsedTimeCameraRotationMS;
        private int currentLevel;
        private int currentScore;

        public MyStateManager(Game game, float maxTimeInMS) : base(game)
        {
            this.maxTimeInMS = maxTimeInMS;
            this.totalElapsedTimeMS = 0;
            this.totalElapsedTimeCameraRotationMS = 0;
            this.currentLevel = 1;
        }

        #region Properties
        public int CurrentLevel
        {
            get
            {
                return currentLevel;
            }
            set
            {
                currentLevel = value;
            }
        }

        public int CurrentScore
        {
            get
            {
                return currentScore;
            }
            set
            {
                currentScore = value;
            }
        }
        #endregion Properties

        public override void Update(GameTime gameTime)
        {
            totalElapsedTimeMS += gameTime.ElapsedGameTime.Milliseconds;

            if (totalElapsedTimeMS >= maxTimeInMS)
            {
                totalElapsedTimeMS = 0;
                currentLevel++;
                string text = "Level: " + Application.StateManager.CurrentLevel;
                object[] parameters = { text };

                EventDispatcher.Raise(new EventData(EventCategoryType.UpdateUIElements,
                    EventActionType.UpdateUI, parameters));
            }

            if(CheckCameraRotateStart())
            {
                UpdateCameraRotataion(gameTime);
            }

            //check game state
            //if win then
            //CheckWinLose()
            //show win toast
            //if lose then
            //show lose toast
            //fade to black
            //show restart screen

            base.Update(gameTime);
        }

        private bool CheckWinLose()
        {
            return false;
            //check individual game items
        }

        private bool CheckCameraRotateStart()
        {
            if(currentLevel >= 2)
            {
                return true;
            }

            return false;
        }
        private float rotation = 0f;
        private float targetRotation = 90f;
        private float rotateSpeed = 0.1f;
       
        private void UpdateCameraRotataion(GameTime gameTime)
        {
            totalElapsedTimeCameraRotationMS += gameTime.ElapsedGameTime.Milliseconds;


            if (totalElapsedTimeCameraRotationMS >= 5000f)
            {

                    float rotationAmt = (float)(rotateSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);

                    if (rotation < targetRotation)
                    {

                    if (Application.CameraManager.ActiveCameraName == AppData.FRONT_CAMERA_NAME)
                    {
                        Application.CameraManager.ActiveCamera.gameObject.Transform.Rotate(0, 0, rotationAmt);
                    }
                    else if (Application.CameraManager.ActiveCameraName == AppData.BACK_CAMERA_NAME)
                    {
                        Application.CameraManager.ActiveCamera.gameObject.Transform.Rotate(0, 0, -rotationAmt);
                    }

                    else if (Application.CameraManager.ActiveCameraName == AppData.TOP_CAMERA_NAME)
                    {
                        Application.CameraManager.ActiveCamera.gameObject.Transform.Rotate(0, rotationAmt, 0);

                    }
                    else if (Application.CameraManager.ActiveCameraName == AppData.BOTTOM_CAMERA_NAME)
                    {
                        Application.CameraManager.ActiveCamera.gameObject.Transform.Rotate(0, -rotationAmt, 0);

                    }

                    rotation += rotationAmt;
                }
                else
                {
                    rotation = 0f;
                    totalElapsedTimeCameraRotationMS = 0;
                }

            }
            
        }
    }
}