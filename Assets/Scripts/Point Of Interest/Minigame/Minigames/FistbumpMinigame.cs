using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FistbumpMinigame is a minigame where the player 
/// simply clicks the fist to complete a fistbump.
/// </summary>
public class FistbumpMinigame : Minigame
{
    [SerializeField] float duration = 5f;
    [SerializeField] GameObject button;

    public override void StartMinigame(MinigameCaller caller)
    {
        base.StartMinigame(caller);
        button.SetActive(true);
        timerUIElement.StartTimer(duration);
        StartCoroutine(Timer());
    }

    /// <summary>
    /// End the minigame after a duration.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/></returns>
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        caller.CompleteMinigame(MinigameStatus.LOST);
        if(showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.MISSBUMP);
        EndMinigame();
    }

    /// <summary>
    /// Do a fistbump.
    /// </summary>
    public void Fistbump()
    {
        caller.CompleteMinigame(MinigameStatus.WIN);
        if(showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.FISTBUMP);
        EndMinigame();
    }

    public override void EndMinigame()
    {
        base.EndMinigame();
        StopAllCoroutines();
        timerUIElement.StopTimer();
        button.SetActive(false);
    }

    public override void ReportObstacleHit()
    {
        throw new System.NotImplementedException("No obstacles in this minigame!");
    }
}
