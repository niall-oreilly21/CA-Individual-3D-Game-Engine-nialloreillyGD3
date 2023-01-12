using GD.Engine.Events;
using GD.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GD.Engine.Globals;

namespace GD.Engine
{
  public class SnakeTongueController : Component
{
    private GameObject target;
    private Vector3 direction;
    private Vector3 newPosition;
    private float translateAmount;

    public SnakeTongueController(float translateAmount)
    {
        this.direction = new Vector3(0, 0, 0);
        this.newPosition = new Vector3(0, 0, 0);
        this.translateAmount = translateAmount;
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        //handle add/remove events
        EventDispatcher.Subscribe(EventCategoryType.Snake, HandleGameObjectEvents);
    }

    protected void HandleGameObjectEvents(EventData eventData)
    {
        switch (eventData.EventActionType)
        {
            case EventActionType.MoveTongue: 
                direction = (Vector3)eventData.Parameters[0];
                break;

            default:
                break;
        }
    }
    public override void Update(GameTime gameTime)
    {
        if (Application.Player != null)
            target = Application.Player;
        else
            throw new ArgumentNullException("Target not set! Do this in main");

        target = Application.Player;

        if (target != null)
        {
                System.Diagnostics.Debug.WriteLine(direction);
                if (direction.X == 0 && direction.Y == 0)
                {
                    direction.Z = direction.Z * translateAmount;
                }
                else if (direction.Z == 0 && direction.Y == 0)
                {
                    direction.X = direction.X * translateAmount;
                }
                else if (direction.X == 0 && direction.Z == 0)
                {
                    direction.Y = direction.Y * translateAmount;
                }
                newPosition = target.Transform.Translation
                + direction;

                //set new camera position
               
            transform.SetTranslation(newPosition);
        }

        //since parent Update does nothing then dont bother calling it
        //base.Update(gameTime);
    }
}
}
