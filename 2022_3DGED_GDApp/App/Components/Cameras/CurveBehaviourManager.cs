using GD.Engine.Globals;
using GD.Engine.Parameters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    /// <summary>
    /// Applies an action (Action) to a target game object
    /// </summary>
    /// <see cref="Main.InitializeCameras()"/>
    public class CurveBehaviourManager : Component
    {
            #region Fields

            private Curve3D currentCurve;
            private Curve3D[] curves;
            private Action<Curve3D, GameObject, GameTime> action;

            #endregion Fields

            #region Constructors

            public CurveBehaviourManager(Curve3D[] curves, Action<Curve3D, GameObject, GameTime> action)
            {
                this.curves = curves;
                this.action = action;
            }

            #endregion Constructors

            public override void Update(GameTime gameTime)
            {
                if (Input.Keys.IsPressed(Keys.Down))
                {
                    currentCurve = curves[0];
                    action(currentCurve, gameObject, gameTime);
                }

                if (Input.Keys.IsPressed(Keys.Up))
                {
                    currentCurve = curves[1];
                    action(currentCurve, gameObject, gameTime);
                }

                //transform.SetTranslation(curve.Evaluate(time, 4));
                // transform.SetRotation(rotationCurve.Evaluate(time, 4));
            }
    }
}

