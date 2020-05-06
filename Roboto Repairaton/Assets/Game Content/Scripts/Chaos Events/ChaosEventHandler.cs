using System.Collections;
using UnityEngine;

/// <summary>
/// Serves as events that can be called whenever chaos happens.
/// </summary>
public class ChaosEventHandler : MonoBehaviour
{
    [Header("Event Acceptance Options")] 
    [Range(0f, 1f)]
    public float eventAcceptChance = 0.1f;
    [Tooltip("How much acceptance increases per second?")]
    public float acceptanceIncrease = 0.01f;
        
    [Header("Standalone Options")]
    public bool standalone = false;
    [Tooltip("0 for it to be equal to period.")]
    public float firstTime = 0f;
    public float period = 15f;
    public float randomOffset = 2f;

    private float nextEvent = -1f;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    protected virtual void Start()
    {
        this.enabled = false;
        if (standalone)
        {
            if (firstTime == 0f)
            {
                firstTime = period;
            }
            nextEvent = Time.time + firstTime;
        }
        else
        {
            //ChaosManager.EventTrigger.AddListener(AcceptEvent);
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (standalone && Time.time >= nextEvent)
        {
            nextEvent = Time.time + period + Random.Range(-randomOffset, randomOffset);
            StartCoroutine(StartEvent());
        }
        else
        {
            eventAcceptChance += acceptanceIncrease * Time.deltaTime;
        }
    }

    /// <summary>
    /// Called if the event has been accepted.
    /// </summary>
    private void AcceptEvent()
    {
        if(!standalone && (Random.Range(0f, 1f) <= eventAcceptChance))
        {
            StartCoroutine(StartEvent());
        }
    }

    /// <summary>
    /// Starts the actual event.
    /// </summary>
    protected virtual IEnumerator StartEvent()
    {
        return null;
    }
}