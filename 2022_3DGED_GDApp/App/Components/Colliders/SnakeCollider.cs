using GD.Engine.Events;
using GD.Engine.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GD.Engine
{
    public class SnakeCollider : CharacterCollider
    {
        public SnakeCollider(GameObject gameObject, bool isHandlingCollision = true) : base(gameObject, isHandlingCollision)
        {
            
        }

        protected override void HandleResponse(GameObject parentGameObject)
        {
            IsHandlingCollision = false;
            if (this.gameObject.GetComponent<SnakeCollider>().Body as Character == Application.SnakeManager.SnakePartsListBodies[1]) return;

            if(Application.SnakeManager.SnakePartsListBodies.Count > 2)
            {
                if (this.gameObject.GetComponent<SnakeCollider>().Body as Character == Application.SnakeManager.SnakePartsListBodies[2]) return;
            }


            if(parentGameObject == Application.Player)
            {
                EventDispatcher.Raise(new EventData(EventCategoryType.SnakeManager,
 EventActionType.RemoveSnake, new object[] { parentGameObject }));
            }

            

        }
    }
}
