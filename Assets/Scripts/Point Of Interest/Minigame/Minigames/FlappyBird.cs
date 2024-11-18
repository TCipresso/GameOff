using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FlappyBird is a <see cref="Minigame"/> that tries its best
/// to be like the original game
/// </summary>
public class FlappyBird : Minigame
{
    [SerializeField] float duration = 5f;
    [SerializeField] GameObject bird;
    [SerializeField] ObstacleSpawner spawner;

    public override void StartMinigame(MinigameCaller caller)
    {
        base.StartMinigame(caller);
        StopAllCoroutines();
        bird.transform.localPosition = new Vector2(-250, 0);
        bird.SetActive(true);
        if(bird.TryGetComponent<MoveableObject>(out MoveableObject moveableBird))
            MovementInput.instance.SetPuppet(moveableBird);
        else
            Debug.Log($"{name}: Bird does not have a moveableObject componenet");

        spawner.reporter = this;
        spawner.SpawnObstacle();
        timerUIElement.StartTimer(duration);
        StartCoroutine(Timer());
    }

    /// <summary>
    /// Ends the minigame after a duration.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/></returns>
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
        timerUIElement.StopTimer();
        StopAllCoroutines();
        bird.SetActive(false);
        spawner.StopSpawning();
    }
}
