using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles the Game Over Canvas.
/// </summary>
public class GameOverCanvas : MonoBehaviour
{
    [Header("Game Over Canvas")]
    [SerializeField]
    private GameObject gameOverDialog;
    [Header("Game Over Elements")]
    [SerializeField]
    private GameObject repairedRobots;
    [SerializeField]
    private GameObject partsDropped;
    [SerializeField]
    private GameObject finalScore;

    /// <summary>
    /// Shows the game over screen.
    /// </summary>
    public void ShowGameOverScreen()
    {
        gameOverDialog.SetActive(true);

        partsDropped.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = string.Format("{0:n0}", GameManager.Instance.partsDropped);
        repairedRobots.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = string.Format("{0:n0}", GameManager.Instance.repairedRobots);
        finalScore.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = string.Format("{0:n0}", GameManager.Instance.score);
    }
}
