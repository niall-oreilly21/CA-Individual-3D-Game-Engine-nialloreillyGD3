using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace GD.Engine
{
    public class CameraKeys
    {
        #region Fields
        Keys left;
        Keys right;
        Keys backward;
        Keys forward;
        #endregion Fields

        #region Properties
        public Keys Left
        {
            get
            {
                return left;
            }
        }

        public Keys Right
        {
            get
            {
                return right;
            }
        }

        public Keys Backward
        {
            get
            {
                return backward;
            }
        }

        public Keys Forward
        {
            get
            {
                return forward;
            }
        }
        #endregion Properties

        public CameraKeys(Keys left, Keys right, Keys backward, Keys forward)
        {
            this.left = left;
            this.right = right;
            this.forward = forward;
            this.backward = backward;
        }
    }
}
