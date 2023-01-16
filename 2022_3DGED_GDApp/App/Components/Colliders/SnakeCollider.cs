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
        public SnakeCollider(GameObject gameObject, bool isHandlingCollision = false) : base(gameObject, isHandlingCollision)
        {
            
        }

        protected override void HandleResponse(GameObject parentGameObject)
        {
            return;
            //if (this.gameObject.GetComponent<SnakeCollider>().Body as Character == Application.SnakeManager.SnakePartsListBodies[1]) return;

            //if(Application.SnakeManager.SnakePartsListBodies.Count > 2)
            //{
            //    if (this.gameObject.GetComponent<SnakeCollider>().Body as Character == Application.SnakeManager.SnakePartsListBodies[2]) return;
            //}







        }
    }
}
