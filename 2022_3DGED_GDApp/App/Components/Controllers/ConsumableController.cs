using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public abstract class ConsumableController : Component
    {
        private float rotationSpeed;
        private float rotationAmount;

        public ConsumableController(float rotationSpeed)
        {
            this.rotationSpeed = rotationSpeed;
        }

        #region Properties
        protected float RotationAmount
        { 
            get 
            {
                return rotationAmount;
            }  
        }
        #endregion Properties

        public override void Update(GameTime gameTime)
        {
            rotationAmount = (float)(rotationSpeed * gameTime.ElapsedGameTime.TotalMilliseconds);
        }
    }
}
