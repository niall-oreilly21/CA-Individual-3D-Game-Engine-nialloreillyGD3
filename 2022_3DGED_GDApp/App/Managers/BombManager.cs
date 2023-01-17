using GD.App;
using GD.Engine.Events;
using GD.Engine;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GD.Engine.Globals;

namespace GD.Engine
{
    /// <summary>
    /// Maanges events for the poison consumable
    /// </summary>
    public class BombManager : ConsumableManager
    {
        public BombManager(Game game, GameObject consumable) : base(game, consumable)
        {

        }

        protected override void SubscribeToEvents()
        {
            EventDispatcher.Subscribe(EventCategoryType.BombManager, HandleEvent);
        }

        protected override void HandleEvent(EventData eventData)
        {
            switch (eventData.EventActionType)
            {
                case EventActionType.AddBomb:
                    InitializeConsumableItem();
                    break;

                case EventActionType.RemoveBomb:
                    GameObject removeFoodItem = (GameObject)eventData.Parameters[0];
                    RemoveBombItem(removeFoodItem);
                    break;

                case EventActionType.InitilizeBombsStartOfLevel:
                    int bombStartNumber = (int)eventData.Parameters[0];
                    InitializeConsumableItemsStart(bombStartNumber);
                    break;

                default:
                    break;
            }

        }

        private void RemoveBombItem(GameObject consumableToRemove)
        {
            if (base.RemoveConsumable(consumableToRemove))
            {
                var audioListener = Application.Player.GetComponent<AudioListenerBehaviour>().AudioListener;
                var audioEmitter = Application.Player.GetComponent<AudioEmitterBehaviour>().AudioEmitter;

                object[] parameters = { AppData.EAT_POISON_SOUND_NAME, audioListener, audioEmitter };

                EventDispatcher.Raise(new EventData(EventCategoryType.Sound,
                    EventActionType.OnPlay3D, parameters));

                EventDispatcher.Raise(new EventData(EventCategoryType.SceneManager,
                EventActionType.OnLose, new object[] { AppData.END_MENU_SCENE_NAME }));

                EventDispatcher.Raise(new EventData(EventCategoryType.StateManager,
                EventActionType.UpdateEndMenuScreenUIText, new object[] { AppData.SNAKE_MENU_UI_TEXT_HIT_BOMB }));
            }

        }
        protected override GameObject CloneModelGameObject(string newName, Vector3 newTranslation)
        {

            GameObject gameObjectClone = base.CloneModelGameObject(newName, newTranslation);

            Collider cloneCollider = new BombCollider(gameObjectClone, true);

            cloneCollider.AddPrimitive(
                new Box
                (
                    gameObjectClone.Transform.Translation,
                    gameObjectClone.Transform.Rotation,
                    gameObjectClone.Transform.Scale

                    ),
                new MaterialProperties(0.8f, 0.8f, 0.7f)
                );

            cloneCollider.Enable(gameObjectClone, false, 1);
            gameObjectClone.AddComponent(cloneCollider);

            gameObjectClone.AddComponent(new BombController(AppData.BOMB_ROTATE_SPEED));

            return gameObjectClone;

        }
    }
}
