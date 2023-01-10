﻿using GD.App;
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
    public class FoodManager : ConsumableManager
    {
        public FoodManager(Game game, GameObject consumable) : base(game, consumable)
        {
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
                    RemoveConsumableItem(removeFoodItem);
                    break;

                case EventActionType.AddFood:
                    InitializeConsumableItem();
                    break;

                default:
                    break;
            }

        }

        protected override void RemoveConsumableItem(GameObject consumableToRemove)
        {
            if(base.RemoveConsumable(consumableToRemove))
            {
                EventDispatcher.Raise(new EventData(EventCategoryType.Snake,
                EventActionType.Grow));
            }
        }
        protected override void InitializeConsumableItem()
        {
            base.ResetSnakeHeadColliding();

            Consumable = CloneModelGameObject(AppData.FOOD_BASE_NAME + ConsumableID);

            GameObject snakePart;
            CharacterCollider snakePartCollider;

            for (int i = 0; i < Application.SnakeParts.Count; i++)
            {
                snakePart = Application.SnakeParts[i].Parent as GameObject;
                snakePartCollider = snakePart.GetComponent<CharacterCollider>();

                if (snakePartCollider.IsColliding)
                {
                    Consumable = CloneModelGameObject(AppData.FOOD_BASE_NAME + ConsumableID);
                    snakePartCollider.IsColliding = false;
                }
            }

            Application.SceneManager.ActiveScene.Add(Consumable);
            ConsumableID++;
        }

        protected override GameObject CloneModelGameObject(string newName)
        {

            GameObject gameObjectClone = base.CloneModelGameObject(newName);

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

            gameObjectClone.AddComponent(new FoodController(AppData.FOOD_ROTATE_SPEED));

            return gameObjectClone;

        }
    }
}
