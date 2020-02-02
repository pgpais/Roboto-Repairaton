using System.Collections;
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

    // Private
    private GameObject patternTimer;
    private Image patternTimerRadial;
    private TextMeshProUGUI patternTimerText;

    [Header("Time and Score")]
    [SerializeField]
    private GameObject scoreWindow = null;
    [SerializeField]
    private GameObject timeWindow = null;

    // Text
    private TextMeshProUGUI scoreText = null;
    private TextMeshProUGUI timeLeftText = null;

    // Animators
    private Animator patternTimerAnimator;
    private Animator patternAnimator;
    private Animator scoreAnimator;
    private Animator timeAnimator;

    private int displayScore = 0;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        scoreText = scoreWindow.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        timeLeftText = timeWindow.transform.Find("Text").GetComponent<TextMeshProUGUI>();

        patternAnimator = patternWindow.GetComponent<Animator>();
        scoreAnimator = scoreWindow.GetComponent<Animator>();
        timeAnimator = timeWindow.GetComponent<Animator>();

        patternTimer = patternWindow.transform.Find("Pattern Timer").gameObject;
        patternTimerAnimator = patternTimer.GetComponent<Animator>();
        patternTimerRadial = patternTimer.transform.GetChild(0).GetComponent<Image>();
        patternTimerText = patternTimerRadial.transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        StartCoroutine(UpdateDisplayScore());
    }

    /// <summary>
    /// Updates the pattern being shown.
    /// </summary>
    public void UpdateShownPattern(Pattern pattern)
    {
        patternAnimator.SetTrigger("New");

        patternWindow.transform.Find("Legs").GetComponent<Image>().sprite = pattern.legsPart.fixedSprite;
        patternWindow.transform.Find("Body").GetComponent<Image>().sprite = pattern.bodyPart.fixedSprite;
        patternWindow.transform.Find("Head").GetComponent<Image>().sprite = pattern.headPart.fixedSprite;
    }

    /// <summary>
    /// Updates the time of the pattern, by changing how the clock looks.
    /// </summary>
    public void UpdatePatternTime(int time, int maxTime)
    {
        patternTimerRadial.fillAmount = (float)((float)time / (float)maxTime);
        patternTimerText.text = time.ToString();

        if(time < 5 && time > 0)
        {
            patternTimerAnimator.SetTrigger("Warning");
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
            timeAnimator.SetTrigger("Warning");
        }
    }

    /// <summary>
    /// Updates the score visually.
    /// </summary>
    public IEnumerator UpdateDisplayScore()
    {
        while(true)
        {
            if(displayScore < GameManager.Instance.score)
            {
                displayScore++;
                scoreText.text = String.Format("{0:n0}", displayScore);

                yield return new WaitForSecondsRealtime(0.005f);
            }
            else if(displayScore > GameManager.Instance.score)
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
}
