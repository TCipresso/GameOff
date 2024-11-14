using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleSpawner : MonoBehaviour, ReportHit
{
    public ReportHit reporter { protected get; set; }
    public abstract void ReportHit();
    public abstract void SpawnObstacle();
    public abstract void StopSpawning();
}
