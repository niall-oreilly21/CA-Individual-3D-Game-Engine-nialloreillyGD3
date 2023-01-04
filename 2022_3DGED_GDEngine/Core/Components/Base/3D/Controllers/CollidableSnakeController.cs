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
        private CharacterCollider snakeHeadCollider;
        private Character snakeHead;
        private bool pressed = false;
        private Keys pressedKey;
        private Keys previousKey;
        private long keyPressedTime = 0;
        protected float moveSpeed = 0.01f;
        protected float multiplier = 1f;
        private Vector3 direction = new Vector3(0,0,0);


        public CollidableSnakeController(CharacterCollider snakeHeadCollider)
        {
            this.snakeHeadCollider = snakeHeadCollider;

            snakeHead = snakeHeadCollider.Body as Character;
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
                    if (previousKey != Keys.A)
                    {
                        previousKey = Keys.D;
                        direction = transform.World.Right;
                    }                  
                }

            }
            else if (Input.Keys.IsPressed(Keys.Left))
            {
                if (!pressed)
                {
                    pressedKey = Keys.Left;
                    if (previousKey != Keys.Right)
                    {
                        previousKey = Keys.Right;
                        direction = transform.World.Down;
                    }
                }
            }

            else if (Input.Keys.IsPressed(Keys.Right))
            {
                if (!pressed)
                {
                    pressedKey = Keys.Right;
                    if (previousKey != Keys.Left)
                    {
                        previousKey = Keys.Right;
                        direction = transform.World.Up;
                    }
                }
            }


            if (snakeHead.Position.X > AppData.SNAKE_GAME_MAX_SIZE)
            {
                snakeHead.transform.Position.X = 0;
            }
            else if(snakeHead.Position.X < 0)
            {
                snakeHead.transform.Position.X = AppData.SNAKE_GAME_MAX_SIZE;
            }
            else if (snakeHead.Position.Y > AppData.SNAKE_GAME_MAX_SIZE)
            {
                snakeHead.transform.Position.Y = 0;
            }
            else if (snakeHead.Position.Y < 0)
            {
                snakeHead.transform.Position.Y = AppData.SNAKE_GAME_MAX_SIZE;
            }
            else if (snakeHead.Position.Z < 0)
            {
                snakeHead.transform.Position.Z = AppData.SNAKE_GAME_MAX_SIZE;
            }
            else if (snakeHead.Position.Z > AppData.SNAKE_GAME_MAX_SIZE)
            {
                snakeHead.transform.Position.Z = 0;
            }
            object[] parameters = { direction, moveSpeed, multiplier, gameTime };
            EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
            EventActionType.OnMove, parameters));



        }
        #endregion Actions - Update, Input
    }
}
