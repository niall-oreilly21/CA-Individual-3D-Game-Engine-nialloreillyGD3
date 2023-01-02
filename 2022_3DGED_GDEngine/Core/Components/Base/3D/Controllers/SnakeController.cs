using GD.App;
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

        private LinkedList<GameObject>snakePartsList = new LinkedList<GameObject>();
        private GameObject head;
        private GameObject tail;

        #endregion Fields

        #region Temps

        protected Vector3 translation = Vector3.Zero;
        protected Vector3 rotation = Vector3.Zero;
 

        #endregion Temps

        #region Constructors
        public SnakeController(GameObject head, SceneManager<Scene>sceneManager)
        {
            this.head = head;
            this.tail = head;
            this.sceneManager = sceneManager;

            snakePartsList.AddFirst(head);
            //grow();
            //grow();
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
                move(new Vector3(head.Transform.Translation.X, head.Transform.Translation.Y - 1, head.Transform.Translation.Z)); ;
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
                    move(new Vector3(head.Transform.Translation.X - 1, head.Transform.Translation.Y, head.Transform.Translation.Z));
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
                if (this.gameObject.Transform.Translation.X < 9)
                {
                    move(new Vector3(head.Transform.Translation.X + 1, head.Transform.Translation.Y, head.Transform.Translation.Z));
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

        private bool isTail(GameObject gameObject)
        {
            System.Diagnostics.Debug.WriteLine(gameObject.Name);
            return tail.Transform.Translation.X == gameObject.Transform.Translation.X;
        }
        private void move(Vector3 newTranslation)
        {

            System.Diagnostics.Debug.WriteLine("1");

            //tail = snakePartsList.Last();

                snakePartsList.RemoveLast();
            Predicate<GameObject> predicate = new Predicate<GameObject>(isTail);

            Predicate<GameObject> collisionPredicate =
            (collidableObject) =>
            {
                if (collidableObject != null)
                    return collidableObject.GameObjectType
                    == GameObjectType.Player;
                return false;
            };
            System.Diagnostics.Debug.WriteLine("2");
            //sceneManager.ActiveScene.RemoveAll(ObjectType.Dynamic, RenderType.Opaque, collisionPredicate);
            System.Diagnostics.Debug.WriteLine("3");

            head = CloneModelGameObject(head, "head", newTranslation);
            System.Diagnostics.Debug.WriteLine("4");
            snakePartsList.AddFirst(head);
            sceneManager.ActiveScene.Add(head);
            System.Diagnostics.Debug.WriteLine("5");
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

            return gameObjectClone;
        }
        #endregion Actions - Update, Input

        #region Snake Parts Methods
        public void grow()
        {
            tail = CloneModelGameObject(tail, "tail", new Vector3(tail.Transform.Translation.X - 1, tail.Transform.Translation.Y, tail.Transform.Translation.Z));
            snakePartsList.AddLast(tail);
            sceneManager.ActiveScene.Add(tail);
        }
        #endregion Snake Parts Methods
    }
}

