using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PonyIsland is a <see cref="Minigame"/> where the 
/// player has to jump over fences.
/// </summary>
public class PonyIsland : Minigame
{
    [SerializeField] float duration;
    [SerializeField] MoveableObject runner;
    [SerializeField] ObstacleSpawner spawner;
    [SerializeField] Collider2D jumpTrigger;
    [SerializeField] Collider2D floor;

    public override void StartMinigame(MinigameCaller caller)
    {
        base.StartMinigame(caller);

        runner.transform.localPosition = new Vector2(-175, -150);
        runner.gameObject.SetActive(true);
        MovementInput.instance.SetPuppet(runner);

        spawner.reporter = this;
        spawner.SpawnObstacle();

        floor.gameObject.SetActive(true);
        jumpTrigger.gameObject.SetActive(true);

        timerUIElement.StartTimer(duration);
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        EndMinigame();
        if(showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.WIN);
        caller.CompleteMinigame(MinigameStatus.WIN);
    }

    public override void EndMinigame()
    {
        base.EndMinigame();
        timerUIElement.StopTimer();
        StopAllCoroutines();
        spawner.StopSpawning();
        
        runner.gameObject.SetActive(false);
        floor.gameObject.SetActive(false);
        jumpTrigger.gameObject.SetActive(false);
    }

    public override void ReportObstacleHit()
    {
        EndMinigame();
        if (showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.LOST);
        caller.CompleteMinigame(MinigameStatus.LOST);
    }
}
