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
        private long keyPressedTime = 0;


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
                if (pressed)
                {
                    return;
                }
                pressedKey = Keys.W;
                pressed = true;
                if (transform.Translation.Z > AppData.SNAKE_GAME_MAX_SIZE)
                {

                    snakeHead.transform.Position.Z = 0;
                }
                else
                {
                    snakeHead.transform.Position.Z++;
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

                if (snakeHead.transform.Position.Z < 0)
                {
                    snakeHead.transform.Position.Z = AppData.SNAKE_GAME_MAX_SIZE;
                }
                else
                {
                    snakeHead.transform.Position.Z--;
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
                if (snakeHead.transform.Position.X < 0)
                {
                    snakeHead.transform.Position.X = AppData.SNAKE_GAME_MAX_SIZE;
                }
                else
                {
                    snakeHead.transform.Position.X--;
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

                if (snakeHead.transform.Position.X > AppData.SNAKE_GAME_MAX_SIZE)
                {
                    snakeHead.transform.Position.X = 0;
                }
                else
                {

                    snakeHead.transform.Position.X++;
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
                if (snakeHead.transform.Position.Y < 0)
                {
                    snakeHead.transform.Position.Y = AppData.SNAKE_GAME_MAX_SIZE;
                }
                else
                {
                    snakeHead.transform.Position.Y--;
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
                if (snakeHead.transform.Position.Y > AppData.SNAKE_GAME_MAX_SIZE)
                {
                    snakeHead.transform.Position.Y = 0;
                }
                else
                {
                    snakeHead.transform.Position.Y++;
                    object[] parameters = { translation };
                    EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
                    EventActionType.OnMove, parameters));
                }
            }
        }
        #endregion Actions - Update, Input
    }
}
