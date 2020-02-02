using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game_Content.Scripts.Chaos_Events
{
    public class ChaosManager : MonoBehaviour
    {
        public static UnityEvent EventTrigger = new UnityEvent();

        public float eventInterval = 2f;

        private float nextEventTime;

        private void Start()
        {
            nextEventTime = Time.time + eventInterval;
        }

        private void Update()
        {
            if (Time.time >= nextEventTime)
            {
                EventTrigger.Invoke();
                Debug.Log("Event Fired");
                nextEventTime = Time.time + eventInterval;
            }
        }
    }
}