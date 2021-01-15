using UnityEngine;

namespace Game_Content.Scripts.Chaos_Events.ChaosEvents
{
    [CreateAssetMenu(fileName = "FreezeControlsChaosEvent", menuName = "Chaos Event/Freeze Controls", order = 0)]
    public class FreezeControlsChaosEvent : ChaosEvent
    {
        public override void EventInit()
        {
            
        }

        public override void EventWarning(RobotArm player)
        {
            //TODO:
        }

        public override void EventStart(RobotArm player)
        {
            player.FreezeControls(true);
        }

        public override void EventStop(RobotArm player)
        {
            player.FreezeControls(false);
        }
    }
}