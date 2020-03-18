using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the Game Over Canvas.
/// </summary>
public class GameOverCanvas : MonoBehaviour
{
    [Header("Game Over Canvas")]
    [SerializeField]
    private GameObject gameOverDialog = null;

    [Header("Game Over Elements")]
    [SerializeField]
    private TextMeshProUGUI gameModeText = null;
    [SerializeField]
    private GameObject repairedRobots = null;
    [SerializeField]
    private GameObject partsDropped = null;
    [SerializeField]
    private GameObject finalScore = null;
    [SerializeField]
    private GameObject divider = null;
    [SerializeField]
    private GameObject playerContribution = null;

    [Header("Buttons")]
    [SerializeField]
    private GameObject retryButton = null;

    /// <summary>
    /// Shows the game over screen.
    /// </summary>
    public void ShowGameOverScreen()
    {
        gameOverDialog.SetActive(true);

        gameModeText.text = "<b>Game Mode:</b> " + GlobalManager.Instance.GameMode.modeName;

        partsDropped.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = string.Format("{0:n0}", GameManager.Instance.partsDropped);
        repairedRobots.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = string.Format("{0:n0}", GameManager.Instance.repairedRobots);
        finalScore.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = string.Format("{0:n0}", GameManager.Instance.score);

        if(!GlobalManager.Instance.GameMode.isCoOp)
        {
            divider.SetActive(false);
            playerContribution.SetActive(false);
        }
        else
        {
            divider.SetActive(true);
            playerContribution.SetActive(true);

            playerContribution.transform.Find("Player1Text").GetComponent<TextMeshProUGUI>().text = string.Format("{0:n0}", GameManager.Instance.player1Contribution);
            playerContribution.transform.Find("Player2Text").GetComponent<TextMeshProUGUI>().text = string.Format("{0:n0}", GameManager.Instance.player2Contribution);
        }

        EventSystem.current.SetSelectedGameObject(retryButton);
    }

    /// <summary>
    /// Resets the Game.
    /// </summary>
    public void ResetGame()
    {
        Time.timeScale = 1;
        GlobalManager.Instance.ChangeScene("Game", 10);
    }

    /// <summary>
    /// Quits to the Menu.
    /// </summary>
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        GlobalManager.Instance.ChangeScene("Title", 10);
    }
}
