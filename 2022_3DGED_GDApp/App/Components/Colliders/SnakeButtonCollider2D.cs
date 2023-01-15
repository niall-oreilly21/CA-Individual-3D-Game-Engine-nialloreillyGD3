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
        }
    }
}
