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

    [Header("Game Values")]
    public int time;
    public int score;

    [Header("Pool Parts")]
    public PoolTable partPool;
    public PoolTable patternPool;

    [Header("Conveyers")]
    public float startingSpawnTime;
    public ConveyerBelt[] conveyerBelts;

    [Header("Patterns")]
    public int patternMaxTime = 25;
    private int patternCurrentTime;
    public Pattern targetPattern;
    private int patternId = 0;

    // Service Locator
    [HideInInspector]
    public AssemblyZone AssemblyZone;
    [HideInInspector]
    public CanvasManager Canvas;

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
        Canvas = FindObjectOfType<CanvasManager>();
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
        GameState = GameState.Running;
        foreach(ConveyerBelt belt in conveyerBelts)
        {
            belt.StartSpawnParts();
        }

        Canvas.UpdateGameTime(time);

        StartCoroutine(GameCountdown());
        GenerateNewPattern();
    }

    /// <summary>
    /// Starts the countdown timer until the end of the game.
    /// </summary>
    public IEnumerator GameCountdown()
    {
        while(time > 0)
        {
            yield return new WaitForSeconds(1f);
            time--;

            Canvas.UpdateGameTime(time);
        }

        GameState = GameState.GameOver;
    }

    /// <summary>
    /// Generates a new pattern.
    /// </summary>
    public void GenerateNewPattern()
    {
        patternId++;
        patternCurrentTime = patternMaxTime;
        Pattern pickedPattern = Pool.Fetch<Pattern>(patternPool);
        targetPattern = pickedPattern;

        Canvas.UpdateShownPattern(pickedPattern);
        Canvas.UpdatePatternTime(patternCurrentTime, patternMaxTime);

        StartCoroutine(PatternCountdown(patternId));
    }

    /// <summary>
    /// Countsdown the pattern time.
    /// </summary>
    /// <returns></returns>
    public IEnumerator PatternCountdown(int patterNumber)
    {
        while(patternCurrentTime > 0 && patternId == patterNumber)
        {
            yield return new WaitForSeconds(1f);
            patternCurrentTime--;

            Canvas.UpdatePatternTime(patternCurrentTime, patternMaxTime);
        }

        if(patternId == patterNumber)
        {
            AssemblyZone.RemoveAll();
            GenerateNewPattern();
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

    /// <summary>
    /// Increasces the score.
    /// </summary>
    public void ConfirmAssembly()
    {
        // Score is defined by the pattern time 
        score += patternCurrentTime * 10;

        AssemblyZone.RemoveAll();
        GenerateNewPattern();
    }
}

/// <summary>
/// Defines the current state of the game in a reachable enum.
/// </summary>
public enum GameState
{
    Preparing,
    Running,
    GameOver,
}
