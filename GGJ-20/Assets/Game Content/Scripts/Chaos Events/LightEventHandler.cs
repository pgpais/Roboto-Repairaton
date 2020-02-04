using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// Makes lights flickers on and off reducing visibility on the field.
/// </summary>
public class LightEventHandler : ChaosEventHandler
{
    [Header("Event Properties")]
    public float offMinTime = 0.1f;
    public float offMaxTime = 0.5f;
    public float onMinTime = 0.1f;
    public float onMaxTime = 0.5f;
    public float duration = 5;

    private new Light2D light = null;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    protected override void Start()
    {
        base.Start();        
        light = GetComponent<Light2D>();
    }

    /// <summary>
    /// Starts the actual event.
    /// </summary>
    protected override IEnumerator StartEvent()
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
