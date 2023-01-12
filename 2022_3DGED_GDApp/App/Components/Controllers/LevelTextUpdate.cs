using GD.Engine.Events;
using GD.Engine.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Engine
{
    /// <summary>
    /// Class not needed anymore as UI Manager is a global variable
    /// </summary>
    //public class LevelTextUpdate : PausableGameComponent
    //{
    //    private TextMaterial2D material2D;
    //    string text;
    //    GameObject gameObject;

    //    public LevelTextUpdate(Game game, GameObject gameObject) : base(game)
    //    {
    //        material2D = (TextMaterial2D)gameObject.GetComponent<Renderer2D>().Material;
    //    }

    //    protected override void SubscribeToEvents()
    //    {
    //        EventDispatcher.Subscribe(EventCategoryType.UpdateUIElements, HandleGameObjectEvents);
    //    }

    //    protected void HandleGameObjectEvents(EventData eventData)
    //    {
    //        switch (eventData.EventActionType)
    //        {
    //            case EventActionType.UpdateUI: //TODO
    //                this.text = (string)eventData.Parameters[0];
    //                UpdateUI();
    //                break;

    //            default:
    //                break;
    //                //add more cases for each method that we want to support with events
    //        }
    //    }

    //    private void UpdateUI()
    //    {
    //        material2D.StringBuilder.Clear();
    //        material2D.StringBuilder.Append(text);
    //    }
    //}
}
