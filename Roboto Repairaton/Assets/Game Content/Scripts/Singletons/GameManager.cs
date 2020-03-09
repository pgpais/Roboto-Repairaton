using Sirenix.OdinInspector;
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

    [Header("Game Values")]
    [ReadOnly]
    public int time;
    [ReadOnly]
    public int score;
    [ReadOnly]
    public int repairedRobots;
    [ReadOnly]
    public int partsDropped;
    [ReadOnly]
    public int timeScoreMultiplier;
    [ReadOnly]
    public int scorePenalty;

    [Header("Contribution")]
    [ReadOnly]
    public int player1Contribution;
    [ReadOnly]
    public int player2Contribution;

    [Header("Pool Parts")]
    public PoolTable partPool;
    public PoolTable patternPool;

    [Header("Conveyors")]
    public float startingSpawnTime;
    public ConveyorBelt[] conveyorBelts;

    [Header("Patterns")]
    public int patternTime = 25;
    private int patternCurrentTime;
    public Pattern targetPattern;
    private int patternId = 0;

    // Service Locator
    [HideInInspector]
    public AssemblyZone AssemblyZone;
    [HideInInspector]
    public CanvasManager Canvas;
    private GameMode GameMode;

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
        AssemblyZone = FindObjectOfType<AssemblyZone>();
        Canvas = FindObjectOfType<CanvasManager>();
    }

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private IEnumerator Start()
    {
        // Gets the current active game mode.
        GameMode = GlobalManager.Instance.GameMode;
        time = GameMode.gameTime;
        patternTime = GameMode.patternTime;
        scorePenalty = GameMode.scorePenalty;
        timeScoreMultiplier = GameMode.timeScoreMultiplier;

        // Updates the time displayed in the UI.
        Canvas.UpdatePatternTime(patternTime, patternTime);

        // Sets-up the part poll.
        List<Pattern> patterns = GlobalManager.Instance.Reference.patterns;
        if (patterns.Count == 0)
        {
            Debug.LogError("No Patterns Exist in the Reference!");
            yield break;
        }

        foreach (Pattern pattern in patterns)
        {
            PoolVariable variable = new PoolVariable(pattern, pattern.poolChance);
            patternPool.AddVariable(variable);
        }

        RefreshPartPool();

        // Populates list of conveyor belts
        conveyorBelts = FindObjectsOfType<ConveyorBelt>();
        
        // Fades-Out.
        GlobalManager.Instance.ProcessFade(true, 10);
        Canvas.UpdateGameTime(time);

        yield return new WaitForSeconds(3f);

        // Starts the conveyor belts.
        GameState = GameState.Running;
        foreach(ConveyorBelt belt in conveyorBelts)
        {
            belt.StartSpawnParts();
        }

        StartCoroutine(GameCountdown());
        GenerateNewPattern();
    }

    /// <summary>
    /// Starts the countdown timer until the end of the game.
    /// </summary>
    public IEnumerator GameCountdown()
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1f);
            time--;

            Canvas.UpdateGameTime(time);
        }

        GameOver();
    }

    /// <summary>
    /// Stops the Time Scale completly.
    /// </summary>
    private void GameOver()
    {
        Time.timeScale = 0;
        GameState = GameState.GameOver;

        Canvas.ShowGameOverScreen();
    }

    /// <summary>
    /// Generates a new pattern.
    /// </summary>
    public void GenerateNewPattern()
    {
        patternId++;
        patternCurrentTime = patternTime;
        Pattern pickedPattern = Pool.Fetch<Pattern>(patternPool);
        targetPattern = pickedPattern;

        RefreshPartPool();
        Canvas.UpdateShownPattern(pickedPattern);
        Canvas.UpdatePatternTime(patternCurrentTime, patternTime);

        StartCoroutine(PatternCountdown(patternId));
    }

    /// <summary>
    /// Refreshes the Part Pool each time a new pattern is picked.
    /// </summary>
    public void RefreshPartPool()
    {
        List<Part> parts = GlobalManager.Instance.Reference.parts;
        if (parts.Count == 0)
        {
            Debug.LogError("No Parts Exist in the Reference!");
            return;
        }

        partPool.poolVariables.Clear();
        foreach (Part part in parts)
        {
            if(!GameMode.onlyGiveCurrentPieces)
            {
                int poolChance = part.poolChance;
                if (part == targetPattern.headPart || part == targetPattern.bodyPart || part == targetPattern.legsPart)
                {
                    poolChance = 20;
                }

                PoolVariable variable = new PoolVariable(part, poolChance);
                partPool.AddVariable(variable);
            }
            else
            {
                if(part != targetPattern.headPart && part != targetPattern.bodyPart && part != targetPattern.legsPart)
                {
                    continue;
                }

                PoolVariable variable = new PoolVariable(part, part.poolChance);
                partPool.AddVariable(variable);
            }
        }
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

            if(patternCurrentTime == 0)
            {
                Canvas.ShakeClock();
            }
            Canvas.UpdatePatternTime(patternCurrentTime, patternTime);
        }

        if(patternId == patterNumber)
        {
            AssemblyZone.ThrowAll();
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
    /// Triggers when an assembly is failed.
    /// </summary>
    public void FailAssembly()
    {
        score -= scorePenalty;       
        if(score < 0)
        {
            score = 0;
        }

        Canvas.ShakeScoreRemove();
    }

    /// <summary>
    /// Increasces the score.
    /// </summary>
    public void ConfirmAssembly()
    {
        // Score is defined by the pattern time 
        score += patternCurrentTime * timeScoreMultiplier;
        repairedRobots += 1;

        Canvas.RecoverClock();
        Canvas.ShakeScoreAdd();
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
