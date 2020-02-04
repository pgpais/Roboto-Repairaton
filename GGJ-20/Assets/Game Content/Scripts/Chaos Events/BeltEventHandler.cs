using System.Collections;
using UnityEngine;

/// <summary>
/// Makes conveyer belts go faster and spawn more pieces over time.
/// </summary>
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

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        belt = GetComponent<ConveyerBelt>();
    }

    /// <summary>
    /// Starts the actual event.
    /// </summary>
    protected override IEnumerator StartEvent()
    {
        float localDur = duration;
        float defaultSpeed = belt.speed;
        float defaultSpawnRate = belt.spawningTime;
        float rand = Random.Range(0f, 1f);

        if (rand <= stopChance)
        {
            belt.stopped = true;
        }

        else if (rand <= stopChance + speedIncreaseChance)
        {
            belt.speed = defaultSpeed * speedMultiplier;
            belt.spawningTime = defaultSpawnRate / speedMultiplier;
        }
        else if (rand <= stopChance + speedIncreaseChance + invertChance)
        {
            belt.invertedDirection = true;
        }
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