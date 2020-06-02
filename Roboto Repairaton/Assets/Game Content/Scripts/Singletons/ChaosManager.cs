using System.Collections;
using System.Collections.Generic;
using Game_Content.Scripts.Chaos_Events;
using Game_Content.Scripts.Chaos_Events.ChaosEvents;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEditor;


/// <summary>
/// Manages chaos events, throwing in random madness to the player over time.
/// </summary>
public class ChaosManager : SingletonBehaviour<ChaosManager>
{
    [Tooltip("How long before the first event fires")]
    public float startEventsTime = 4f;
    [ReadOnly][SerializeField]
    public List<ChaosEvent>[] playerEventHit;
    [SerializeField]
    private List<ChaosEvent> events;
    [ReadOnly] [SerializeField]private List<RobotArm> players;
    public List<RobotArm> Players => players;

    [ReadOnly] private int targetPlayer;
    [ReadOnly] private int nextEvent;
    [ReadOnly] private float nextEventTime;

    [SerializeField]
    private float lateStartDelay = 0.5f;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        nextEvent = Random.Range(0, events.Count - 1);
        
        nextEventTime = Time.time + startEventsTime; // Delay so events don't happen as soon as game starts

        StartCoroutine(LateStart(lateStartDelay)); // TODO: This shouldn't be necessary with scripts order. Discuss
    }

    private IEnumerator LateStart(float waitTime)
    {

        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            if (GameManager.Instance.GameState == GameState.Running)
            {
                Debug.Log("Game Running");
                StartCoroutine(ManageEvents());
                break;
            }
        }
        
        Debug.Log("Looking for players");
        
        players = new List<RobotArm>(GameObject.FindObjectsOfType<RobotArm>());
        playerEventHit = new List<ChaosEvent>[players.Count];
        
        for (int i = 0; i < playerEventHit.Length; i++)
        {
            playerEventHit[i] = new List<ChaosEvent>(events);
        }
    }

    /// <summary>
    /// Loop that manages events. It handles triggering and picking a new event.
    /// </summary>
    /// <returns></returns>
    IEnumerator ManageEvents()
    {
        // Wait the first few seconds
        Debug.Log("Waiting " + nextEventTime + " seconds before launching first event.");
        yield return new WaitForSeconds(nextEventTime); //TODO: seems to happen way too fast, maybe increase this
        while (true)
        {
            PickEvent();
            DoEvent();
            Debug.Log("Waiting " + nextEventTime + " seconds before launching next event.");
            yield return new WaitForSeconds(nextEventTime);
        }
    }

    void DoEvent()
    {
        // Launch event behaviour
        ChaosEvent ev = playerEventHit[targetPlayer][nextEvent];
        Debug.Log("Launching " + ev.GetType().Name + " event");
        StartCoroutine(ev.StartEvent(players[targetPlayer]));

        // Set event and player combo as picked
        playerEventHit[targetPlayer].RemoveAt(nextEvent);
    }
    
    void PickEvent()
    {
        Debug.Log("Picking new target player");
       // Randomize next target player
       targetPlayer = Random.Range(0, players.Count);
       for (int i = 0; playerEventHit[targetPlayer].Count == 0; i++)
       {
           // targetPlayer was already hit with every event (unlucky)
           targetPlayer = (targetPlayer + 1) % players.Count;
           if (i >= players.Count)
           {
               Debug.LogError("All players have been hit with every event! Pausing game.", this);
#if UNITY_EDITOR
               EditorApplication.isPaused = true;
#endif
               break;
           }
       }
       Debug.Log("Picked player " + targetPlayer);
       
       // Randomize next event for target player
       Debug.Log("Picking new event");
       nextEvent = Random.Range(0, playerEventHit[targetPlayer].Count);
    }
}