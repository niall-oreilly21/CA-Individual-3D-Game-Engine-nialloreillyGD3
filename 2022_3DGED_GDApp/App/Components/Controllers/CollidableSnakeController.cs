using GD.App;
using GD.Engine;
using GD.Engine.Events;
using GD.Engine.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Engine
{
    class CollidableSnakeController : SnakeController
    {
        private bool pressed = false;
        private Keys pressedKey;
        private Keys previousKey;
        protected float moveSpeed;
        private Vector3 direction = new Vector3(AppData.SCALE_AMOUNT, 0, 0);


        public CollidableSnakeController()
        {
            this.moveSpeed = Application.SnakeMoveSpeed;
        }


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
                if (!pressed)
                {
                    pressedKey = Keys.W;
                    pressed = true;
                    if (previousKey != Keys.S)
                    {
                        previousKey = Keys.W;
                        direction = transform.World.Forward;
                    }

                }
            }

            else if (Input.Keys.IsPressed(Keys.S))
            {
                if (!pressed)
                {
                    pressedKey = Keys.S;
                    pressed = true;
                    if (previousKey != Keys.W)
                    {
                        previousKey = Keys.S;
                        direction = transform.World.Backward;
                    }

                }
            }
            else if (Input.Keys.IsPressed(Keys.A))
            {
                if (!pressed)
                {
                    pressedKey = Keys.A;
                    pressed = true;
                    if (previousKey != Keys.D)
                    {
                        previousKey = Keys.A;
                        direction = transform.World.Left;
                    }
                    
                }
                
            }
            else if (Input.Keys.IsPressed(Keys.D))
            {
                if (!pressed)
                {
                    pressedKey = Keys.D;
                    pressed = true;
                    if (previousKey != Keys.A)
                    {
                        previousKey = Keys.D;
                        direction = transform.World.Right;
                    }                  
                }

            }
            else if (Input.Keys.IsPressed(Keys.Q))
            {
                if (!pressed)
                {
                    pressedKey = Keys.Q;
                    pressed = true;
                    if (previousKey != Keys.R)
                    {
                        previousKey = Keys.R;
                        direction = transform.World.Down;
                    }
                }
            }

            else if (Input.Keys.IsPressed(Keys.R))
            {
                if (!pressed)
                {
                    pressedKey = Keys.R;
                    pressed = true;
                    if (previousKey != Keys.Q)
                    {
                        previousKey = Keys.Q;
                        direction = transform.World.Up;
                    }
                }
            }


            
            object[] parameters = { direction, moveSpeed, gameTime};
            EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
            EventActionType.OnMove, parameters));

            object[] parametersTwo = {direction};
            EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
            EventActionType.MoveTongue, parametersTwo));

        }
        #endregion Actions - Update, Input
    }
}
