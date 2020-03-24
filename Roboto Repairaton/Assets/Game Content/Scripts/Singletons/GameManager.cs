using Sirenix.OdinInspector;
using System;
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

    [Header("Arms")]
    [SerializeField]
    private GameObject singlePlayerArm = null;
    [SerializeField]
    private GameObject coopArms = null;

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

        // Spawns the expected wands.
        if(!GameMode.isCoOp)
        {
            Instantiate(singlePlayerArm, Vector2.zero, Quaternion.identity);
        }
        else
        {
            Instantiate(coopArms, Vector2.zero, Quaternion.identity);
        }

        // Updates the time displayed in the UI.
        Canvas.UpdatePatternTime(patternTime, patternTime);

        // Sets-up the pattern poll.
        List<Pattern> patterns = GlobalManager.Instance.Reference.patterns;
        if (patterns.Count == 0)
        {
            Debug.LogError("No Patterns Exist in the Reference!");
            yield break;
        }

        foreach (Pattern pattern in patterns)
        {
            PoolVariable variable = new PoolVariable(pattern, pattern.poolChance);
            patternPool.AddOrChangeVariable(variable);
        }

        // Sets-up the parts poll.
        List<Part> parts = GlobalManager.Instance.Reference.parts;
        if (parts.Count == 0)
        {
            Debug.LogError("No Parts Exist in the Reference!");
            yield break;
        }
 
        foreach (Part part in parts)
        {
            PoolVariable variable = new PoolVariable(part, part.poolChance);
            partPool.AddOrChangeVariable(variable);
        }

        // Populates list of conveyor belts
        conveyorBelts = FindObjectsOfType<ConveyorBelt>();
        
        // Fades-Out.
        GlobalManager.Instance.ProcessFade(true, 10);

        yield return new WaitForSeconds(1f);
        Canvas.PlayCountdown();

        yield return new WaitForSeconds(4f);

        // Starts the conveyor belts.
        GameState = GameState.Running;
        foreach(ConveyorBelt belt in conveyorBelts)
        {
            belt.StartSpawnParts();
        }

        Canvas.StartWindows();
        StartCoroutine(TimerCountdown());
        GenerateNewPattern();
    }

    /// <summary>
    /// Starts the countdown timer until the end of the game.
    /// </summary>
    public IEnumerator TimerCountdown()
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
    /// Shows the Game Over screen and stops the timescale completly.
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

        // Resets the pattern probability back to their original.
        if (GameMode.onlyGiveCurrentPieces)
        {
            partPool.AddOrChangeVariable(targetPattern.legsPart, 0);
            partPool.AddOrChangeVariable(targetPattern.bodyPart, 0);
            partPool.AddOrChangeVariable(targetPattern.headPart, 0);
        }
        else
        {
            partPool.AddOrChangeVariable(targetPattern.legsPart, targetPattern.legsPart.poolChance);
            partPool.AddOrChangeVariable(targetPattern.bodyPart, targetPattern.bodyPart.poolChance);
            partPool.AddOrChangeVariable(targetPattern.headPart, targetPattern.headPart.poolChance);
        }
        
        Pattern pickedPattern = Pool.Fetch<Pattern>(patternPool);
        targetPattern = pickedPattern;

        // Buffs the pattern pieces on the current pattern.
        partPool.AddOrChangeVariable(targetPattern.legsPart, 26);
        partPool.AddOrChangeVariable(targetPattern.bodyPart, 26);
        partPool.AddOrChangeVariable(targetPattern.headPart, 26);

        Canvas.ResetPatternCheckmarks();
        Canvas.UpdateShownPattern(pickedPattern);
        Canvas.UpdatePatternTime(patternCurrentTime, patternTime);

        StartCoroutine(PatternCountdown(patternId));
    }

    /// <summary>
    /// Gives bias to the current part to appear depending on the parts that have already been placed
    /// on the assembly zone.
    /// </summary>
    public void UpdatePartPoolBias()
    {
        int timeLeft = patternTime - patternCurrentTime;
        int extraBias = timeLeft * GlobalManager.Instance.GameMode.extraBiasPerSecond;
        if(AssemblyZone.assembledLegs == null)
        {
            partPool.AddOrChangeVariable(targetPattern.legsPart, 26 + extraBias);
            return;
        }
        else
        {
            partPool.AddOrChangeVariable(targetPattern.legsPart, 26);
        }

        if (AssemblyZone.assembledBody == null)
        {
            partPool.AddOrChangeVariable(targetPattern.bodyPart, 26 + extraBias);
            return;
        }
        else
        {
            partPool.AddOrChangeVariable(targetPattern.bodyPart, 26);
        }

        if (AssemblyZone.assembledHead == null)
        {
            partPool.AddOrChangeVariable(targetPattern.headPart, 26 + extraBias);
            return;
        }
        else
        {
            partPool.AddOrChangeVariable(targetPattern.headPart, 26);
        }
    }

    /// <summary>
    /// Countsdown the pattern time.
    /// </summary>
    public IEnumerator PatternCountdown(int patterNumber)
    {
        while(patternCurrentTime > 0 && patternId == patterNumber)
        {
            yield return new WaitForSeconds(1f);
            patternCurrentTime--;
            UpdatePartPoolBias();

            if (patternCurrentTime == 0)
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
    /// Triggers when an assembly is failed, throwing the pieces all away.
    /// </summary>
    public void FailAssembly()
    {
        score -= scorePenalty;
        if (score < 0)
        {
            score = 0;
        }

        Canvas.ResetPatternCheckmarks();
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
