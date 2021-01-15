using UnityEngine;

namespace Game_Content.Scripts.Chaos_Events.ChaosEvents
{
    [CreateAssetMenu(fileName = "InvertControlsChaosEvent", menuName = "Chaos Event/Invert Controls", order = 0)]
    public class InvertControlsChaosEvent : ChaosEvent
    {
        public override void EventInit()
        {
            
        }

        public override void EventWarning(RobotArm player)
        {
            // Play animation
        }

        public override void EventStart(RobotArm player)
        {
            player.InvertControls(true);
        }

        public override void EventStop(RobotArm player)
        {
            player.InvertControls(false);
        }
    }
}