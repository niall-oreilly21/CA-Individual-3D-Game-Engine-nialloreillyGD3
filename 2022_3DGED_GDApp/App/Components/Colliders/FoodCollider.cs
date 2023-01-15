using GD.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public class FoodCollider : Collider
    {
        public FoodCollider(GameObject gameObject, bool isHandlingCollision = false, bool isTrigger = false) : base(gameObject, isHandlingCollision, isTrigger)
        {

        }

        protected override void HandleResponse(GameObject parentGameObject)
        {
            object[] parameters = {this.gameObject};

            EventDispatcher.Raise(new EventData(EventCategoryType.FoodManager, EventActionType.RemoveFood, parameters));   
        }
    }
}
