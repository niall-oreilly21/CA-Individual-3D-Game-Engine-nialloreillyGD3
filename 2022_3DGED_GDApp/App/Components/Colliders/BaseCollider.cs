using GD.Engine.Events;
using GD.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public class BaseCollider : Collider
    {
        public BaseCollider(GameObject gameObject, bool isHandlingCollision = false, bool isTrigger = false) : base(gameObject, isHandlingCollision, isTrigger)
        {

        }

        protected override void HandleResponse(GameObject parentGameObject)
        {
            // send event to game state to check time remaining. If time is still left,
            // change state to win state
            EventDispatcher.Raise(new EventData(EventCategoryType.Player, EventActionType.RemoveFood));

        }
    }
}

