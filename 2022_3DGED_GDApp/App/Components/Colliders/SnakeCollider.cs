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
            //if (Application.SnakeManager.SnakePartsListBodies.Count > 3)
            //{

            //if (parentGameObject.GetComponent<Collider>().Body as Character == Application.SnakeManager.SnakePartsListBodies[1]) return;
            //if (parentGameObject.GetComponent<Collider>().Body as Character == Application.SnakeManager.SnakePartsListBodies[2]) return;


            //GameObject colliderOne = Application.SnakeManager.SnakePartsListBodies[1].Parent as GameObject;
            //GameObject colliderTwo = Application.SnakeManager.SnakePartsListBodies[2].Parent as GameObject;
            //GameObject colliderThree = Application.SnakeManager.SnakePartsListBodies[3].Parent as GameObject;


            if (isColliding)
                {


                    //System.Diagnostics.Debug.WriteLine(parentGameObject.Name);
                    if (parentGameObject == Application.Player)
                    {
                        
                        EventDispatcher.Raise(new EventData(EventCategoryType.SnakeManager,
                        EventActionType.RemoveSnake, new object[] { parentGameObject }));

                    }

                }


            }
        
    }
}
