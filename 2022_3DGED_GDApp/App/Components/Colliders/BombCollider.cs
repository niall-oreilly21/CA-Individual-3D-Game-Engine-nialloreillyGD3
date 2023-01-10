using GD.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public class BombCollider : Collider
    {
        public BombCollider(GameObject gameObject, bool isHandlingCollision = false, bool isTrigger = false) : base(gameObject, isHandlingCollision, isTrigger)
        {
        }

        protected override void HandleResponse(GameObject parentGameObject)
        {
            EventDispatcher.Raise(new EventData(EventCategoryType.Menu,
                  EventActionType.OnPause));

            EventDispatcher.Raise(new EventData(EventCategoryType.Player,
            EventActionType.OnLose));
        }
      
    }
}
