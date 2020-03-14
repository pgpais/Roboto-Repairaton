using System.Collections;
using UnityEngine;

/// <summary>
/// Inverts the player's controls.
/// </summary>
public class ControlsEventHandler : ChaosEventHandler
{
    [Header("General Event Properties")] 
    public float duration = 2f;
    public bool isFreezeEvent = true;
    public bool hasFrozen = false;
    
    [Header("Invert Event Properties")]
    public float invertChance = 0.2f;

    private RobotArm controls;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    protected override void Start()
    {
        base.Start();           
        controls = GetComponent<RobotArm>();
    }

    /// <summary>
    /// Starts the actual event.
    /// </summary>
    protected override IEnumerator StartEvent()
    {
        // If controls are already broken or frozen, avoid doing them twice.
        if(controls.controlsFrozen || controls.controlsInverted || hasFrozen)
        {
            yield break;
        }

        if (isFreezeEvent)
        {
            controls.FreezeControls(true);
            controls.ChangeSpriteColors(controls.frozenColor);
        }
        else
        {
            float rand = Random.Range(0f, 1f);
            if (rand <= invertChance)
            {
                controls.controlsInverted = true;
            }

            controls.InvertControls(true);
            controls.ChangeSpriteColors(controls.invertedColor);
        }

        yield return new WaitForSeconds(duration);

        // Clears any effects from the arms.
        hasFrozen = true;
        controls.InvertControls(false);
        controls.FreezeControls(false);
        controls.ChangeSpriteColors(Color.clear);

    }
}