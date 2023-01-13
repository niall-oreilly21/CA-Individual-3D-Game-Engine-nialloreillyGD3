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
                if (transform.Translation.Z > AppData.SNAKE_GAME_MAX_BOUNDARY)
                {
                    transform.SetTranslation(transform.Translation.X, transform.Translation.Y, 0);
                }
                else
                {
                    translation.Z++;
                    object[] parameters = { translation };
                    EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
                    EventActionType.OnMove, parameters));
                }
            }

            else if (Input.Keys.IsPressed(Keys.S))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.S;
                pressed = true;

                if (transform.Translation.Z < 0)
                {
                    transform.SetTranslation(transform.Translation.X, transform.Translation.Y, AppData.SNAKE_GAME_MAX_BOUNDARY);
                }
                else
                {
                    translation.Z--;
                    object[] parameters = { translation };
                    EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
                    EventActionType.OnMove, parameters));
                }
            }

            else if (Input.Keys.IsPressed(Keys.A))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.A;
                pressed = true;
                if (transform.Translation.X < 0)
                {
                    transform.SetTranslation(AppData.SNAKE_GAME_MAX_BOUNDARY, transform.Translation.Y, transform.Translation.Z);
                }
                else
                {
                    translation.X--;
                    object[] parameters = { translation };
                    EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
                    EventActionType.OnMove, parameters));
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

                if (transform.Translation.X > AppData.SNAKE_GAME_MAX_BOUNDARY)
                {
                    transform.SetTranslation(0, transform.Translation.Y, transform.Translation.Z);
                }
                else
                {
                    translation.X++;
                    object[] parameters = { translation };
                    EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
                    EventActionType.OnMove, parameters));                 
                }

            }

            else if (Input.Keys.IsPressed(Keys.Left))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.Left;
                pressed = true;
                if (transform.Translation.Y < 0)
                {
                    transform.SetTranslation(transform.Translation.X, AppData.SNAKE_GAME_MAX_BOUNDARY, transform.Translation.Z);
                }
                else
                {
                    translation.Y--;
                    object[] parameters = { translation };
                    EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
                    EventActionType.OnMove, parameters));
                }
            }

            else if (Input.Keys.IsPressed(Keys.Right))
            {
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.Right;
                pressed = true;
                if (transform.Translation.Y > AppData.SNAKE_GAME_MAX_BOUNDARY)
                {
                    transform.SetTranslation(transform.Translation.X, 0, transform.Translation.Z);
                }
                else
                {
                    translation.Y++;
                    object[] parameters = { translation };
                    EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
                    EventActionType.OnMove, parameters));
                }
            }
        }
        #endregion Actions - Update, Input
    }
}

