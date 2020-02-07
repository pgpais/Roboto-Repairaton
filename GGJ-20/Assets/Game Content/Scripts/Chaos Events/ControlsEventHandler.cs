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
    
    [Header("Invert Event Properties")]
    public float invertStretchChance = 0.45f;
    public float invertRotationChance = 0.45f;
    public float invertBothChance = 0.1f;


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
        if (isFreezeEvent)
        {
            controls.FreezeControls(true);
            controls.ChangeSpriteColors(controls.frozenColor);
        }
        else
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
            controls.ChangeSpriteColors(controls.invertedColor);
        }

        yield return new WaitForSeconds(duration);

        
        controls.stretchInverted = false;
        controls.rotationInverted = false;
        controls.FreezeControls(false);
        controls.InvertControls(false);
        controls.ChangeSpriteColors(Color.clear);

    }
}