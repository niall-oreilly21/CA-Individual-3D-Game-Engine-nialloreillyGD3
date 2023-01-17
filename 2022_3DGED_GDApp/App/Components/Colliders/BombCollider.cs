using GD.App;
using GD.Engine.Events;
using GD.Engine.Globals;
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
            if (parentGameObject == Application.Player)
            {
                object[] parameters = { this.gameObject };

                EventDispatcher.Raise(new EventData(EventCategoryType.BombManager, EventActionType.RemoveBomb, parameters));
            }
             
        }
      
    }
}
