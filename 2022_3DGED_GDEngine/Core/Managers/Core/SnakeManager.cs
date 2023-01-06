using GD.App;
using GD.Engine.Events;
using GD.Engine.Globals;
using GD.Engine.Managers;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;

namespace GD.Engine
{
    /// <summary>
    /// Adds simple movement controller for Snake Head using keyboard
    /// </summary>
    public class SnakeManager : PausableGameComponent
    {


            #region Fields
            private SceneManager<Scene> sceneManager;

            private List<GameObject> snakePartsList = new List<GameObject>();
            private List<Character> snakePartsListBodies = new List<Character>();
            private GameObject head;
            private GameObject tail;
        float timeFlag = 0f;

        #endregion Fields

        #region Constructors
        public SnakeManager(Game game, GameObject head, SceneManager<Scene> sceneManager) : base(game)
        {
            this.head = head;
            this.tail = head;
            this.sceneManager = sceneManager;
            snakePartsList.Add(head);
            snakePartsListBodies.Add(head.GetComponent<CharacterCollider>().Body as Character);

            for (int i = 0; i < 10; i++)
            {
                Grow();
            }
        }
        #endregion Constructors

        protected override void SubscribeToEvents()
        {
            //handle add/remove events
            EventDispatcher.Subscribe(EventCategoryType.Snake, HandleGameObjectEvents);

            base.SubscribeToEvents();
        }

        protected void HandleGameObjectEvents(EventData eventData)
        {
            switch (eventData.EventActionType)
            {
                case EventActionType.OnMove: //TODO
                    Vector3 direction = (Vector3)eventData.Parameters[0];
                    float moveSpeed = (float)eventData.Parameters[1];
                    float multiplier = (float)eventData.Parameters[2];
                    GameTime gameTime = (GameTime)eventData.Parameters[3];
                    Keys pressedKey = (Keys)eventData.Parameters[4];


                    Move(direction, moveSpeed, multiplier, gameTime, pressedKey);
                    break;

                case EventActionType.Grow: //TODO
                    Grow();
                    break;

                case EventActionType.ResetVelocity:
                    ResetVelocity();
                    break;

                default:
                    break;
                    //add more cases for each method that we want to support with events
            }

            //call base method because we want to participate in the pause/play events
            base.HandleEvent(eventData);
        }

        private void ResetVelocity()
        {
            foreach(Character snakeHead in snakePartsListBodies)
            {
                snakeHead.Velocity = Vector3.Zero;
            }
        }

        private bool IsTail(GameObject gameObject)
        {
            System.Diagnostics.Debug.WriteLine(gameObject.Name);
            return tail.Transform.Translation.X == gameObject.Transform.Translation.X;
        }

        private void ZeroPosition(Character snake)
        {
            if (snake.Position.X > AppData.SNAKE_GAME_MAX_SIZE)
            {
                snake.transform.Position.X = 0;
            }
            else if (snake.Position.X < 0)
            {
                snake.transform.Position.X = AppData.SNAKE_GAME_MAX_SIZE;
            }
            else if (snake.Position.Y > AppData.SNAKE_GAME_MAX_SIZE)
            {
                snake.transform.Position.Y = 0;
            }
            else if (snake.Position.Y < 0)
            {
                snake.transform.Position.Y = AppData.SNAKE_GAME_MAX_SIZE;
            }
            else if (snake.Position.Z < 0)
            {
                snake.transform.Position.Z = AppData.SNAKE_GAME_MAX_SIZE;
            }
            else if (snake.Position.Z > AppData.SNAKE_GAME_MAX_SIZE)
            {
                snake.transform.Position.Z = 0;
            }
        }

        private float totalTime = 0f;
        private void Move(Vector3 newTranslation, float moveSpeed, float multiplier, GameTime gameTime, Keys pressedKey)
        {

            Vector3 newTranslate;
  
            totalTime = totalTime + gameTime.ElapsedGameTime.Milliseconds;
            if(timeFlag == 0f)
            {
                timeFlag = gameTime.TotalGameTime.Milliseconds;
            }


            if (totalTime - timeFlag < (100f))
            {
                return;
            }
            timeFlag = totalTime + gameTime.ElapsedGameTime.Milliseconds;

            for (int i = snakePartsListBodies.Count - 1; i > 0; i--)
            {

                ///head = 9 body = 0                           
                newTranslate = snakePartsListBodies[i - 1].Position - snakePartsListBodies[i].Position;

                //if (

                //(((int)Math.Round(snakePartsListBodies[i - 1].Position.X, 0) > (int)Math.Round(snakePartsListBodies[i].Position.X, 0)) && pressedKey == Keys.A) ||


                //(((int)Math.Round(snakePartsListBodies[i - 1].Position.Z, 0) > (int)Math.Round(snakePartsListBodies[i].Position.Z, 0)) && pressedKey == Keys.W) ||


                //(((int)Math.Round(snakePartsListBodies[i - 1].Position.Z, 0) < (int)Math.Round(snakePartsListBodies[i].Position.Z, 0)) && pressedKey == Keys.S)

                //)
                //{
                //    snakePartsListBodies[i].Position += newTranslation * (moveSpeed * multiplier) * gameTime.ElapsedGameTime.Milliseconds;
                //}
                //else
                //{

                /// newTranslate = 9
                    snakePartsListBodies[i].Position += newTranslate;
                //}



                //if (snakePartsListBodies[i].transform.Position.X > AppData.SNAKE_GAME_MAX_SIZE)
                //{
                //    snakePartsListBodies[i].transform.Position.X = 0;
                //}
                //else if (snakePartsListBodies[i].transform.Position.X < 0)
                //{
                //    zeroPosition = new Vector3(AppData.SNAKE_GAME_MAX_SIZE - 1, snakePartsListBodies[i].Position.Y, snakePartsListBodies[i].Position.Z) - snakePartsListBodies[i].Position;
                //    snakePartsListBodies[i].Position += zeroPosition; 
                //}

            }

            //Move head

            snakePartsListBodies[0].Position += newTranslation ;


            

            //if (snakePartsListBodies[0].transform.Position.X > AppData.SNAKE_GAME_MAX_SIZE)
            //{
            //        snakePartsListBodies[0].transform.Position.X = 0;
            //}
            //else if (snakePartsListBodies[0].transform.Position.X < 0)
            //{
            //    zeroPosition = new Vector3(AppData.SNAKE_GAME_MAX_SIZE - 1, snakePartsListBodies[0].Position.Y, snakePartsListBodies[0].Position.Z) - snakePartsListBodies[0].Position;
            //    snakePartsListBodies[0].Position += zeroPosition; //* (moveSpeed * multiplier) * gameTime.ElapsedGameTime.Milliseconds;

            //    System.Diagnostics.Debug.WriteLine("new translate" + zeroPosition);
            //    System.Diagnostics.Debug.WriteLine(snakePartsListBodies[0].Position);
            //    //    snakePartsListBodies[0].transform.Position.X = AppData.SNAKE_GAME_MAX_SIZE;
            //}
            ////Adjusting for edges
            //// for (int i = 0; i < snakePartsListBodies.Count; i++)
            //{

               ZeroPosition(snakePartsListBodies[0]);
            //}

        }

            public GameObject CloneModelGameObject(GameObject gameObject, string newName, Vector3 translation)
            {
                GameObject gameObjectClone = new GameObject(newName, gameObject.ObjectType, gameObject.RenderType);
                gameObjectClone.GameObjectType = gameObject.GameObjectType;

                gameObjectClone.Transform = new Transform(
                    gameObject.Transform.Scale,
                    gameObject.Transform.Rotation,
                    translation
                    );

                Renderer renderer = gameObject.GetComponent<Renderer>();
                Renderer cloneRenderer = new Renderer(renderer.Effect, renderer.Material, renderer.Mesh);
                gameObjectClone.AddComponent(cloneRenderer);


            Collider cloneCollider = new CharacterCollider(gameObjectClone, true);

            cloneCollider.AddPrimitive(
                new Box(
                    gameObjectClone.Transform.Translation,
                    gameObjectClone.Transform.Rotation,
                    gameObjectClone.Transform.Scale
                    ),
                new MaterialProperties(0.8f, 0.8f, 0.7f)
                );

            cloneCollider.Enable(gameObjectClone, false, 1);
            gameObjectClone.AddComponent(cloneCollider);

            return gameObjectClone;
            }

            #region Snake Parts Methods
            public void Grow()
            {
                tail = CloneModelGameObject(tail, "tail", new Vector3(tail.Transform.Translation.X - 2, tail.Transform.Translation.Y, tail.Transform.Translation.Z));
                snakePartsList.Add(tail);
            snakePartsListBodies.Add(tail.GetComponent<CharacterCollider>().Body as Character);
            sceneManager.ActiveScene.Add(tail);
            }
            #endregion Snake Parts Methods
        }
    }

