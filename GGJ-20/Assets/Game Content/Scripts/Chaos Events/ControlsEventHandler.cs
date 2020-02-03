﻿using System;
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

        private new void Start()
        {
            base.Start();
            
            controls = GetComponent<RobotArm>();
        }

        protected override IEnumerator StartEvent()
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
            controls.InvertControls(true);
            yield return new WaitForSeconds(duration);

            controls.stretchInverted = false;
            controls.rotationInverted = false;
            controls.InvertControls(false);

        }
    }
}