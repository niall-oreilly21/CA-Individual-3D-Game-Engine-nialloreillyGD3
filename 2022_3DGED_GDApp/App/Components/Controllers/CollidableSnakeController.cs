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
        private bool pressed;
        private Keys pressedKey;
        private Keys previousKey;
        private Vector3 direction;


        public CollidableSnakeController()
        {
            pressed = false;
            direction = new Vector3(AppData.SCALE_AMOUNT, 0, 0);

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
                        transform.SetRotation(0, 0, 0);
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
                        transform.SetRotation(0, 0, 0);
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
                        transform.SetRotation(0, 0, 0);
                        direction = transform.World.Left;
                        transform.SetRotation(0, 90, 0);
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
                        transform.SetRotation(0, 0, 0);
                        direction = transform.World.Right;
                        transform.SetRotation(0, 90, 0);
                    }                  
                }

            }
            else if (Input.Keys.IsPressed(Keys.Q))
            {
                if (!pressed)
                {
                    
                    pressedKey = Keys.Q;
                    pressed = true;
                    if (previousKey != Keys.E)
                    {
                        previousKey = Keys.Q;
                        transform.SetRotation(0, 0, 0);
                        direction = transform.World.Down;
                        transform.SetRotation(0, 90, 0);

                        System.Diagnostics.Debug.WriteLine(transform.Rotation);
                    }
                }
            }

            else if (Input.Keys.IsPressed(Keys.E))
            {
                if (!pressed)
                {
                    
                    pressedKey = Keys.E;
                    pressed = true;
                    if (previousKey != Keys.Q)
                    {
                        previousKey = Keys.E;
                        transform.SetRotation(0, 0, 0);
                        direction = transform.World.Up;
                        transform.SetRotation(0, 90, 0);
                    }
                }
            }

            object[] parameters = { direction, gameTime};
            EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
            EventActionType.OnMove, parameters));

            object[] parametersTwo = {direction};
            EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
            EventActionType.MoveTongue, parametersTwo));

        }
        #endregion Actions - Update, Input
    }
}
