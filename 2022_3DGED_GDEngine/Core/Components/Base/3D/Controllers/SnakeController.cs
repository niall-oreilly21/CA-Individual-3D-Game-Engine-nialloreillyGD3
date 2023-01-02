using GD.App;
using GD.Engine.Events;
using GD.Engine.Globals;
using GD.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GD.Engine
{
    /// <summary>
    /// Adds simple movement controller for Snake Head using keyboard
    /// </summary>
    public class SnakeController : Component
    {
        #region Fields
        private bool pressed = false;
        private Keys pressedKey;
        private long keyPressedTime = 0;
        private SceneManager<Scene> sceneManager;

        #endregion Fields

        #region Temps

        protected Vector3 translation = Vector3.Zero;
        protected Vector3 rotation = Vector3.Zero;
 

        #endregion Temps

        #region Constructors
        public SnakeController()
        {
        }
        #endregion Constructors

        #region Actions - Update, Input

        public override void Update(GameTime gameTime)
        {
            HandleKeyboardInput(gameTime);
        }

        public virtual void HandleKeyboardInput(GameTime gameTime)
        {
            if (Input.Keys.WasJustPressed(pressedKey))
            {
                pressed = false;
            }


            translation = Vector3.Zero;
            if (Input.Keys.IsPressed(Keys.W))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.W;
                pressed = true;
                translation.Z = 1;
            }

            else if (Input.Keys.IsPressed(Keys.S))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.S;
                pressed = true;

               // move(new Vector3(this.gameObject.Translation.X, head.Transform.Translation.Y, head.Transform.Translation.Z - 1));
            }

            else if (Input.Keys.IsPressed(Keys.A))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.A;
                pressed = true;
                if (this.gameObject.Transform.Translation.X > 0)
                {
                    //move(new Vector3(head.Transform.Translation.X - 1, head.Transform.Translation.Y, head.Transform.Translation.Z));
                }
                else
                {
                    translation.X = 9;
                }
            }
            else if (Input.Keys.IsPressed(Keys.D))
            {

                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.D;
                pressed = true;
                if (true)
                {
                    object[] parameters = { new Vector3(0,0,-1)};

                    System.Diagnostics.Debug.WriteLine(parameters);
                    EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
                    EventActionType.OnMove, parameters));
                }
                else
                {
                    translation.X = -9;
                }

            }

            else if (Input.Keys.IsPressed(Keys.Q))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.Q;
                pressed = true;
                translation.Y = 1;
            }

            else if (Input.Keys.IsPressed(Keys.R))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.R;
                pressed = true;
                translation.Y = -1;
            }

            //transform.Translate(translation);
        }
        #endregion Actions - Update, Input
    }
}

