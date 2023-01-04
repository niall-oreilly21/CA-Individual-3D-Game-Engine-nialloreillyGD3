using GD.Engine.Globals;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GD.Engine
{
    public class ThirdPersonController : Component
    {
        private GameObject target;
        private List<GameObject> snakeParts;

        public ThirdPersonController()
        { }

        public override void Update(GameTime gameTime)
        {
            //if (Application.Player != null)
            //    target = Application.Player;
            //else
            //    throw new ArgumentNullException("Target not set! Do this in main");

            snakeParts = Application.SnakeParts;

            //target = snakeParts.Last();
            if (target != null)
            {
                System.Diagnostics.Debug.WriteLine(target.Transform.Translation.Y);
                //use target position + offset to generate new camera position
                var newPosition = target.Transform.Translation
                    + new Vector3(0, 1, 0);

                //set new camera position
                transform.SetTranslation(newPosition);
            }

            //since parent Update does nothing then dont bother calling it
            //base.Update(gameTime);
        }
    }
}