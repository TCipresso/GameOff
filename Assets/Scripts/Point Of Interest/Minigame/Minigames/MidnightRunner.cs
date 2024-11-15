using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MidnightRunner is a <see cref="Minigame"/> where the player has 
/// to avoid falling <see cref="Obstacle"/>s.
/// </summary>
public class MidnightRunner : Minigame
{
    [SerializeField] ObstacleSpawner spawner;
    [SerializeField] GameObject runner;
    [SerializeField] float gameDuration;


    public override void StartMinigame(MinigameCaller caller)
    {
        base.StartMinigame(caller);
        StopAllCoroutines();
        runner.transform.localPosition = new Vector2(0, -250); //I did not want to mess with localPosition and position. This is going to bite someone in the ass.
        runner.SetActive(true);

        if (runner.TryGetComponent<MoveableObject>(out MoveableObject movableRunner))
            MovementInput.instance.SetPuppet(movableRunner);
        else
            Debug.LogWarning("Runner is not a MoveableObject");

        spawner.reporter = this;
        spawner.SpawnObstacle();
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(gameDuration);
        EndMinigame();
        if (showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.WIN);
        caller?.CompleteMinigame(MinigameStatus.WIN); // Check for null caller
    }

    public override void ReportObstacleHit()
    {
        StopAllCoroutines();
        EndMinigame();
        if (showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.LOST);
        caller?.CompleteMinigame(MinigameStatus.LOST); // Check for null caller
    }

    public override void EndMinigame()
    {
        runner.SetActive(false);
        spawner.StopSpawning();
    }
}
