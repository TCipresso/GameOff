using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBird : Minigame
{
    [SerializeField] float duration = 5f;
    [SerializeField] GameObject bird;
    [SerializeField] ObstacleSpawner spawner;

    public override void StartMinigame(MinigameCaller caller)
    {
        base.StartMinigame(caller);
        StopAllCoroutines();
        bird.transform.localPosition = Vector3.zero;
        bird.SetActive(true);
        if(bird.TryGetComponent<MoveableObject>(out MoveableObject moveableBird))
            MovementInput.instance.SetPuppet(moveableBird);
        else
            Debug.Log($"{name}: Bird does not have a moveableObject componenet");

        spawner.reporter = this;
        spawner.SpawnObstacle();
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        EndMinigame();
        if (showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.WIN);
        caller?.CompleteMinigame(MinigameStatus.WIN);
    }

    public override void ReportObstacleHit()
    {
        EndMinigame();
        if (showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.LOST);
        caller?.CompleteMinigame(MinigameStatus.LOST);
    }

    public override void EndMinigame()
    {
        StopAllCoroutines();
        bird.SetActive(false);
        spawner.StopSpawning();
    }
}
