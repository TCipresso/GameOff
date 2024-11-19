using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ObstacleClosingMaze is a <see cref="ObstacleSpawner"/> that spawns 
/// a maze and a wall that closes in on the player.
/// </summary>
public class ObstacleClosingMaze : ObstacleSpawner
{
    [SerializeField] Obstacle wall;
    [SerializeField] Vector2 wallStartingPosition = Vector2.zero;
    [SerializeField] List<Obstacle> mazes = new List<Obstacle>();
    
    public override void ReportObstacleHit()
    {
        reporter.ReportObstacleHit();
    }

    public override void SpawnObstacle()
    {
        foreach(Obstacle maze in mazes)
        {
            maze.reporter = this;
            maze.gameObject.SetActive(false);
        }
        wall.reporter = this;
        wall.gameObject.transform.localPosition = wallStartingPosition;
        mazes[Random.Range(0, mazes.Count)].gameObject.SetActive(true);
        wall.gameObject.SetActive(true);
    }

    public override void StopSpawning()
    {
        foreach (Obstacle maze in mazes)
        {
            if (!maze.gameObject.activeSelf) continue;
            maze.gameObject.SetActive(false);
        }
        wall.gameObject.transform.localPosition = wallStartingPosition;
        wall.gameObject.SetActive(false);
    }
}
