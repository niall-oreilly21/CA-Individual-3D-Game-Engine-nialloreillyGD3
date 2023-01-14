using GD.Engine;
using GD.Engine.Events;
using GD.Engine.Globals;
using GD.Engine.Parameters;
using Microsoft.Xna.Framework;
using System;

namespace GD.App
{
    /// <summary>
    /// Applies an action (Action) to a target game object
    /// </summary>
    /// <see cref="Main.InitializeCameras()"/>
    public class CurveBehaviour : Component
    {
        #region Fields

        private Curve3D curve;
        private Action<Curve3D, GameObject, GameTime> action;
        private bool reachedLastPositionOnCurve;
        private bool isStart;
        private float currentTime;
        

        #endregion Fields

        #region Constructors

        public CurveBehaviour(Curve3D curve, Action<Curve3D, GameObject, GameTime> action)
        {
            this.curve = curve;
            this.action = action;
            this.isStart = true;
        }

        #endregion Constructors


        public override void Update(GameTime gameTime)
        {
            if(!reachedLastPositionOnCurve)
            {
                action(curve, gameObject, gameTime);
            }

            if(currentTime <= AppData.CURVE_TIME_SPAN)
            {
                currentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                isStart = false;
            }
  
            if(!isStart)
            {
                if (Vector3.Distance(transform.Translation, AppData.DEFAULT_FRONT_CAMERA_TRANSLATION) < 0.1f && !reachedLastPositionOnCurve)
                {
                    reachedLastPositionOnCurve = true;

                    // Switch main camera
                    Application.CameraManager.SetActiveCamera(AppData.FRONT_CAMERA_NAME);

                    // Start player movement
                    //EventDispatcher.Raise(new EventData(EventCategoryType.Player, EventActionType.OnStartMovement));
                }
            }
           
        }
    }
}