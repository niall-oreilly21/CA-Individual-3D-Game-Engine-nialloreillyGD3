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
        private bool isColliding = true;
        public SnakeCollider(GameObject gameObject, bool isHandlingCollision = true) : base(gameObject, isHandlingCollision)
        {
            if (gameObject.Name == "snake part 1" || gameObject.Name == "snake part 2" || gameObject.Name == "tail")
            {
                isColliding = false;
            }
        }

        protected override void HandleResponse(GameObject parentGameObject)
        {

            if (isColliding)
                {
                    if (parentGameObject == Application.Player)
                    {
                        
                        EventDispatcher.Raise(new EventData(EventCategoryType.SnakeManager,
                        EventActionType.RemoveSnake));

                    }

                }


            }
        
    }
}
