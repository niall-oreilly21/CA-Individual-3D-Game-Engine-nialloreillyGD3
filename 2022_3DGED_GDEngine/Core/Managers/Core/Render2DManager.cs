using GD.Engine.Events;
using GD.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System.Collections.Generic;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using TextureFilter = Microsoft.Xna.Framework.Graphics.TextureFilter;

namespace GD.Engine
{
    public class Render2DManager : PausableDrawableGameComponent
    {
        #region Fields

        private SpriteBatch spriteBatch;
        private SceneManager<Scene2D> sceneManager;
        private SamplerState samplerState;
        private bool uiIsDrawn;

        #endregion Fields

        #region Constructors

        public Render2DManager(Game game, SpriteBatch spriteBatch,
            SceneManager<Scene2D> sceneManager)
     : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.sceneManager = sceneManager;

            //used when drawing textures
            samplerState = new SamplerState();
            samplerState.Filter = TextureFilter.Linear;
        }

        #endregion Constructors

        #region Actions - Draw

        protected override void SubscribeToEvents()
        {
            EventDispatcher.Subscribe(EventCategoryType.RenderUIGameObjects, HandleEvent);
            base.SubscribeToEvents();
        }

        protected override void HandleEvent(EventData eventData)
        {
            switch (eventData.EventActionType)
            {
                case EventActionType.UITextIsDrawn:
                    uiIsDrawn = true;
                    break;

                case EventActionType.UITextIsNotDrawn:
                    uiIsDrawn = false;
                    break;

                default:
                    break;
            }

            base.HandleEvent(eventData);
        }

        public override void Draw(GameTime gameTime)
        {
            if (IsDrawn)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, samplerState, null, null, null, null);
                foreach (GameObject gameObject in sceneManager.ActiveScene.ObjectList)
                {
                    List<Renderer2D> renderers = gameObject.GetComponents<Renderer2D>();
                    if (renderers != null)
                    {

                        //Check if game ui should be drawn
                        switch (uiIsDrawn)
                        {
                            case true:
                                foreach (Renderer2D renderer in renderers)
                                    renderer.Draw(spriteBatch);
                                break;

                            case false:
                                if(gameObject.GameObjectType != GameObjectType.UI_Game_Text)
                                foreach (Renderer2D renderer in renderers)
                                    renderer.Draw(spriteBatch);
                                break;
                        }
                    }
                            
                }
                spriteBatch.End();
            }

        }

        #endregion Actions - Draw
    }
}