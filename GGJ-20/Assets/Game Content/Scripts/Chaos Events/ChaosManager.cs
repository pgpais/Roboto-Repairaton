using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game_Content.Scripts.Chaos_Events
{
    public class ChaosManager : MonoBehaviour
    {
        public static UnityEvent LightFlicker = new UnityEvent();

        public bool startEvent = false;

        private void Update()
        {
            if(startEvent)
                LightFlicker.Invoke();
            startEvent = false;
        }
    }
}