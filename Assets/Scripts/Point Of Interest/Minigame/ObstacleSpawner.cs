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
    public abstract void Report();
    public abstract void SpawnObstacle();
    public abstract void StopSpawning();
}
