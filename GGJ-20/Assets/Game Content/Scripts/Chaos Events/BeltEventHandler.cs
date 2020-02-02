using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game_Content.Scripts.Chaos_Events
{
    public class BeltEventHandler : ChaosEventHandler
    {
        [Header("Event Properties")]
        public float duration = 2f;
        public float speedIncreaseChance = 0.35f;
        public float invertChance = 0.35f;
        public float stopChance = 0.2f;
        public float invertSpeedIncreaseChance = 0.1f;
        public float speedMultiplier = 2f;

        private ConveyerBelt belt;

        private void Start()
        {
            belt = GetComponent<ConveyerBelt>();
            ChaosManager.EventTrigger.AddListener(Acceptevent);
        }

        public void Acceptevent()
        {
            if((Random.Range(0f, 1f) <= eventAcceptChance))
                StartCoroutine("ErraticBeltMovement");
        }

        IEnumerator ErraticBeltMovement()
        {
            float localDur = duration;
            float defaultSpeed = belt.speed;
            float defaultSpawnRate = belt.spawningTime;
            float rand = Random.Range(0f, 1f);
            if (rand <= stopChance)
                belt.stopped = true;
            else if (rand <= stopChance + speedIncreaseChance)
            {
                belt.speed = defaultSpeed * speedMultiplier;
                belt.spawningTime = defaultSpawnRate / speedMultiplier;
            }
            else if (rand <= stopChance + speedIncreaseChance + invertChance)
                belt.invertedDirection = true;
            else
            {
                belt.speed = defaultSpeed * speedMultiplier;
                belt.spawningTime = defaultSpawnRate / speedMultiplier;
                belt.invertedDirection = true;
            }
            yield return new WaitForSeconds(duration);
            
            belt.speed = defaultSpeed;
            belt.spawningTime = defaultSpawnRate;
            belt.stopped = false;
            belt.invertedDirection = false;
        }
    }
}