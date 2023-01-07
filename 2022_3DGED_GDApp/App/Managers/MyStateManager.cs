using GD.Engine.Events;
using GD.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GD.Engine.Globals;

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
        private int currentLevel;
        private int currentScore;

        public MyStateManager(Game game, float maxTimeInMS) : base(game)
        {
            this.maxTimeInMS = maxTimeInMS;
            this.totalElapsedTimeMS = 0;
            this.currentLevel = 0;
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
    }
}