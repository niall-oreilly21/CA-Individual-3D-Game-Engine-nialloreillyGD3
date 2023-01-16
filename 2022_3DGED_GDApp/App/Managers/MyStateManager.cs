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
using System.Windows.Forms;

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
        private SnakeLevelsData snakeLevelsData;
        private bool startMove;

        public MyStateManager(Game game, SnakeLevelsData snakeLevelsData) : base(game)
        {
            this.maxTimeInMS = snakeLevelsData.TimesEachLevel[0];
            this.totalElapsedTimeMS = 0;
            this.totalSeconds = 0;
            this.minutes = 0;
            this.seconds = 0;
            this.currentScore = 0;
            this.currentLevel = AppData.LEVEL_ONE;
            this.snakeLevelsData = snakeLevelsData;
            Enabled = false;
            startMove = false;
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
                return currentLevel;
            }
            set
            {
                System.Diagnostics.Debug.WriteLine(value);
                currentLevel = value;
            }
        }

        public bool StartMove
        {
            get
            {
                return startMove;
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
                    UpdateScoreText();
                    break;

                case EventActionType.RemoveUILevelStart:
                    GameObject removeGameObject = (GameObject)eventData.Parameters[0];
                    StartNewLevel(removeGameObject);
                    break;

                case EventActionType.StartOfLevel:
                    int newLevel = (int)eventData.Parameters[0];
                    currentLevel = newLevel;
                    StartOfLevel();
                    break;

                case EventActionType.UpdateEndMenuScreenUIText:
                    string endScreenMessage = (string)eventData.Parameters[0];
                    UpdateEndScreenUI(endScreenMessage);
                    break;

                case EventActionType.RestartOfLevel:
                    StartOfLevel();
                    break;

                default:
                    break;
            }
        }

        private void UpdateEndScreenUI(string endScreenMessage)
        {
            GameObject uiEndScreenMessageGameObject = Application.MenuSceneManager.ActiveScene.Find((uiElement) => uiElement.Name == AppData.END_MENU_UI_TEXT_NAME);
            var material2D = (TextMaterial2D)uiEndScreenMessageGameObject.GetComponent<Renderer2D>().Material;
            material2D.StringBuilder.Clear();
            material2D.StringBuilder.Append(endScreenMessage);


            uiEndScreenMessageGameObject = Application.MenuSceneManager.ActiveScene.Find((uiElement) => uiElement.Name == AppData.END_MENU_UI_FINAL_SCORE_TEXT_NAME);

            material2D = (TextMaterial2D)uiEndScreenMessageGameObject.GetComponent<Renderer2D>().Material;
            material2D.StringBuilder.Clear();
            
            if(currentLevel == AppData.LEVEL_THREE)
            {
                material2D.StringBuilder.Append(AppData.END_MENU_UI_FINAL_SCORE_TEXT + currentScore);
            }
            

        }

        public override void Update(GameTime gameTime)
        {
                totalElapsedTimeMS += gameTime.ElapsedGameTime.Milliseconds;
                totalSeconds = maxTimeInMS - totalElapsedTimeMS / 1000d;
                minutes = Math.Floor(totalSeconds / 60);
                seconds = Math.Round(totalSeconds % 60);
                CheckTimer();
                CheckScore();
        }

        private void CheckTimer()
        {
            if(totalSeconds <= 0)
            {
                EventDispatcher.Raise(new EventData(EventCategoryType.SceneManager,
                EventActionType.OnLose, new object[] { AppData.END_MENU_SCENE_NAME}));

                EventDispatcher.Raise(new EventData(EventCategoryType.StateManager,
                EventActionType.UpdateEndMenuScreenUIText, new object[] { AppData.SNAKE_MENU_UI_TEXT_OUT_OF_TIME}));
            }
        }

        private void CheckScore()
        {
            if(CurrentLevel < AppData.LEVEL_THREE)
            {
                if (currentScore >= snakeLevelsData.MaxScore[CurrentLevel - 1])
                {
                    EventDispatcher.Raise(new EventData(EventCategoryType.SceneManager,
                    EventActionType.OnWin, new object[] { AppData.WIN_LEVEL_MENU_SCENE_NAME }));
                }
            }
           
        }


        private void UpdateScoreText()
        {
            if(startMove)
            {
                System.Diagnostics.Debug.WriteLine("HERE");
                currentScore++;
            }
            
            GameObject scoreGameObject = Application.UISceneManager.ActiveScene.Find((uiElement) => uiElement.Name == AppData.SCORE_UI_TEXT_NAME);

            var material2D = (TextMaterial2D)scoreGameObject.GetComponent<Renderer2D>().Material;
            material2D.StringBuilder.Clear();
            material2D.StringBuilder.Append(AppData.DEFAULT_SCORE_TEXT + currentScore);
        }

        private void UpdateLevelText()
        {
            GameObject levelGameObject = Application.UISceneManager.ActiveScene.Find((uiElement) => uiElement.Name == AppData.LEVEL_UI_TEXT_NAME);

            var material2D = (TextMaterial2D)levelGameObject.GetComponent<Renderer2D>().Material;
            material2D.StringBuilder.Clear();
            material2D.StringBuilder.Append(AppData.DEFAULT_LEVEL_TEXT + currentLevel);
        }

        private void ResetLevel()
        {
            EventDispatcher.Raise(new EventData(EventCategoryType.SnakeManager,
            EventActionType.ResetSnake));

            Application.SceneManager.ActiveScene.RemoveAll(ObjectType.Static, RenderType.Opaque, (consumable) => consumable.GameObjectType == GameObjectType.Consumable);

            System.Diagnostics.Debug.WriteLine(CurrentLevel);
            EventDispatcher.Raise(new EventData(EventCategoryType.RenderUIGameObjects,
               EventActionType.UITextIsNotDrawn));


            EventDispatcher.Raise(new EventData(EventCategoryType.Game,
               EventActionType.ResetIntroCamera));

            currentScore = 0;
            UpdateScoreText();

            //Application.CameraManager.SetActiveCamera(AppData.FRONT_CAMERA_NAME);
        }

        private void StartOfLevel()
        {
            startMove = false;
            ResetLevel();


            int defaultFoodNumber = 0;
            int defaultBombNumber = 0;

            if (currentLevel > 0)
            {
                defaultFoodNumber = snakeLevelsData.DefaultFoodEachLevel[currentLevel - 1];
                defaultBombNumber = snakeLevelsData.DefaultBombsEachLevel[currentLevel - 1];
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

            EventDispatcher.Raise(new EventData(EventCategoryType.Sound, EventActionType.OnPause, new object[] { AppData.SNAKE_MENU_BACKGROUND_SOUND_NAME }));

            EventDispatcher.Raise(new EventData(EventCategoryType.Sound, EventActionType.OnPlay2D, new object[] { AppData.IN_GAME_BACKGROUND_SOUND_NAME }));
        }

        private void StartNewLevel(GameObject removeGameObject)
        {
            Application.UISceneManager.ActiveScene.Remove((uiElement) => uiElement.Name == removeGameObject.Name);

            totalElapsedTimeMS = 0;

            UpdateLevelText();

            EventDispatcher.Raise(new EventData(EventCategoryType.Game, EventActionType.ResetCameraUI, new object[] { AppData.FRONT_CAMERA_UI_TEXT }));

            if (currentLevel > 0)
            {
                maxTimeInMS = snakeLevelsData.TimesEachLevel[currentLevel - 1];
            }


            EventDispatcher.Raise(new EventData(EventCategoryType.RenderUIGameObjects,
            EventActionType.UITextIsDrawn));

            startMove = true;
        }
    
    }
}