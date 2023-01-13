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
using System.Windows.Forms;
using Application = GD.Engine.Globals.Application;

namespace GD.Engine
{
    /// <summary>
    /// Adds simple movement controller for Snake Head using keyboard
    /// </summary>
    public class SnakeManager : PausableGameComponent
    {
            #region Fields
            private List<Character> snakePartsListBodies = new List<Character>();
            private GameObject snakePart;
            private GameObject head;
            private Character tail;
            private CubeMesh snakeBodyMesh;
            private OctahedronMesh snakeTailMesh;
        private Material snakeMaterial;
            private int snakeNumber;
            float timeFlag = 0f;

        #endregion Fields

        #region Constructors
        public SnakeManager(Game game, GameObject snakePart, CubeMesh snakeBodyMesh, OctahedronMesh snakeTailMesh, Material snakeMaterial) : base(game)
        {
            this.head = snakePart;
            this.snakePart = snakePart;
            this.tail = snakePart.GetComponent<CharacterCollider>().Body as Character;
            snakePartsListBodies.Add(tail);
            this.snakeBodyMesh = snakeBodyMesh;
            this.snakeTailMesh = snakeTailMesh;
            this.snakeMaterial = snakeMaterial;

            snakeNumber = 0;

            Application.SnakeParts = snakePartsListBodies;

            for (int i = 0; i < 2; i++)
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
                    GameTime gameTime = (GameTime)eventData.Parameters[1];

                    Move(direction, gameTime);
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

        //private bool IsTail(GameObject gameObject)
        //{
        //    System.Diagnostics.Debug.WriteLine(gameObject.Name);
        //    return tail.Transform.Translation.X == gameObject.Transform.Translation.X;
        //}

        private void ZeroPosition(Character snake)
        {
            if (snake.Position.X > AppData.SNAKE_GAME_MAX_SIZE)
            {
                snake.transform.Position.X = AppData.SNAKE_GAME_MIN_SIZE;
            }
            else if (snake.Position.X < AppData.SNAKE_GAME_MIN_SIZE)
            {
                snake.transform.Position.X = AppData.SNAKE_GAME_MAX_SIZE;
            }
            else if (snake.Position.Y > AppData.SNAKE_GAME_MAX_SIZE)
            {
                snake.transform.Position.Y = AppData.SNAKE_GAME_MIN_SIZE;
            }
            else if (snake.Position.Y < AppData.SNAKE_GAME_MIN_SIZE)
            {
                snake.transform.Position.Y = AppData.SNAKE_GAME_MAX_SIZE;
            }
            else if (snake.Position.Z < AppData.SNAKE_GAME_MIN_SIZE)
            {
                snake.transform.Position.Z = AppData.SNAKE_GAME_MAX_SIZE;
            }
            else if (snake.Position.Z > AppData.SNAKE_GAME_MAX_SIZE)
            {
                snake.transform.Position.Z = AppData.SNAKE_GAME_MIN_SIZE;
            }
        }

        private float totalTime = 0f;
        private void Move(Vector3 newTranslation, GameTime gameTime)
        {

            Vector3 newTranslate;
  
            totalTime = totalTime + gameTime.ElapsedGameTime.Milliseconds;
            if(timeFlag == 0f)
            {
                timeFlag = gameTime.TotalGameTime.Milliseconds;
            }

            System.Diagnostics.Debug.WriteLine(Application.SnakeMoveSpeed);

            if (totalTime - timeFlag < Application.SnakeMoveSpeed)
            {
                return;
            }
            timeFlag = totalTime + gameTime.ElapsedGameTime.Milliseconds;


            for (int i = snakePartsListBodies.Count - 1; i > 0; i--)
            {                        
                newTranslate = snakePartsListBodies[i - 1].Position - snakePartsListBodies[i].Position;
                snakePartsListBodies[i].Position += newTranslate;
            }

            //Move head
            snakePartsListBodies[0].Position += newTranslation ;
            ZeroPosition(snakePartsListBodies[0]);
        }

        private GameObject CloneModelGameObjectSnake(GameObject gameObject, string newName, Vector3 translation)
        {
            GameObject gameObjectClone = new GameObject(newName, gameObject.ObjectType, gameObject.RenderType);
            gameObjectClone.GameObjectType = gameObject.GameObjectType;

            gameObjectClone.Transform = new Transform(
                AppData.SNAKE_GAMEOBJECTS_SCALE,
                gameObject.Transform.Rotation,
                translation
                );


            return gameObjectClone;
        }
        private GameObject CloneModelGameObjectSnakeBody(GameObject gameObject, string newName, Vector3 translation)
        {
               
        GameObject gameObjectClone = CloneModelGameObjectSnake(gameObject, newName, translation);

        Renderer renderer = gameObject.GetComponent<Renderer>();
        Renderer cloneRenderer = new Renderer(renderer.Effect, snakeMaterial, snakeBodyMesh);
        gameObjectClone.AddComponent(cloneRenderer);


            Collider cloneCollider = new CharacterCollider(gameObjectClone, true);

        cloneCollider.AddPrimitive(
            new Box(
                gameObjectClone.Transform.Translation,
                gameObjectClone.Transform.Rotation,
                AppData.SNAKE_GAMEOBJECTS_COLLIDER_SCALE
                ),
            new MaterialProperties(0.8f, 0.8f, 0.7f)
            );

        cloneCollider.Enable(gameObjectClone, false, 1);
        gameObjectClone.AddComponent(cloneCollider);

        return gameObjectClone;
        }

        private GameObject CreateSnakeTail(GameObject gameObject, string newName, Vector3 translation)
        {
            GameObject gameObjectClone = CloneModelGameObjectSnake(gameObject, newName, translation);

            Renderer renderer = gameObject.GetComponent<Renderer>();
            Renderer cloneRenderer = new Renderer(renderer.Effect, snakeMaterial, snakeTailMesh);
            gameObjectClone.AddComponent(cloneRenderer);

            Collider cloneCollider = new CharacterCollider(gameObjectClone, true);

            cloneCollider.AddPrimitive(
                new Box(
                    gameObjectClone.Transform.Translation,
                    gameObjectClone.Transform.Rotation,
                    AppData.SNAKE_GAMEOBJECTS_COLLIDER_SCALE
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

            if (snakePartsListBodies.Count == 1)
            {
                GameObject tailGameObject = CreateSnakeTail(snakePart, "tail ", new Vector3(snakePart.Transform.Translation.X - snakePart.Transform.Scale.X, snakePart.Transform.Translation.Y, snakePart.Transform.Translation.Z));
                tail = tailGameObject.GetComponent<CharacterCollider>().Body as Character;
                snakePartsListBodies.Add(tail);
                Application.SceneManager.ActiveScene.Add(tailGameObject);

            }
            else
            {
                    snakeNumber++;
                    snakePart = CloneModelGameObjectSnakeBody(snakePart, "snake part " + snakeNumber, new Vector3(tail.Transform.Position.X, tail.Transform.Position.Y, tail.Transform.Position.Z));

                    snakePartsListBodies.Add(snakePart.GetComponent<CharacterCollider>().Body as Character);

                snakePartsListBodies.Remove(tail);
                snakePartsListBodies.Add(tail);
                //snakePartsListBodies[snakePartsListBodies.Count - 1].Position -= new Vector3(3,0,0); 


                    Application.SceneManager.ActiveScene.Add(snakePart);
                }

            Application.SnakeMoveSpeed -= AppData.SNAKE_MULTIPLIER;
            Application.StateManager.CurrentScore++;

            EventDispatcher.Raise(new EventData(EventCategoryType.StateManager,
            EventActionType.UpdateScore));
        }
            
            #endregion Snake Parts Methods
        }
    }

