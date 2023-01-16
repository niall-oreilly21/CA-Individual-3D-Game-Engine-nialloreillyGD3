using GD.App;
using GD.Engine.Events;
using GD.Engine.Globals;
using GD.Engine.Inputs;
using Microsoft.Xna.Framework;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    /// <summary>
    /// Increases the size of a button when hovered over
    /// </summary>
    public class SnakeButtonCollider2D : ButtonCollider2D
    {
        #region Fields

        private float xOriginalPosition;
        private float yOriginalPosition;
        private float zOriginalPosition;
        private float scaleFacor;
        private Vector3 oldScale;
        private Vector3 newScale;
        private float offset;

        #endregion Fields

        #region Properties
        public float ScaleFacor
        {
            get
            {
                return scaleFacor;
            }
        }

        public float Offset
        {
            get
            {
                return offset;
            }
        }
        #endregion Properties

        public SnakeButtonCollider2D(GameObject gameObject, Renderer2D spriteRenderer, float scaleFactor, float offset) : base(gameObject, spriteRenderer)
        {
            this.xOriginalPosition = gameObject.Transform.Translation.X;
            this.yOriginalPosition = gameObject.Transform.Translation.Y;
            this.zOriginalPosition = gameObject.Transform.Translation.Z;
            this.scaleFacor = scaleFactor;

            this.oldScale = gameObject.Transform.Scale;
            this.newScale = gameObject.Transform.Scale;

            this.newScale.X += scaleFacor;
            this.newScale.Y += scaleFacor;
            this.newScale.Z += scaleFacor;
            this.offset = offset;
        }

        public override void CheckMouseOver()
        {
            base.CheckMouseOver();

            if (!Bounds.Intersects(Input.Mouse.Bounds))
            {
                gameObject.Transform.SetScale(oldScale);
                gameObject.Transform.SetTranslation(xOriginalPosition, yOriginalPosition, zOriginalPosition);
            }
        }
        protected override void HandleMouseHover()
        {
            base.HandleMouseHover();

            gameObject.Transform.SetScale(newScale);
            gameObject.Transform.SetTranslation(xOriginalPosition - offset, yOriginalPosition - offset / 2, zOriginalPosition);
        }

        protected override void HandleMouseClick(MouseButton mouseButton)
        {
            base.HandleMouseClick(mouseButton);

            EventDispatcher.Raise(new EventData(EventCategoryType.Sound, EventActionType.OnPlay2D, new object[] {AppData.BUTTON_CLICK_SOUND_NAME }));

            if (gameObject.Name == AppData.RESUME_BUTTON_NAME)
            {
                Application.StateManager.Enabled = true;

                EventDispatcher.Raise(new EventData(EventCategoryType.Sound, EventActionType.OnResume, new object[] { AppData.IN_GAME_BACKGROUND_SOUND_NAME }));
            }

            if (gameObject.Name == AppData.LEVEL_ONE_BUTTON_NAME || gameObject.Name == AppData.LEVEL_TWO_BUTTON_NAME || gameObject.Name == AppData.LEVEL_THREE_BUTTON_NAME || gameObject.Name == AppData.RESTART_BUTTON_NAME || gameObject.Name == AppData.NEXT_LEVEL_BUTTON_NAME)
            {
                Application.StateManager.Enabled = true;
            }

            if (gameObject.Name == AppData.MAIN_MENU_BUTTON_NAME)
            {
                EventDispatcher.Raise(new EventData(EventCategoryType.Sound, EventActionType.OnResume, new object[] { AppData.SNAKE_MENU_BACKGROUND_SOUND_NAME }));
            }
        }
    }
}
