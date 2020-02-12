using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages chaos events, throwing in random madness to the player over time.
/// </summary>
public class ChaosManager : MonoBehaviour
{
    public static UnityEvent EventTrigger = new UnityEvent();

    [Header("Chaos Properties")]
    public float eventInterval = 2f;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        StartCoroutine(CauseEvents());
    }

    /// <summary>
    /// Causes events over time.
    /// </summary>
    private IEnumerator CauseEvents()
    {
        while (true)
        {
            yield return new WaitForSeconds(eventInterval);
            EventTrigger.Invoke();
        }
    }
}