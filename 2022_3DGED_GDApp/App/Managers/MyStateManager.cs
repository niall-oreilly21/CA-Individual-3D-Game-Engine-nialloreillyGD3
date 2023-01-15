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
        private double totalSeconds;
        private double minutes;
        private double seconds;
        private int currentLevel;
        private int currentScore;
        private int totalLevels;
        private bool gameStarted;
        private int[] defaultFoodEachLevel;
        private int[] defaultBombsEachLevel;

        public MyStateManager(Game game, float maxTimeInMS, int totalLevels, int[] defaultFoodEachLevel, int[] defaultBombsEachLevel) : base(game)
        {
            this.maxTimeInMS = maxTimeInMS;
            this.totalElapsedTimeMS = 0;
            this.totalSeconds = 0;
            this.minutes = 0;
            this.seconds = 0;
            this.currentLevel = AppData.LEVEL_ONE;
            this.totalLevels = totalLevels;
            this.gameStarted = false;
            this.defaultFoodEachLevel = defaultFoodEachLevel;
            this.defaultBombsEachLevel = defaultBombsEachLevel;
        }

        #region Properties
        public double Minutes
        {
            get
            {
                return minutes;
            }
        }

        public double Seconds
        {
            get
            {
                return seconds;
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

        public int CurrentLevel
        {
            get
            {
                return currentScore;
            }
            set
            {
                currentLevel = value;
            }
        }

        public bool GameStarted
        {
            get
            {
                return gameStarted;
            }
            set
            {
                gameStarted = value;
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
                case EventActionType.UpdateScore:
                    UpdateScore();
                    break;

                case EventActionType.RemoveUILevelStart:                   
                    GameObject removeGameObject = (GameObject)eventData.Parameters[0];
                    StartNewLevel(removeGameObject);
                    break;

                case EventActionType.StartOfLevel:
                    StartOfLevel();
                    break;

                default:
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            totalElapsedTimeMS += gameTime.ElapsedGameTime.Milliseconds;
            totalSeconds = Math.Abs(maxTimeInMS - totalElapsedTimeMS / 1000d);
            minutes = Math.Floor(totalSeconds / 60);
            seconds = Math.Round(totalSeconds % 60);
        }

        private void UpdateScore()
        {
            currentScore++;
            GameObject scoreGameObject = Application.UISceneManager.ActiveScene.Find((uiElement) => uiElement.Name == AppData.SCORE_UI_TEXT_NAME);

            var material2D = (TextMaterial2D)scoreGameObject.GetComponent<Renderer2D>().Material;
            material2D.StringBuilder.Clear();
            material2D.StringBuilder.Append(AppData.DEFAULT_SCORE_TEXT + currentScore);
        }

        private void UpdateLevel()
        {
            GameObject levelGameObject = Application.UISceneManager.ActiveScene.Find((uiElement) => uiElement.Name == AppData.DEFAULT_LEVEL_TEXT);

            var material2D = (TextMaterial2D)levelGameObject.GetComponent<Renderer2D>().Material;
            material2D.StringBuilder.Clear();
            material2D.StringBuilder.Append(AppData.DEFAULT_LEVEL_TEXT + currentLevel);
        }

        private void ResetLevel()
        {        
            gameStarted = false;
            EventDispatcher.Raise(new EventData(EventCategoryType.SnakeManager,
               EventActionType.ResetSnake));

            Application.SceneManager.ActiveScene.RemoveAll(ObjectType.Static, RenderType.Opaque, (consumable) => consumable.GameObjectType == GameObjectType.Consumable);

            EventDispatcher.Raise(new EventData(EventCategoryType.RenderUIGameObjects,
               EventActionType.UITextIsNotDrawn));

            EventDispatcher.Raise(new EventData(EventCategoryType.Game,
               EventActionType.ResetIntroCamera));
        }

        private void StartOfLevel()
        {
            ResetLevel();
            int defaultFoodNumber = 0;
            int defaultBombNumber = 0;


            switch (this.currentLevel)
                {
                case (int)Level.LevelOne:
                    defaultFoodNumber = defaultFoodEachLevel[0];
                    break;

                case (int)Level.LevelTwo:
                    defaultFoodNumber = defaultFoodEachLevel[1];
                    defaultBombNumber = defaultBombsEachLevel[1];
                    break;

                case (int)Level.LevelThree:
                    defaultFoodNumber = defaultFoodEachLevel[2];
                    defaultBombNumber = defaultBombsEachLevel[2];
                    break;

                default:
                   break;
                }

            object[] parametersFood = { defaultFoodNumber };
            EventDispatcher.Raise(new EventData(EventCategoryType.FoodManager,
            EventActionType.InitilizeFoodStartOfLevel, parametersFood));

            if (currentLevel > AppData.LEVEL_ONE)
            {
                object[] parametersBombs = { defaultBombNumber };
                EventDispatcher.Raise(new EventData(EventCategoryType.BombManager,
                EventActionType.InitilizeBombsStartOfLevel, parametersBombs));
            }

            EventDispatcher.Raise(new EventData(EventCategoryType.Menu,
               EventActionType.OnPlay));
        }

        private void StartNewLevel(GameObject removeGameObject)
        {
            Application.UISceneManager.ActiveScene.Remove((uiElement) => uiElement.Name == removeGameObject.Name);           
            
            totalElapsedTimeMS = 0;
            currentScore = 0;
            UpdateScore();

            EventDispatcher.Raise(new EventData(EventCategoryType.RenderUIGameObjects,
            EventActionType.UITextIsDrawn));

            gameStarted = true;          
        }

        private void CheckCurrentLevel()
        {
            if(currentLevel > totalLevels)
            {
                EventDispatcher.Raise(new EventData(EventCategoryType.Menu,
                EventActionType.OnPause));

                EventDispatcher.Raise(new EventData(EventCategoryType.Game,
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
       
        //private void UpdateCameraRotataion(GameTime gameTime)
        //{
        //    totalElapsedTimeCameraRotationMS += gameTime.ElapsedGameTime.Milliseconds;


        //    if (totalElapsedTimeCameraRotationMS >= 5000f)
        //    {

        //            float rotationAmt = (float)(rotateSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);

        //            if (rotation < targetRotation)
        //            {

        //            if (Application.CameraManager.ActiveCameraName == AppData.FRONT_CAMERA_NAME)
        //            {
        //                Application.CameraManager.ActiveCamera.gameObject.Transform.Rotate(0, 0, rotationAmt);
        //            }
        //            else if (Application.CameraManager.ActiveCameraName == AppData.BACK_CAMERA_NAME)
        //            {
        //                Application.CameraManager.ActiveCamera.gameObject.Transform.Rotate(0, 0, -rotationAmt);
        //            }

        //            else if (Application.CameraManager.ActiveCameraName == AppData.TOP_CAMERA_NAME)
        //            {
        //                Application.CameraManager.ActiveCamera.gameObject.Transform.Rotate(0, rotationAmt, 0);

        //            }
        //            else if (Application.CameraManager.ActiveCameraName == AppData.BOTTOM_CAMERA_NAME)
        //            {
        //                Application.CameraManager.ActiveCamera.gameObject.Transform.Rotate(0, -rotationAmt, 0);

        //            }

        //            rotation += rotationAmt;
        //        }
        //        else
        //        {
        //            rotation = 0f;
        //            totalElapsedTimeCameraRotationMS = 0;
        //        }

        //    }
            
        //}
    }
}