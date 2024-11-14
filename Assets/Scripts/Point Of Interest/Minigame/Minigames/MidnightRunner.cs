using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidnightRunner : Minigame
{
    [SerializeField] ObstacleSpawner spawner;
    [SerializeField] GameObject runner;
    [SerializeField] float gameDuration;

    public override void StartMinigame(MinigameCaller caller)
    {
        base.StartMinigame(caller);
        runner.transform.localPosition = new Vector2(0, -250); //I did not want to mess with localPosition and position. This is going to bite someone in the ass.
        runner.SetActive(true);
        spawner.reporter = this;
        spawner.SpawnObstacle();
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(gameDuration);
        EndMinigame();
        caller.CompleteMinigame(MinigameStatus.WIN);
    }

    public override void Report()
    {
        StopAllCoroutines();
        EndMinigame();
        caller.CompleteMinigame(MinigameStatus.LOST);
    }

    public override void EndMinigame()
    {
        runner.SetActive(false);
        spawner.StopSpawning();
    }
}
