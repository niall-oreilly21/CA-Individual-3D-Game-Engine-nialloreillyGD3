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
    public class BombManager : ConsumableManager
    {
        public BombManager(Game game, GameObject consumable) : base(game, consumable)
        {
            for (int i = 0; i < 10; i++)
            {
                InitializeConsumableItem();
            }
        }

        protected override void SubscribeToEvents()
        {
            EventDispatcher.Subscribe(EventCategoryType.Bomb, HandleEvent);
        }

        protected override void HandleEvent(EventData eventData)
        {
            switch (eventData.EventActionType)
            {
                case EventActionType.AddBomb:
                    InitializeConsumableItem();
                    break;

                case EventActionType.InitilizeBombsStartOfLevel:
                    int bombStartNumber = (int)eventData.Parameters[0];
                    InitializeConsumableItemsStart(bombStartNumber);
                    break;

                default:
                    break;
            }

        }

        protected override GameObject CloneModelGameObject(string newName)
        {

            GameObject gameObjectClone = base.CloneModelGameObject(newName);

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
