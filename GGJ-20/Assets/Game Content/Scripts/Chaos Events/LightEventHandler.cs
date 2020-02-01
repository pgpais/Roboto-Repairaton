using System;
using System.Collections;
using System.Collections.Generic;
using Game_Content.Scripts.Chaos_Events;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class LightEventHandler : ChaosEventHandler
{
    [Header("Event Properties")]
    public float offMinTime = 0.1f;
    public float offMaxTime = 0.5f;
    public float onMinTime = 0.1f;
    public float onMaxTime = 0.5f;
    public float duration = 5;
    
    private Light2D light;
    
    
    private void Start()
    {
        ChaosManager.EventTrigger.AddListener(AcceptEvent);
        light = GetComponent<Light2D>();
    }

    

    public void AcceptEvent()
    {
        if((Random.Range(0f, 1f) <= eventAcceptChance))
            StartCoroutine("LightFlicker");
    }

    IEnumerator LightFlicker()
    {
        // I know, this is disgusting.
        float localDur = duration;
        while (true)
        {
            if (localDur <= 0)
            {
                light.enabled = true;
                break;
            }
            
            if (light.enabled)
            {
                light.enabled = false;

                float dur = Random.Range(offMinTime, offMaxTime);
                localDur -= dur;
                yield return new WaitForSeconds(dur);
            }
            else
            {
                light.enabled = true;

                float dur = Random.Range(onMinTime, onMaxTime);
                localDur -= dur;
                yield return new WaitForSeconds(dur);
            }
        }
    }
}
