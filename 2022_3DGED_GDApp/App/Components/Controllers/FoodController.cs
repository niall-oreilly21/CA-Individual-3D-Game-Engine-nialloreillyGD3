using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public class FoodController : Component
    {
        private float rotationSpeed;
        private float rotationAmount;

        public FoodController()
        {
            this.rotationSpeed = 0.2f;
        }

        public override void Update(GameTime gameTime)
        {
            rotationAmount = (float)(rotationSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);

            transform.Rotate(rotationAmount, rotationAmount, rotationAmount);
        }
    }
}
