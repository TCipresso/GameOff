using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// TimerUI is a UI Element representing a timer.
/// </summary>
public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textElement;
    private float duration = 0f;

    /// <summary>
    /// Start the timer.
    /// </summary>
    /// <param name="duration">Amount of time to show</param>
    public void StartTimer(float duration)
    {
        ShowTimer();
        this.duration = duration;
        StartCoroutine(Timer());
    }

    /// <summary>
    /// Show the Timer's text element.
    /// </summary>
    public void ShowTimer()
    {
        textElement.gameObject.SetActive(true);
    }

    /// <summary>
    /// Count down.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/></returns>
    IEnumerator Timer()
    {
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
            if (duration < 0) duration = 0;
            UpdateTimer();
        }
        StopTimer();
    }

    /// <summary>
    /// Update the timer's text element.
    /// </summary>
    private void UpdateTimer()
    {
        textElement.text = $"{duration.ToString("F3")}";
    }

    /// <summary>
    /// Stop the timer and hide it.
    /// </summary>
    /// <param name="hideTimer">Hide the timer after stoping it.</param>
    public void StopTimer(bool hideTimer = true)
    {
        StopAllCoroutines();
        if (hideTimer) HideTimer();
    }

    /// <summary>
    /// Hide the Timer's text element.
    /// </summary>
    public void HideTimer()
    {
        textElement.gameObject.SetActive(false);
    }
    
}
