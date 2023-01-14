using GD.App;
using GD.Engine.Events;
using GD.Engine.Globals;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public class UITimerController : Component
    {
        private TextMaterial2D material2D;
        public UITimerController()
        {
        }

        public override void Update(GameTime gameTime)
        {
            material2D = (TextMaterial2D)gameObject.GetComponent<Renderer2D>().Material;

            material2D.StringBuilder.Clear();

            if(Application.StateManager.Seconds <= 9)
            {
                material2D.StringBuilder.Append(Application.StateManager.Minutes + ":0" + Application.StateManager.Seconds);
            }
            else if(Application.StateManager.Seconds == 60)
            {
                material2D.StringBuilder.Append("0:00");
            }
            else
            {
                material2D.StringBuilder.Append(Application.StateManager.Minutes + ":" + Application.StateManager.Seconds);
            }
            
            
        }
    }
}
