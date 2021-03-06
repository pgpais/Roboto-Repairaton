﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Handles images and patterns on the canvas manager.
/// </summary>
public class CanvasManager : MonoBehaviour
{
    [Header("Pattern Window")]
    [SerializeField]
    private GameObject patternWindow = null;
    [SerializeField]
    private GameObject newOrderWindow = null;

    // Private
    private GameObject patternTimer;
    private Image patternTimerRadial;
    private TextMeshProUGUI patternTimerText;

    [Header("Time and Score")]
    [SerializeField]
    private GameObject scoreWindow = null;
    [SerializeField]
    private GameObject timeLeftWindow = null;
    [SerializeField]
    private Animator timeLeftWarning = null;

    [Header("UI Elements")]
    [SerializeField]
    private GameObject countdown;

    // Text
    private TextMeshProUGUI scoreText = null;
    private TextMeshProUGUI highscoreText = null;
    private TextMeshProUGUI timeLeftText = null;

    // Images
    private Image patternWindowLegs;
    private Image patternWindowBody;
    private Image patternWindowHead;
    private Image newOrderWindowLegs;
    private Image newOrderWindowBody;
    private Image newOrderWindowHead;

    // Animators
    private Animator patternTimerAnimator;
    private Animator patternAnimator;
    private Animator scoreAnimator;
    private Animator timeLeftAnimator;

    private int displayScore = 0;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        scoreText = scoreWindow.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        highscoreText = scoreWindow.transform.Find("Highscore").GetComponent<TextMeshProUGUI>();
        timeLeftText = timeLeftWindow.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        patternAnimator = patternWindow.GetComponent<Animator>();
        scoreAnimator = scoreWindow.GetComponent<Animator>();
        timeLeftAnimator = timeLeftWindow.GetComponent<Animator>();

        patternTimer = patternWindow.transform.Find("Pattern Timer").gameObject;
        patternTimerAnimator = patternTimer.GetComponent<Animator>();
        patternTimerRadial = patternTimer.transform.GetChild(0).GetComponent<Image>();
        patternTimerText = patternTimerRadial.transform.GetComponentInChildren<TextMeshProUGUI>();

        patternWindowLegs = patternWindow.transform.Find("Legs").GetComponent<Image>();
        patternWindowBody = patternWindow.transform.Find("Body").GetComponent<Image>();
        patternWindowHead = patternWindow.transform.Find("Head").GetComponent<Image>();

        Transform newOrderInfo = newOrderWindow.transform.Find("New Order Window").Find("New Order Info");

        newOrderWindowLegs = newOrderInfo.Find("Legs").GetComponent<Image>();
        newOrderWindowBody = newOrderInfo.Find("Body").GetComponent<Image>();
        newOrderWindowHead = newOrderInfo.Find("Head").GetComponent<Image>();
    }

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        StartCoroutine(UpdateDisplayScore());
    }

    /// <summary>
    /// Plays the countdown animation.
    /// </summary>
    public void PlayCountdown()
    {
        countdown.SetActive(true);
    }

    /// <summary>
    /// Starts the windows around the game.
    /// </summary>
    public void StartWindows()
    {
        patternAnimator.SetTrigger("Start");
        scoreAnimator.SetTrigger("Start");
        timeLeftAnimator.SetTrigger("Start");
    }

    /// <summary>
    /// Updates the pattern being shown.
    /// </summary>
    public void UpdateShownPattern(Pattern pattern)
    {
        patternAnimator.SetTrigger("New");
        patternWindowLegs.sprite = pattern.legsPart.fixedSprite;
        patternWindowBody.sprite = pattern.bodyPart.fixedSprite;
        patternWindowHead.sprite = pattern.headPart.fixedSprite;

        newOrderWindow.SetActive(true);
        newOrderWindowLegs.sprite = pattern.legsPart.fixedSprite;
        newOrderWindowBody.sprite = pattern.bodyPart.fixedSprite;
        newOrderWindowHead.sprite = pattern.headPart.fixedSprite;
    }

    /// <summary>
    /// Resets the checkmarks in patterns.
    /// </summary>
    public void ResetPatternCheckmarks()
    {
        patternWindowLegs.transform.GetChild(0).GetComponent<Image>().enabled = false;
        patternWindowBody.transform.GetChild(0).GetComponent<Image>().enabled = false;
        patternWindowHead.transform.GetChild(0).GetComponent<Image>().enabled = false;
    }

    /// <summary>
    /// Shows the checkmark on the head part.
    /// </summary>
    public void CheckmarkHeadPart()
    {
        patternWindowHead.transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    /// <summary>
    /// Shows the checkmark on the body part.
    /// </summary>
    public void CheckmarkBodyPart()
    {
        patternWindowBody.transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    /// <summary>
    /// Shows the checkmark on the legs part.
    /// </summary>
    public void CheckmarkLegsPart()
    {
        patternWindowLegs.transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    /// <summary>
    /// Updates the time of the pattern, by changing how the clock looks.
    /// </summary>
    public void UpdatePatternTime(int time, int maxTime)
    {
        patternTimerRadial.fillAmount = (float)((float)time / (float)maxTime);
        patternTimerText.text = time.ToString();

        if(time < 6 && time > 0)
        {
            patternTimerAnimator.SetTrigger("Warning");
            timeLeftWarning.SetTrigger("Warning");
        }
    }

    /// <summary>
    /// Shows the clock recover animation.
    /// </summary>
    public void RecoverClock()
    {
        patternTimerAnimator.SetTrigger("Recover");
    }

    /// <summary>
    /// Shakes the clock with the shaking animation.
    /// </summary>
    public void ShakeClock()
    {
        patternTimerAnimator.SetTrigger("Shake");
    }

    /// <summary>
    /// Updates the overall Game Clock.
    /// </summary>
    public void UpdateGameTime(int time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string timeString = timeSpan.ToString(@"mm\:ss");

        timeLeftText.text = timeString;

        if(time % 60 == 0 || time < 11)
        {
            timeLeftAnimator.SetTrigger("Warning");
        }
    }

    /// <summary>
    /// Updates the score visually.
    /// </summary>
    public IEnumerator UpdateDisplayScore()
    {
        bool newHighscoreAnimationDone = false;
        bool removeHighscoreAnimationDone = true;

        while (true)
        {
            if (GameManager.Instance.previousHighScore < GameManager.Instance.score)
            {
                highscoreText.text = "New Highscore!";

                if (!newHighscoreAnimationDone)
                {
                    highscoreText.GetComponent<Animator>().SetTrigger("Bump");
                    highscoreText.ForceMeshUpdate();

                    removeHighscoreAnimationDone = false;
                    newHighscoreAnimationDone = true;
                }
            }
            else
            {
                highscoreText.text = "<size=56>Highscore:</size> " + GameManager.Instance.previousHighScore;

                if (!removeHighscoreAnimationDone)
                {
                    highscoreText.GetComponent<Animator>().SetTrigger("Remove");
                    highscoreText.ForceMeshUpdate();

                    removeHighscoreAnimationDone = true;
                    newHighscoreAnimationDone = false;
                }
            }

            if (displayScore < GameManager.Instance.score)
            {
                displayScore++;
                scoreText.text = String.Format("{0:n0}", displayScore);

                yield return new WaitForSecondsRealtime(0.005f);
            }
            else if (displayScore > GameManager.Instance.score)
            {
                displayScore--;
                scoreText.text = String.Format("{0:n0}", displayScore);

                yield return new WaitForSecondsRealtime(0.005f);
            }

            yield return null;
        }
    }

    /// <summary>
    /// Shakes the score window when score is obtained.
    /// </summary>
    public void ShakeScoreAdd()
    {
        scoreAnimator.SetTrigger("Add");
    }

    /// <summary>
    /// Shakes the score window when score is removed.
    /// </summary>
    public void ShakeScoreRemove()
    {
        scoreAnimator.SetTrigger("Remove");
    }

    /// <summary>
    /// Shows the Game Over Screen.
    /// </summary>
    public void ShowGameOverScreen(bool isNewHighscore)
    {
        FindObjectOfType<GameOverCanvas>().ShowGameOverScreen(isNewHighscore);
    }
}
