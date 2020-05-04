using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Picks a game mode for the game and starts it.
/// </summary>
public class StartGameMode : MonoBehaviour
{
    [Header("Game Mode to Start")]
    [SerializeField]
    private GameMode gameMode;

    /// <summary>
    /// Starts the Game Mode 
    /// </summary>
    public void GameModeStart()
    {
        GlobalManager.Instance.GameMode = gameMode;
        FindObjectOfType<TitleMenu>().StartGame();
    }
}
