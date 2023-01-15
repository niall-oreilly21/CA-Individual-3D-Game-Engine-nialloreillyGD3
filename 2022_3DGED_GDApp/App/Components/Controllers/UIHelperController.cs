using GD.App;
using GD.Engine.Globals;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public class UIHelperController : Component
    {
        private TextMaterial2D material2D;
        private string[] helperText;
        public UIHelperController(string[] helperText)
        {
            this.helperText = helperText;
        }

        public override void Update(GameTime gameTime)
        {
            material2D = (TextMaterial2D)gameObject.GetComponent<Renderer2D>().Material;

            material2D.StringBuilder.Clear(); 
            material2D.StringBuilder.Append(helperText[Application.StateManager.CurrentLevel - 1]);                  
        }
    }
}
