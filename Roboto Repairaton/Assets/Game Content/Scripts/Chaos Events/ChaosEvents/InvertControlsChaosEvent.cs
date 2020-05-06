using System.Collections;
using UnityEngine;

namespace Game_Content.Scripts.Chaos_Events
{
    [CreateAssetMenu(fileName = "InvertControlsChaosEvent", menuName = "Create Chaos Event/Invert Controls", order = 0)]
    public class InvertControlsChaosEvent : ChaosEvent
    {
        public override IEnumerator StartEvent(RobotArm player)
        {
            // Play animation
            Debug.Log(GetType().Name + " WARNING");
            yield return new WaitForSeconds(warningDuration);
            
            // Start event
            Debug.Log(GetType().Name + " Started | Player " + player.name + " his controls inverted! (evil laugh)");
            player.controlsInverted = true;
            yield return new WaitForSeconds(eventDuration);
            
            //Stop event
            Debug.Log(GetType().Name + " Ended");
            player.controlsInverted = false;
        }
    }
}