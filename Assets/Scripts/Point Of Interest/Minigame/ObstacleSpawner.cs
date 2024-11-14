using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ObstacleSpawner is an abstract class to spawn obstacles during
/// minigames. Implements <see cref="ReportHit"/>
/// </summary>
public abstract class ObstacleSpawner : MonoBehaviour, ReportHit
{
    public ReportHit reporter { protected get; set; }

    /// <summary>
    /// Tell reporter that one of my <see cref="Obstacle"/>s hit something.
    /// </summary>
    public abstract void ReportObstacleHit();

    /// <summary>
    /// Start the <see cref="Obstacle"/> spawning pattern.
    /// </summary>
    public abstract void SpawnObstacle();

    /// <summary>
    /// Stop the <see cref="Obstacle"/> spawning pattern.
    /// </summary>
    public abstract void StopSpawning();
}
