using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ButtonMash is a <see cref="Minigame"/> where the player has to
/// mash a button quickly.
/// </summary>
public class ButtonMash : Minigame
{
    [Header("Game Statistics")]
    [SerializeField] float duration = 10f;
    [SerializeField] float passPoints = 100f;
    [SerializeField] float currPoints = 0f;
    [SerializeField] float pointsPerClick = 10f;
    [SerializeField] float timeBetweenDecay = .2f;
    [SerializeField] float pointsLostPerDecay = 1f;

    [Header("UI Elements")]
    [SerializeField] GameObject button;
    [SerializeField] Transform progressObject;

    public override void StartMinigame(MinigameCaller caller)
    {
        StopAllCoroutines();
        currPoints = 0f;
        base.StartMinigame(caller);
        
        button.SetActive(true);
        progressObject.gameObject.SetActive(true);
        progressObject.localScale = Vector2.zero;

        timerUIElement.StartTimer(duration);
        StartCoroutine(Timer());
        StartCoroutine(Decay());
    }

    /// <summary>
    /// Ends the game after a duration.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/></returns>
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        EndMinigame();
    }

    /// <summary>
    /// Removes a few points after every few decay periods.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/></returns>
    IEnumerator Decay()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenDecay);
            LosePoints(pointsLostPerDecay);
        }
    }

    /// <summary>
    /// Removes points from the player's total.
    /// </summary>
    /// <param name="points">Points to remove.</param>
    public void LosePoints(float points)
    {
        if (currPoints <= 0) return;

        currPoints -= points;
        if (currPoints < 0) currPoints = 0;
        ShowProgress();
    }

    /// <summary>
    /// Changes the scale of the progressObjects based
    /// on the ratio of currpoints:passPoints * 1.25
    /// </summary>
    private void ShowProgress()
    {
        float percentage = currPoints / (passPoints * 1.25f);
        if (percentage > 1) percentage = 1;
        progressObject.localScale = new Vector2(percentage, percentage);
    }

    /// <summary>
    /// Add points to the player's total.
    /// Number of points defined by <see cref="ButtonMash"/>
    /// </summary>
    public void EarnPoints()
    {
        EarnPoints(pointsPerClick);
    }

    /// <summary>
    /// Add points to the player's total.
    /// </summary>
    /// <param name="points">Number of points to add.</param>
    public void EarnPoints(float points)
    {
        currPoints += points;
        ShowProgress();
    }

    public override void EndMinigame()
    {
        base.EndMinigame();
        button.SetActive(false);
        progressObject.localScale = Vector2.zero;
        progressObject.gameObject.SetActive(false);

        timerUIElement.StopTimer();
        StopAllCoroutines();
        
        if (currPoints >= passPoints) {
            caller.CompleteMinigame(MinigameStatus.WIN);
            if (showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.WIN);
        }
        else {
            caller.CompleteMinigame(MinigameStatus.LOST);
            if (showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.LOST);
        }
        currPoints = 0f;
    }

    public override void ReportObstacleHit()
    {
        throw new System.NotImplementedException("No obstacles in this minigame!");
    }
}
