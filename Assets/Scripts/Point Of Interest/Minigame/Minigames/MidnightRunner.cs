using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidnightRunner : Minigame
{
    [SerializeField] ObstacleSpawner spawner;
    [SerializeField] GameObject runner;
    public override void StartMinigame(MinigameCaller caller)
    {
        base.StartMinigame(caller);
        runner.SetActive(true);
        spawner.SpawnObstacle();
    }

    public override void EndMinigame()
    {
        runner.SetActive(false);
        spawner.StopSpawning();
    }
}
