using System.Collections;
using UnityEngine;

namespace Game_Content.Scripts.Chaos_Events.ChaosEvents
{
    public abstract class ChaosEvent : ScriptableObject
    {
        public float warningDuration;
        public float eventDuration;

        /// <summary>
        /// Start the Event
        /// </summary>
        /// <param name="player"></param>
        public virtual IEnumerator StartEvent(RobotArm player)
        {
            Debug.Log(GetType().Name + " WARNING");
            EventWarning(player);
            yield return new WaitForSeconds(warningDuration);

            Debug.Log(GetType().Name + " Started on Player " + player.name + "! (evil laugh)");
            EventStart(player);
            yield return new WaitForSeconds(eventDuration);

            Debug.Log(GetType().Name + " Ended");
            EventStop(player);
            yield return null;
        }

        public abstract void EventWarning(RobotArm player);

        public abstract void EventStart(RobotArm player);

        public abstract void EventStop(RobotArm player);


    }
}