using GD.Engine.Events;
using GD.Engine.Globals;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GD.Engine.Managers
{
    /// <summary>
    /// Stores all scenes and updates the active scene
    /// </summary>
    public class SceneManager<T> : PausableGameComponent where T : IUpdateable
    {
        #region Fields

        private T activeScene;
        private Dictionary<string, T> scenes;

        #endregion Fields

        #region Properties

        public T ActiveScene
        {
            get
            {
                if (activeScene == null)
                    throw new NullReferenceException("Active scene not set! Call SetActiveScene()");

                return activeScene;
            }
        }

        #endregion Properties

        #region Constructors

        public SceneManager(Game game)
            : base(game)
        {
            scenes = new Dictionary<string, T>();
        }

        #endregion Constructors

        protected override void SubscribeToEvents()
        {
            //handle add/remove events
            EventDispatcher.Subscribe(EventCategoryType.SceneManager, HandleMenuChangeEvents);

            base.SubscribeToEvents();
        }

        protected void HandleMenuChangeEvents(EventData eventData)
        {
            string menuName = "";
            switch (eventData.EventActionType)
            {
                case EventActionType.OnMainMenuScene:
                    menuName = (string)eventData.Parameters[0];
                    SetActiveScene(menuName);
                    break;

                case EventActionType.OnLevelsScene:
                    menuName = (string)eventData.Parameters[0];
                    SetActiveScene(menuName);
                    break;

                case EventActionType.OnAudioScene:
                    menuName = (string)eventData.Parameters[0];
                    SetActiveScene(menuName);
                    break;

                case EventActionType.OnControlsScene:
                    menuName = (string)eventData.Parameters[0];
                    SetActiveScene(menuName);
                    break;

                case EventActionType.OnLose:
                    menuName = (string)eventData.Parameters[0];
                    SetActiveScene(menuName);

                    EventDispatcher.Raise(new EventData(EventCategoryType.Menu,
                    EventActionType.OnPause));

                    Application.StateManager.Enabled = false;
                    break;

                case EventActionType.OnWin:
                    menuName = (string)eventData.Parameters[0];

                    EventDispatcher.Raise(new EventData(EventCategoryType.Menu,
                   EventActionType.OnPause));
                    Application.StateManager.Enabled = false;

                    SetActiveScene(menuName);
                    break;

                case EventActionType.OnGameExit:
                    Game.Exit();
                    break;

                default:
                    break;
            }

            base.HandleEvent(eventData);
        }

        #region Actions - Add, SetActiveScene

        public bool Add(string id, T scene)
        {
            id = id.Trim().ToLower();

            if (scenes.ContainsKey(id))
                return false;

            scenes.Add(id, scene);
            return true;
        }

        public T SetActiveScene(string id)
        {
            id = id.Trim().ToLower();

            if (scenes.ContainsKey(id))
                activeScene = scenes[id];

            return activeScene;
        }

        #endregion Actions - Add, SetActiveScene

        #region Actions - Update

        public override void Update(GameTime gameTime)
        {
            if (IsUpdated)
                activeScene?.Update(gameTime);
        }

        #endregion Actions - Update
    }
}