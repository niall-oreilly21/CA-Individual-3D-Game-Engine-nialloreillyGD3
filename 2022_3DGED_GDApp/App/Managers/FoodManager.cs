using GD.App;
using GD.Engine.Events;
using GD.Engine.Globals;
using GD.Engine.Managers;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public class FoodManager : PausableGameComponent
    {
        GameObject food;
        private int foodNumber;
        public FoodManager(Game game, GameObject food) : base(game)
        {
            if (food == null)
            {
                throw new ArgumentNullException("Active object not set. Please make sure the food item is not null");
            }
            this.food = food;
        }

        protected override void SubscribeToEvents()
        {
            EventDispatcher.Subscribe(EventCategoryType.Food, HandleEvent);
        }

        protected override void HandleEvent(EventData eventData)
        {
            switch (eventData.EventActionType)
            {
                case EventActionType.RemoveFood:
                    GameObject removeFoodItem = (GameObject)eventData.Parameters[0];
                    RemoveFoodItem(removeFoodItem);
                    break;

                case EventActionType.AddFood:
                    InitializeFoodItem();
                    break;

                default:
                    break;
            }
           
        }

        private void RemoveFoodItem(GameObject removeFoodItem)
        {
            if (Application.SceneManager.ActiveScene.Remove(ObjectType.Static, RenderType.Opaque, (food) => food.Transform == removeFoodItem.Transform))
            {
                EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
                EventActionType.Grow));

                InitializeFoodItem();
                
            }
        }

        private void InitializeFoodItem()
        {
            GameObject snakePart;
            CharacterCollider snakePartCollider;

            GameObject head = Application.SnakeParts[0].Parent as GameObject;
            CharacterCollider headCollider = head.GetComponent<CharacterCollider>();

            if (headCollider.IsColliding)
            {
                headCollider.IsColliding = false;
            }

            food = CloneModelGameObject(AppData.FOOD_BASE_NAME + foodNumber);

            for (int i = 0; i < Application.SnakeParts.Count; i++)
            {
                snakePart = Application.SnakeParts[i].Parent as GameObject;
                snakePartCollider = snakePart.GetComponent<CharacterCollider>();

                if (snakePartCollider.IsColliding)
                {
                    food = CloneModelGameObject(AppData.FOOD_BASE_NAME + foodNumber);
                    snakePartCollider.IsColliding = false;        
                }
            }

            Application.SceneManager.ActiveScene.Add(food);

        }

        private Vector3 GetRandomTranslation()
        {
            Random random = new Random();
            int x = random.Next(AppData.SNAKE_GAME_MIN_SIZE, AppData.SNAKE_GAME_MAX_SIZE);
            int y = random.Next(AppData.SNAKE_GAME_MIN_SIZE, AppData.SNAKE_GAME_MAX_SIZE);
            int z = random.Next(AppData.SNAKE_GAME_MIN_SIZE, AppData.SNAKE_GAME_MAX_SIZE);

            Vector3 newTranslation = new Vector3(x,y,z);
            return newTranslation;
        }

        private GameObject CloneModelGameObject(string newName)
        {
            GameObject gameObjectClone = new GameObject(newName, food.ObjectType, food.RenderType);
            gameObjectClone.GameObjectType = food.GameObjectType;

            gameObjectClone.Transform = new Transform(
                food.Transform.Scale,
                food.Transform.Rotation,
                GetRandomTranslation()
                );

            Renderer renderer = food.GetComponent<Renderer>();
            Renderer cloneRenderer = new Renderer(renderer.Effect, renderer.Material, renderer.Mesh);
            gameObjectClone.AddComponent(cloneRenderer);


            Collider cloneCollider = new FoodCollider(gameObjectClone, true);

            cloneCollider.AddPrimitive(
                new Sphere(
                     gameObjectClone.Transform.Translation,
                    AppData.SCALE_AMOUNT / 2f
                    ),
                new MaterialProperties(0.8f, 0.8f, 0.7f)
                );

            cloneCollider.Enable(gameObjectClone, false, 1);
            gameObjectClone.AddComponent(cloneCollider);

            return gameObjectClone;

        }
    }
}
