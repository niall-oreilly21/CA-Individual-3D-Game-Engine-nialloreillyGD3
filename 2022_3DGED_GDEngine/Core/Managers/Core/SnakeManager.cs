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

                case EventActionType.Grow: //TODO
                    grow();
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
            Vector3 newPosition;
            List<Character> snakeHeads = new List<Character>();

            for(int i = 0; i < snakePartsList.Count; i++)
            {
                snakeHeads.Add(snakePartsList[i].GetComponent<CharacterCollider>().Body as Character);
            }

            for(int i = snakePartsList.Count - 1; i > 0; i--)
            {
                newTranslate = snakePartsList[i - 1].Transform.Translation - snakePartsList[i].Transform.Translation;
                snakePartsList[i].Transform.Translate(newTranslate);
                newPosition = snakePartsList[i].Transform.Translation;

                snakeHeads[i].transform.Position = newPosition;

                System.Diagnostics.Debug.WriteLine(snakeHeads[i].transform.Position);
            }
            head.Transform.Translate(newTranslation);

            snakeHeads[0].transform.Position = head.Transform.Translation;

            System.Diagnostics.Debug.WriteLine("Force:" + snakeHeads[0].Force);
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
            public void grow()
            {
                tail = CloneModelGameObject(tail, "tail", new Vector3(tail.Transform.Translation.X - 1, tail.Transform.Translation.Y, tail.Transform.Translation.Z));
                snakePartsList.Add(tail);
                sceneManager.ActiveScene.Add(tail);
            }
            #endregion Snake Parts Methods
        }
    }

