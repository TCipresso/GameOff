using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMash : Minigame
{
    [SerializeField] float duration = 10f;
    [SerializeField] float passPoints = 100f;
    [SerializeField] float currPoints = 0f;
    [SerializeField] float pointsPerClick = 10f;
    [SerializeField] float timeBetweenDecay = .2f;
    [SerializeField] float pointsLostPerDecay = 1f;
    [SerializeField] GameObject button;

    public override void StartMinigame(MinigameCaller caller)
    {
        StopAllCoroutines();
        currPoints = 0f;
        base.StartMinigame(caller);
        button.SetActive(true);
        StartCoroutine(Timer());
        StartCoroutine(Decay());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        EndMinigame();
    }

    IEnumerator Decay()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenDecay);
            LosePoints(pointsLostPerDecay);
        }
    }

    public void LosePoints(float points)
    {
        if (currPoints <= 0) return;
        
        currPoints -= points;
        if (currPoints < 0) currPoints = 0;
    }

    public void EarnPoints()
    {
        EarnPoints(pointsPerClick);
    }

    public void EarnPoints(float points)
    {
        currPoints += points;
    }

    public override void EndMinigame()
    {
        button.SetActive(false);
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
