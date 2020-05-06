using System.Collections;
using UnityEngine;

namespace Game_Content.Scripts.Chaos_Events
{
    public abstract class ChaosEvent : ScriptableObject
    {
        public float warningDuration;
        public float eventDuration;

        /// <summary>
        /// Start the Event
        /// </summary>
        /// <param name="player"></param>
        public abstract IEnumerator StartEvent(RobotArm player);
    }
}