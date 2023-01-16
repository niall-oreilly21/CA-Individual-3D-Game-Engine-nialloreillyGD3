using GD.Engine.Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    public class MenuButton
    {
        #region Fields
        private Vector2 buttonTranslation;
        private Vector2 buttonTextOffSet;
        private Color buttonColor;
        private string buttonText;
        private EventData buttonEventData;

        #endregion Fields

        #region Properties
        public Vector2 ButtonTranslation
        {
            get
            {
                return buttonTranslation;
            }
        }

        public Vector2 ButtonTextOffSet
        {
            get
            {
                return buttonTextOffSet;
            }
        }

        public Color ButtonColor
        {
            get
            {
                return buttonColor;
            }
        }

        public string ButtonText
        {
            get
            {
                return buttonText;
            }
        }

        public EventData ButtonEventData
        {
            get
            {
                return buttonEventData;
            }
        }

        #endregion Properies
        public MenuButton(Vector2 buttonTranslation, Vector2 buttonTextOffSet, Color buttonColor, string buttonText, EventData buttonEventData)
        {
            this.buttonTranslation = buttonTranslation;
            this.buttonTextOffSet = buttonTextOffSet;
            this.buttonColor = buttonColor;
            this.buttonText = buttonText;
            this.buttonEventData = buttonEventData;
        }
    }
}
