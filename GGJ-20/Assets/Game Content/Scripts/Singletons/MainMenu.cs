using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

/// <summary>
/// Defines the Main Menu of the game.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Animator menuAnimator;

    private bool hasStarted = false;

    // Start is called just before any of the Update methods is called the first time
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
        if (hasStarted)
        {
            return;
        }

        if(ReInput.players.GetSystemPlayer().GetButtonDown("UISubmit"))
        {
            hasStarted = true;
            StartCoroutine(StartToGame());
        }
    }


    /// <summary>
    /// Starts the game.
    /// </summary>
    public IEnumerator StartToGame()
    {
        menuAnimator.enabled = true;
        yield return new WaitForSeconds(1f);
        GlobalManager.Instance.ChangeScene("Game", 10);
    }
}
