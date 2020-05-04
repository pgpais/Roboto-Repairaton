using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

/// <summary>
/// Defines the Main Menu of the game.
/// </summary>
public class TitleMenu : MonoBehaviour
{
    // HACK
    public GameMode singlePlayer;
    public GameMode coOp;

    [SerializeField]
    private Animator menuAnimator = null;

    private bool hasStarted = false;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        // Fades-Out.
        GlobalManager.Instance.ProcessFade(true, 10);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (ReInput.players.GetSystemPlayer().GetButtonDown("UIQuit") || ReInput.players.GetPlayer(0).GetButtonDown("UIQuit")
|| ReInput.players.GetPlayer(1).GetButtonDown("UIQuit"))
        {
            QuitGame();
        }
    }

    /// <summary>
    /// Starts to the game.
    /// </summary>
    public void StartGame()
    {
        if(hasStarted)
        {
            return;
        }

        StartCoroutine(StartToGame());
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public IEnumerator StartToGame()
    {
        hasStarted = true;

        menuAnimator.enabled = true;
        yield return new WaitForSeconds(1f);
        GlobalManager.Instance.ChangeScene("Game", 10);
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
        return;
    }
}
