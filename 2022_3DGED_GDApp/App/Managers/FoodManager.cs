using GD.App;
using GD.Engine.Events;
using GD.Engine.Globals;
using GD.Engine.Managers;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    /// <summary>
    /// Maanges events for the apple consumable
    /// </summary>
    public class FoodManager : ConsumableManager
    {
        public FoodManager(Game game, GameObject consumable) : base(game, consumable)
        {
        }

        protected override void SubscribeToEvents()
        {
            EventDispatcher.Subscribe(EventCategoryType.FoodManager, HandleEvent);
        }

        protected override void HandleEvent(EventData eventData)
        {
            switch (eventData.EventActionType)
            {
                case EventActionType.RemoveFood:
                    GameObject removeFoodItem = (GameObject)eventData.Parameters[0];
                    RemoveFoodItem(removeFoodItem);
                    break;

                case EventActionType.AddFood:
                    InitializeConsumableItem();
                    break;

                case EventActionType.InitilizeFoodStartOfLevel:
                    int foodStartNumber = (int)eventData.Parameters[0];
                    InitializeConsumableItemsStart(foodStartNumber);
                    break;

                default:
                    break;
            }

        }

        private void RemoveFoodItem(GameObject consumableToRemove)
        {
            if(base.RemoveConsumable(consumableToRemove))
            {
                EventDispatcher.Raise(new EventData(EventCategoryType.SnakeManager, EventActionType.Grow));

                var audioListener = Application.Player.GetComponent<AudioListenerBehaviour>().AudioListener;
                var audioEmitter = Application.Player.GetComponent<AudioEmitterBehaviour>().AudioEmitter;

                object[] parameters = { AppData.EAT_APPLE_SOUND_NAME, audioListener, audioEmitter };

                EventDispatcher.Raise(new EventData(EventCategoryType.Sound,
                    EventActionType.OnPlay3D, parameters));

                if (Application.StateManager.CurrentLevel == AppData.LEVEL_THREE)
                {
                    InitializeConsumableItem();

                    EventDispatcher.Raise(new EventData(EventCategoryType.BombManager,
                    EventActionType.AddBomb));


                }
                InitializeConsumableItem();
            }

        }
 

        protected override GameObject CloneModelGameObject(string newName, Vector3 newTranslation)
        {

            GameObject gameObjectClone = base.CloneModelGameObject(newName, newTranslation);

            Collider cloneCollider = new FoodCollider(gameObjectClone, true);

            cloneCollider.AddPrimitive(
                new Sphere(
                     gameObjectClone.Transform.Translation,
                    AppData.SCALE_AMOUNT / 2f
                    ),
                new MaterialProperties(0.8f, 0.8f, 0.7f)
                );

            cloneCollider.Enable(gameObjectClone, false, 1);
            gameObjectClone.AddComponent(cloneCollider);

            gameObjectClone.AddComponent(new FoodController(AppData.FOOD_ROTATE_SPEED));

            return gameObjectClone;

        }
    }
}
