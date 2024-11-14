using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleSpawner : MonoBehaviour
{
    //[SerializeField] Obstacle obstacle;
    public abstract void SpawnObstacle();
    public abstract void StopSpawning();
}
