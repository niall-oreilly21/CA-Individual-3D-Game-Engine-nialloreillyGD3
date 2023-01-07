using GD.Engine.Events;
using GD.Engine.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public class LevelTextUpdate : PausableGameComponent
    {
        private TextMaterial2D oldMaterial2D;
        private TextMaterial2D newMaterial2D;
        private Renderer2D newRenderer2D;
        string text;
        GameObject gameObject;

        public LevelTextUpdate(Game game, GameObject gameObject) : base(game)
        {
            this.gameObject = gameObject;
        }

        protected override void SubscribeToEvents()
        {
            EventDispatcher.Subscribe(EventCategoryType.UpdateUIElements, HandleGameObjectEvents);
        }

        protected void HandleGameObjectEvents(EventData eventData)
        {
            switch (eventData.EventActionType)
            {
                case EventActionType.UpdateUI: //TODO
                    this.text = (string)eventData.Parameters[0];
                    UpdateUI();
                    break;

                default:
                    break;
                    //add more cases for each method that we want to support with events
            }
        }

        private void UpdateUI()
        {
            oldMaterial2D = (TextMaterial2D)gameObject.GetComponent<Renderer2D>().Material;
            newMaterial2D = new TextMaterial2D(oldMaterial2D.SpriteFont, text, oldMaterial2D.TextOffset, oldMaterial2D.Color);

            newRenderer2D = new Renderer2D(newMaterial2D);

            gameObject.RemoveComponent<Renderer2D>();
            gameObject.AddComponent(newRenderer2D);
        }
    }
}
