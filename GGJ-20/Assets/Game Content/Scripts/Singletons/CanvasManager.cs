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
    private GameObject patternWindow;

    // Private
    private GameObject patternTimer;
    private Image patternTimerRadial;
    private TextMeshProUGUI patternTimerText;

    [Header("Time and Score")]
    [SerializeField]
    private TextMeshProUGUI scoreText = null;
    [SerializeField]
    private TextMeshProUGUI timeLeftText = null;

    private int displayScore = 0;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        StartCoroutine(UpdateDisplayScore());
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        patternTimer = patternWindow.transform.Find("Pattern Timer").gameObject;
        patternTimerRadial = patternTimer.transform.GetChild(0).GetComponent<Image>();
        patternTimerText = patternTimerRadial.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Updates the pattern being shown.
    /// </summary>
    public void UpdateShownPattern(Pattern pattern)
    {
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
    }

    /// <summary>
    /// Updates the overall Game Clock.
    /// </summary>
    public void UpdateGameTime(int time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string timeString = timeSpan.ToString(@"mm\:ss");

        timeLeftText.text = timeString;
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

                yield return new WaitForSeconds(0.005f);
            }

            yield return null;
        }
    }
}
