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
        private float snakeMoveSpeed;
        private float defaultMoveSpeed;
        private float totalMilliseconds;
        private float snakeMultiplier;

        #endregion Fields

        #region Properties
        public List<Character> SnakePartsListBodies
        {
            get
            {
                return snakePartsListBodies;
            }
            set
            {
                snakePartsListBodies = value;
            }
        }
        #endregion Properties

        #region Constructors
        public SnakeManager(Game game, GameObject snakePart, CubeMesh snakeBodyMesh, OctahedronMesh snakeTailMesh, Material snakeMaterial, float snakeMoveSpeed, float snakeMutplier) : base(game)
        {
            this.head = CreateSnakeHead(snakePart);
            this.snakePart = snakePart;
            //this.tail = snakePart.GetComponent<CharacterCollider>().Body as Character;
            //snakePartsListBodies.Add(tail);
            this.snakeBodyMesh = snakeBodyMesh;
            this.snakeTailMesh = snakeTailMesh;
            this.snakeMaterial = snakeMaterial;
            this.snakeMoveSpeed = snakeMoveSpeed;
            this.defaultMoveSpeed = snakeMoveSpeed;
            this.totalMilliseconds = 0f;
            this.snakeMultiplier = snakeMutplier;

            this.snakeNumber = 0;
        }
        #endregion Constructors

        protected override void SubscribeToEvents()
        {
            //handle add/remove events
            EventDispatcher.Subscribe(EventCategoryType.SnakeManager, HandleGameObjectEvents);

            base.SubscribeToEvents();
        }

        protected void HandleGameObjectEvents(EventData eventData)
        {
            switch (eventData.EventActionType)
            {
                case EventActionType.OnMove: 
                    Vector3 direction = (Vector3)eventData.Parameters[0];
                    GameTime gameTime = (GameTime)eventData.Parameters[1];

                    if(Application.StateManager.StartMove)
                    {
                        Move(direction, gameTime);
                    }                  
                    break;

                case EventActionType.Grow: 
                    Grow();
                    break;

                case EventActionType.ResetSnake:
                    ResetSnake();
                    break;

                case EventActionType.RemoveSnake:
                    GameObject removeSnakePart = (GameObject)eventData.Parameters[0];
                    RemoveSnake(removeSnakePart);
                    break;

                default:
                    break;
                    //add more cases for each method that we want to support with events
            }

            //call base method because we want to participate in the pause/play events
            base.HandleEvent(eventData);
        }


        private void RemoveSnake(GameObject removeSnakePart)
        {
                Application.SceneManager.ActiveScene.RemoveAll(ObjectType.Dynamic, RenderType.Opaque, (snake) => snake.GameObjectType == GameObjectType.SnakePart);
                Application.SceneManager.ActiveScene.RemoveAll(ObjectType.Dynamic, RenderType.Opaque, (snake) => snake.GameObjectType == GameObjectType.Player);

                EventDispatcher.Raise(new EventData(EventCategoryType.SceneManager,
                EventActionType.OnLose, new object[] { AppData.END_MENU_SCENE_NAME }));

            EventDispatcher.Raise(new EventData(EventCategoryType.StateManager,
            EventActionType.UpdateEndMenuScreenUIText, new object[] { AppData.SNAKE_MENU_UI_TEXT_HIT_SNAKE }));


        }

        private void ResetSnake()
        {

            Application.SceneManager.ActiveScene.RemoveAll(ObjectType.Dynamic, RenderType.Opaque, (snake) => snake.GameObjectType == GameObjectType.SnakePart);
            Application.SceneManager.ActiveScene.RemoveAll(ObjectType.Dynamic, RenderType.Opaque, (snake) => snake.GameObjectType == GameObjectType.Player);

            snakePartsListBodies.Clear();

            GameObject newHead = CreateSnakeHead(this.head);
            snakePartsListBodies.Add(newHead.GetComponent<CharacterCollider>().Body as Character);
           
            Application.SceneManager.ActiveScene.Add(newHead);           

            this.snakePart = newHead;
            Application.Player = newHead;

            this.snakeNumber = 0;

            this.snakeMoveSpeed = defaultMoveSpeed;

            Grow();
        }


        private void ZeroPosition(Character snake)
        {
            if (snake.Position.X > AppData.SNAKE_GAME_MAX_BOUNDARY)
            {
                snake.transform.Position.X = AppData.SNAKE_GAME_MIN_BOUNDARY;
            }
            else if (snake.Position.X < AppData.SNAKE_GAME_MIN_BOUNDARY)
            {
                snake.transform.Position.X = AppData.SNAKE_GAME_MAX_BOUNDARY;
            }
            else if (snake.Position.Y > AppData.SNAKE_GAME_MAX_BOUNDARY)
            {
                snake.transform.Position.Y = AppData.SNAKE_GAME_MIN_BOUNDARY;
            }
            else if (snake.Position.Y < AppData.SNAKE_GAME_MIN_BOUNDARY)
            {
                snake.transform.Position.Y = AppData.SNAKE_GAME_MAX_BOUNDARY;
            }
            else if (snake.Position.Z < AppData.SNAKE_GAME_MIN_BOUNDARY)
            {
                snake.transform.Position.Z = AppData.SNAKE_GAME_MAX_BOUNDARY;
            }
            else if (snake.Position.Z > AppData.SNAKE_GAME_MAX_BOUNDARY)
            {
                snake.transform.Position.Z = AppData.SNAKE_GAME_MIN_BOUNDARY;
            }
        }

        private float totalTime = 0f;
        private void Move(Vector3 newTranslation, GameTime gameTime)
        {

            Vector3 newTranslate;
  
            totalTime = totalTime + gameTime.ElapsedGameTime.Milliseconds;

            if(totalMilliseconds == 0f)
            {
                totalMilliseconds = gameTime.TotalGameTime.Milliseconds;
            }

            if (totalTime - totalMilliseconds < snakeMoveSpeed)
            {
                return;
            }
            totalMilliseconds = totalTime + gameTime.ElapsedGameTime.Milliseconds;


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

            gameObjectClone.GameObjectType = GameObjectType.SnakePart;
            Renderer renderer = gameObject.GetComponent<Renderer>();
        Renderer cloneRenderer = new Renderer(renderer.Effect, snakeMaterial, snakeBodyMesh);
        gameObjectClone.AddComponent(cloneRenderer);


            Collider cloneCollider = new SnakeCollider(gameObjectClone, true);

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
            gameObjectClone.GameObjectType = GameObjectType.SnakePart;

            Renderer renderer = gameObject.GetComponent<Renderer>();
            Renderer cloneRenderer = new Renderer(renderer.Effect, snakeMaterial, snakeTailMesh);
            gameObjectClone.AddComponent(cloneRenderer);

            Collider cloneCollider = new SnakeCollider(gameObjectClone, true);

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

        private GameObject CreateSnakeHead(GameObject gameObject)
        {
            GameObject gameObjectClone = CloneModelGameObjectSnake(gameObject, gameObject.Name, gameObject.Transform.Translation);
            gameObjectClone.GameObjectType = GameObjectType.Player;

            Renderer renderer = gameObject.GetComponent<Renderer>();
            Renderer cloneRenderer = new Renderer(renderer.Effect, renderer.Material, renderer.Mesh);
            gameObjectClone.AddComponent(cloneRenderer);

            Collider cloneCollider = new CharacterCollider(gameObjectClone, true);

            cloneCollider.AddPrimitive(
                  new Sphere(
                    gameObjectClone.Transform.Translation,
                    AppData.SCALE_AMOUNT / 2f
                    ),
                new MaterialProperties(0.8f, 0.8f, 0.7f)
                );

            cloneCollider.Enable(gameObjectClone, false, 1);
            gameObjectClone.AddComponent(cloneCollider);

            gameObjectClone.AddComponent(gameObject.GetComponent<CollidableSnakeController>());

            gameObjectClone.GetComponent<CollidableSnakeController>().Direction = new Vector3(AppData.SCALE_AMOUNT, 0, 0);

            gameObjectClone.AddComponent(new AudioListenerBehaviour());
            gameObjectClone.AddComponent(new AudioEmitterBehaviour());

            return gameObjectClone;
        }

        #region Snake Parts Methods
        public void Grow()
            {

            if (snakePartsListBodies.Count == 1)
            {
                GameObject tailGameObject = CreateSnakeTail(snakePart, "tail ", new Vector3(snakePart.Transform.Translation.X - snakePart.Transform.Scale.X, snakePart.Transform.Translation.Y, snakePart.Transform.Translation.Z));
                tail = tailGameObject.GetComponent<SnakeCollider>().Body as Character;
                snakePartsListBodies.Add(tail);
                Application.SceneManager.ActiveScene.Add(tailGameObject);

            }
            else
            {
                snakeNumber++;
                snakePart = CloneModelGameObjectSnakeBody(snakePart, "snake part " + snakeNumber, new Vector3(tail.Transform.Position.X, tail.Transform.Position.Y, tail.Transform.Position.Z));
                System.Diagnostics.Debug.WriteLine(snakePart.Name);
                snakePartsListBodies.Add(snakePart.GetComponent<SnakeCollider>().Body as Character);

                snakePartsListBodies.Remove(tail);
                snakePartsListBodies.Add(tail);

                Application.SceneManager.ActiveScene.Add(snakePart);
                }

            snakeMoveSpeed -= snakeMultiplier;

            if(snakePartsListBodies.Count > 2 && Application.StateManager.StartMove == true)
            {
                EventDispatcher.Raise(new EventData(EventCategoryType.StateManager,
                EventActionType.UpdateScore));
            }

        }
            
            #endregion Snake Parts Methods
        }
    }

