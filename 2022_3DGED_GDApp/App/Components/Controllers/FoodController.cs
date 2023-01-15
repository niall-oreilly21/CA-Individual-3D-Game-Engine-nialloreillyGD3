using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public class FoodController : ConsumableController
    {
        public FoodController(float rotationSpeed) : base(rotationSpeed)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            transform.Rotate(RotationAmount, RotationAmount, RotationAmount);
        }
    }
}
