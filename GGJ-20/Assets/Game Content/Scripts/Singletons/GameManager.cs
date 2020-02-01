﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhalesAndGames.Pools;

/// <summary>
/// Singleton that serves as a base entrance for the game's logic.
/// </summary>
public class GameManager : SingletonBehaviour<GameManager>
{
    [Header("Game State")]
    public GameState gameState;
    public Reference reference;

    [Header("Pool Parts")]
    public PoolTable partPool;

    [Header("Conveyers")]
    public float startingSpawnTime;
    public ConveyerBelt[] conveyerBelts;

    [Header("Patterns")]
    public Pattern targetPattern;

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
        reference = Instantiate(reference);
    }

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        List<Part> parts = GameManager.Instance.reference.parts;
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

        // Starts the conveyer belts.
        gameState = GameState.Start;
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
