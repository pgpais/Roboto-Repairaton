using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game_Content.Scripts.Chaos_Events
{
    public class ControlsEventHandler : ChaosEventHandler
    {
        [Header("Event Properties")] 
        public float duration = 2f;

        public float invertStretchChance = 0.45f;
        public float invertRotationChance = 0.45f;
        public float invertBothChance = 0.1f;

        private RobotArm controls;

        private void Start()
        {
            ChaosManager.EventTrigger.AddListener(AcceptEvent);
            controls = GetComponent<RobotArm>();
        }

        public void AcceptEvent()
        {
            if((Random.Range(0f, 1f) <= eventAcceptChance))
                StartCoroutine("InvertControls");
        }

        IEnumerator InvertControls()
        {
            float rand = Random.Range(0f, 1f);

            if (rand <= invertStretchChance)
            {
                controls.stretchInverted = true;
            }
            else if (rand <= invertStretchChance + invertRotationChance)
            {
                controls.rotationInverted = true;
            }
            else
            {
                controls.stretchInverted = true;
                controls.rotationInverted = true;
            }
            yield return new WaitForSeconds(duration);

            controls.stretchInverted = false;
            controls.rotationInverted = false;
        }
    }
}