using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhalesAndGames.Pools;

/// <summary>
/// Singleton that serves as a base entrance for the game's logic.
/// </summary>
public class GameManager : SingletonBehaviour<GameManager>
{
    [Header("Game State")]
    public GameState GameState;
    public Reference Reference;

    [Header("Pool Parts")]
    public PoolTable partPool;
    public PoolTable patternPool;

    [Header("Conveyers")]
    public float startingSpawnTime;
    public ConveyerBelt[] conveyerBelts;

    [Header("Patterns")]
    public Pattern targetPattern;

    // Serice Locator
    [HideInInspector]
    public AssemblyZone AssemblyZone;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
        }

        // Clones the reference for future-proofing.
        Reference = Instantiate(Reference);
        AssemblyZone = FindObjectOfType<AssemblyZone>();
    }

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        List<Part> parts = Reference.parts;
        if (parts.Count == 0)
        {
            Debug.LogError("No Parts Exist in the Reference!");
            return;
        }

        foreach (Part part in parts)
        {
            PoolVariable variable = new PoolVariable(part, part.poolChance);
            partPool.AddVariable(variable);
        }

        List<Pattern> patterns = Reference.patterns;
        if (parts.Count == 0)
        {
            Debug.LogError("No Patterns Exist in the Reference!");
            return;
        }

        foreach (Pattern pattern in patterns)
        {
            PoolVariable variable = new PoolVariable(pattern, pattern.poolChance);
            patternPool.AddVariable(variable);
        }

        // Starts the conveyer belts.
        GameState = GameState.Start;
        foreach(ConveyerBelt belt in conveyerBelts)
        {
            belt.StartSpawnParts();
        }
    }

    /// <summary>
    /// Requests a part to be spawned.
    /// </summary>
    /// <returns></returns>
    public Part RequestPartToSpawn()
    {
        if (partPool.poolVariables.Count == 0)
        {
            Debug.LogError("No Parts Exist in the Part Pool! Please double check!");
            return null;
        }

        Part pickedPart = Pool.Fetch<Part>(partPool);
        return pickedPart;
    }
}

/// <summary>
/// Defines the current state of the game in a reachable enum.
/// </summary>
public enum GameState
{
    Preparing,
    Start,
    GameOver,
}
