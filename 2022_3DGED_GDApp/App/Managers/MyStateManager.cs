using GD.Engine.Events;
using GD.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GD.Engine.Globals;
using GD.App;
using GD.Engine.Managers;
using SharpDX.MediaFoundation.DirectX;
using System;
using static System.Net.Mime.MediaTypeNames;
using Application = GD.Engine.Globals.Application;

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
    public class MyStateManager : PausableGameComponent
    {
        private float maxTimeInMS;
        private float totalElapsedTimeMS;
        private float totalElapsedTimeCameraRotationMS;
        private int currentLevel;
        private int currentScore;
        private int totalLevels;
        private string text;

        public MyStateManager(Game game, float maxTimeInMS, int totalLevels) : base(game)
        {
            this.maxTimeInMS = maxTimeInMS;
            this.totalElapsedTimeMS = 0;
            this.totalElapsedTimeCameraRotationMS = 0;
            this.currentLevel = 1;
            this.totalLevels = totalLevels;
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

        protected override void SubscribeToEvents()
        {
            EventDispatcher.Subscribe(EventCategoryType.StateManager, HandleGameObjectEvents);
        }

        protected void HandleGameObjectEvents(EventData eventData)
        {
            switch (eventData.EventActionType)
            { 
                case EventActionType.UpdateScore: //TODO
                    UpdateScore();
                    break;

                default:
                    break;
                    //add more cases for each method that we want to support with events
            }
        }

        public override void Update(GameTime gameTime)
        {
            //System.Diagnostics.Debug.WriteLine(totalElapsedTimeMS);
            //totalElapsedTimeMS = 0;
            //currentLevel++;
            //string text = "Level: " + Application.StateManager.CurrentLevel;
            //object[] parameters = { text };
            totalElapsedTimeMS += gameTime.ElapsedGameTime.Milliseconds;

            if (totalElapsedTimeMS >= maxTimeInMS)
            {
                StartNewLevel();       
            }


            //CheckCurrentLevel();
            // base.Update(gameTime);
        }

        private void UpdateScore()
        {
            GameObject scoreGameObject = Application.UISceneManager.ActiveScene.Find((uiElement) => uiElement.Name == AppData.SCORE_TEXT);

            var material2D = (TextMaterial2D)scoreGameObject.GetComponent<Renderer2D>().Material;
            material2D.StringBuilder.Clear();
            material2D.StringBuilder.Append(AppData.DEFAULT_SCORE_TEXT + Application.StateManager.CurrentScore);
        }

        private void UpdateLevel()
        {
            GameObject levelGameObject = Application.UISceneManager.ActiveScene.Find((uiElement) => uiElement.Name == AppData.DEFAULT_LEVEL_TEXT);

            var material2D = (TextMaterial2D)levelGameObject.GetComponent<Renderer2D>().Material;
            material2D.StringBuilder.Clear();
            material2D.StringBuilder.Append(AppData.DEFAULT_LEVEL_TEXT + Application.StateManager.CurrentLevel);
        }

        private void StartNewLevel()
        {

            totalElapsedTimeMS = 0;            

            Application.StateManager.CurrentLevel++;

            if(currentLevel == 2)
            {
                EventDispatcher.Raise(new EventData(EventCategoryType.Player,
                  EventActionType.InitilizeBombManager));
            }
           

        }

        private void CheckCurrentLevel()
        {
            if(currentLevel > totalLevels)
            {
                EventDispatcher.Raise(new EventData(EventCategoryType.Menu,
                EventActionType.OnPause));

                EventDispatcher.Raise(new EventData(EventCategoryType.Player,
                EventActionType.OnLose));
            }
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