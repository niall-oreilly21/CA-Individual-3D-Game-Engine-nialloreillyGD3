using GD.Engine.Events;
using GD.Engine.Globals;
using GD.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            private GameObject head;
            private GameObject tail;

            #endregion Fields

        #region Constructors
        public SnakeManager(Game game, GameObject head, SceneManager<Scene> sceneManager) : base(game)
        {
            this.head = head;
            this.tail = head;
            this.sceneManager = sceneManager;
            snakePartsList.Add(head);

            for(int i = 0; i < 4; i++)
            {
                grow();
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
                    move((Vector3)eventData.Parameters[0]);
                    break;

                default:
                    break;
                    //add more cases for each method that we want to support with events
            }

            //call base method because we want to participate in the pause/play events
            base.HandleEvent(eventData);
        }

        private bool isTail(GameObject gameObject)
        {
            System.Diagnostics.Debug.WriteLine(gameObject.Name);
            return tail.Transform.Translation.X == gameObject.Transform.Translation.X;
        }
        private void move(Vector3 newTranslation)
        {
            Vector3 newTranslate;
            for(int i = snakePartsList.Count - 1; i > 0; i--)
            {
                newTranslate = snakePartsList[i - 1].Transform.Translation - snakePartsList[i].Transform.Translation;

                snakePartsList[i].Transform.Translate(newTranslate);

                System.Diagnostics.Debug.WriteLine(snakePartsList[i].Transform.Translation);
            }
            head.Transform.Translate(newTranslation);
            //tail = snakePartsList.Last();

            //snakePartsList.RemoveLast();
            //Predicate<GameObject> predicate = new Predicate<GameObject>(isTail);

            //Predicate<GameObject> collisionPredicate =
            //(collidableObject) =>
            //{
            //    if (collidableObject != null)
            //        return collidableObject.GameObjectType
            //        == GameObjectType.Player;
            //    return false;
            //};
            //sceneManager.ActiveScene.Remove(ObjectType.Dynamic, RenderType.Opaque, collisionPredicate);
            
            //GameObject newHead = CloneModelGameObject(head, "head", newTranslation);

            //Predicate<Component> snake =
            //  (collidableObject) =>
            //  {
            //      if (collidableObject != null)
            //          return collidableObject.gameObject.Transform.Translation
            //          == head.Transform.Translation;
            //      return false;
            //  };

            //head.RemoveComponent(snake);

            //System.Diagnostics.Debug.WriteLine("4");
            //snakePartsList.AddFirst(newHead);
            //head = newHead;
            //sceneManager.ActiveScene.Add(newHead);
            //System.Diagnostics.Debug.WriteLine("5");
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

            #region Snake Parts Methods
            public void grow()
            {
                tail = CloneModelGameObject(tail, "tail", new Vector3(tail.Transform.Translation.X - 1, tail.Transform.Translation.Y, tail.Transform.Translation.Z));
                snakePartsList.Add(tail);
                sceneManager.ActiveScene.Add(tail);
            }
            #endregion Snake Parts Methods
        }
    }

