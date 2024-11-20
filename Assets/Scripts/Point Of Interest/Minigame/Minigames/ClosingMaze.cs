using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingMaze : Minigame
{
    [SerializeField] ObstacleSpawner obstacleSpawner;
    [SerializeField] GameObject runner;
    [SerializeField] Collider2D endTrigger;

    public override void StartMinigame(MinigameCaller caller)
    {
        StopAllCoroutines();
        base.StartMinigame(caller);

        runner.transform.localPosition = new Vector2(-275, 0);
        runner.SetActive(true);
        if (runner.TryGetComponent<MoveableObject>(out MoveableObject movableRunner))
        {
            MovementInput.instance.SetPuppet(movableRunner);
        }
        else Debug.LogError($"{runner.name} is not a moveable object!");

        obstacleSpawner.reporter = this;
        obstacleSpawner.SpawnObstacle();

        endTrigger.gameObject.SetActive(true);
    }

    public void WinMinigame()
    {
        EndMinigame();
        if (showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.WIN);
        caller.CompleteMinigame(MinigameStatus.WIN);
    }

    public override void EndMinigame()
    {
        base.EndMinigame();
        StopAllCoroutines();
        obstacleSpawner.StopSpawning();
        runner.SetActive(false);
        endTrigger.gameObject.SetActive(false);
    }

    public override void ReportObstacleHit()
    {
        EndMinigame();
        if(showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.LOST);
        caller.CompleteMinigame(MinigameStatus.LOST);
    }
}
