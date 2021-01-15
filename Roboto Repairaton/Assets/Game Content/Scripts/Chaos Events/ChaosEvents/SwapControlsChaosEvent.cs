using UnityEngine;

namespace Game_Content.Scripts.Chaos_Events.ChaosEvents
{
    [CreateAssetMenu(fileName = "SwapControlsChaosEvent", menuName = "Chaos Event/Swap Controls", order = 0)]
    public class SwapControlsChaosEvent : ChaosEvent
    {
        private RobotArm otherPlayer;
        
        public override void EventInit()
        {
            ChaosManager chaosManager = ChaosManager.Instance;
            //otherPlayer = chaosManager.Players;
            otherPlayer = chaosManager.Players[Random.Range(0, chaosManager.Players.Count)];
        }

        public override void EventWarning(RobotArm player)
        {
            // Play animation
            
        }

        public override void EventStart(RobotArm player)
        {
            player.controlsSwapped = true;
            player.playerSwapped = otherPlayer;
        }

        public override void EventStop(RobotArm player)
        {
            player.controlsSwapped = false;
            player.playerSwapped = null;
        }
    }
}