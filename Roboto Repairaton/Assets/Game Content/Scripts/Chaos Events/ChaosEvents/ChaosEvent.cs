using System.Collections;
using UnityEngine;

namespace Game_Content.Scripts.Chaos_Events.ChaosEvents
{
    public abstract class ChaosEvent : ScriptableObject
    {
        public float warningDuration = 2;
        public float eventDuration = 2;

        /// <summary>
        /// Initializes variables and such of Event
        /// </summary>
        public abstract void EventInit();

        /// <summary>
        /// Start the Event
        /// </summary>
        /// <param name="player"></param>
        public virtual IEnumerator StartEvent(RobotArm player)
        {
            EventInit();
            
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

        /// <summary>
        /// Triggers the event warning (animation or coroutine? where to code it?)
        /// </summary>
        /// <param name="player">Player to be affected by the event</param>
        public abstract void EventWarning(RobotArm player);

        /// <summary>
        /// Triggers the event behaviour
        /// </summary>
        /// <param name="player">Player to be affected by the event</param>
        public abstract void EventStart(RobotArm player);

        /// <summary>
        /// Stops the event (and maybe play some animation and stuff?)
        /// </summary>
        /// <param name="player">Player to be affected by the event</param>
        public abstract void EventStop(RobotArm player);


    }
}