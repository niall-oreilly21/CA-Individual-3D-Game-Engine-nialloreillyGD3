using GD.App;
using GD.Engine.Events;
using GD.Engine.Globals;
using GD.Engine.Managers;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public abstract class ConsumableManager : PausableGameComponent
    {
        GameObject consumable;
        int consumableID;

        public ConsumableManager(Game game, GameObject consumable) : base(game)
        {
            if (consumable == null)
            {
                throw new ArgumentNullException("Active object not set. Please make sure the consumable item is not null");
            }

            this.consumable = consumable;
            this.consumableID = 0;
        }

        #region Properties
        protected GameObject Consumable
        {
            get
            {
                return consumable;
            }

            set
            {
                consumable = value;
            }
        }

        protected int ConsumableID
        {
            get
            {
                return consumableID;
            }

            set
            {
                consumableID = value;
            }
        }
        #endregion Properties

        protected bool RemoveConsumable(GameObject consumableToRemove)
        {
            return Application.SceneManager.ActiveScene.Remove(ObjectType.Static, RenderType.Opaque, (consumable) => consumable.Transform == consumableToRemove.Transform);
        }

        protected void InitializeConsumableItem()
        {
            ResetSnakeHeadColliding();

            consumable = CloneModelGameObject(AppData.BOMB_BASE_NAME + ConsumableID);

            GameObject snakePart;
            CharacterCollider snakePartCollider;

            bool noCollision = false;

            while (!noCollision)
            {
                noCollision = true;
                for (int i = 0; i < Application.SnakeParts.Count; i++)
                {
                    snakePart = Application.SnakeParts[i].Parent as GameObject;
                    snakePartCollider = snakePart.GetComponent<CharacterCollider>();

                    if (snakePartCollider.IsColliding)
                    {
                        consumable = CloneModelGameObject(AppData.BOMB_BASE_NAME + ConsumableID);
                        snakePartCollider.IsColliding = false;
                        noCollision = false;
                        break;
                    }
                }

            }
            Application.SceneManager.ActiveScene.Add(Consumable);
            ConsumableID++;
        }

        protected virtual void InitializeConsumableItemsStart(int consumableNumber)
        {
            for(int i = 0; i < consumableNumber; i++)
            {
                InitializeConsumableItem();
            }
        }

        protected void ResetSnakeHeadColliding()
        {
            GameObject head = Application.SnakeParts[0].Parent as GameObject;
            CharacterCollider headCollider = head.GetComponent<CharacterCollider>();

            if (headCollider.IsColliding)
            {
                headCollider.IsColliding = false;
            }
        }

        private Vector3 GetRandomTranslation()
        {
            Random random = new Random();
            float x = random.Next((int)AppData.SNAKE_GAME_MIN_BOUNDARY, (int)AppData.SNAKE_GAME_MAX_BOUNDARY);
            float y = random.Next((int)AppData.SNAKE_GAME_MIN_BOUNDARY, (int)AppData.SNAKE_GAME_MAX_BOUNDARY);
            float z = random.Next((int)AppData.SNAKE_GAME_MIN_BOUNDARY, (int)AppData.SNAKE_GAME_MAX_BOUNDARY);

            Vector3 newTranslation = new Vector3(x, y, z);
            return newTranslation;
        }

        protected virtual GameObject CloneModelGameObject(string newName)
        {
            GameObject gameObjectClone = new GameObject(newName, consumable.ObjectType, consumable.RenderType);
            gameObjectClone.GameObjectType = consumable.GameObjectType;

            gameObjectClone.Transform = new Transform(
            consumable.Transform.Scale,
            consumable.Transform.Rotation,
            GetRandomTranslation()
            );

            Renderer renderer = consumable.GetComponent<Renderer>();
            Renderer cloneRenderer = new Renderer(renderer.Effect, renderer.Material, renderer.Mesh);
            gameObjectClone.AddComponent(cloneRenderer);

            return gameObjectClone;
        }
    }
}
