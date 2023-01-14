using GD.Engine.Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public class UIStartLevelTimerController : Component
    {
        float milliSeconds;
        double currentSecondDrawn;
        private TextMaterial2D material2D;

        public UIStartLevelTimerController(float milliSeconds)
        {
            this.milliSeconds = milliSeconds;
        }

        public override void Update(GameTime gameTime)
        {
            material2D = (TextMaterial2D)gameObject.GetComponent<Renderer2D>().Material;
            milliSeconds -= gameTime.ElapsedGameTime.Milliseconds;

            currentSecondDrawn = Math.Round((milliSeconds) / 1000d);

            material2D.StringBuilder.Clear();

            if (currentSecondDrawn == 0)
            {
                material2D.StringBuilder.Clear();
                material2D.StringBuilder.Append("GO!");
            }

            else if (currentSecondDrawn <= -1)
            {
                object[] parameters = { gameObject };
                EventDispatcher.Raise(new EventData(EventCategoryType.StateManager,
                EventActionType.RemoveUILevelStart, parameters));
            }
            else
            {
                material2D.StringBuilder.Append(currentSecondDrawn);
            }
        }
    }
}
