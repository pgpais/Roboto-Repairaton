using UnityEngine;
using System.Collections;

/// <summary>
/// Game Mode specify the difference between a single-player and multiplayer mode, the times between orders,
/// and other game relevant variables.
/// </summary>
[CreateAssetMenu(fileName = "New Game Mode", menuName = "Robots/Game Mode")]
public class GameMode : ScriptableObject
{
    [Header("Mode Information")]
    public string modeId;
    public string modeName;
    public bool isCoOp;

    [Header("Game Variables")]
    public bool onlyGiveCurrentPieces;
    public int gameTime;
    public int patternTime;
    public int timeScoreMultiplier;
    public int scorePenalty;
}
