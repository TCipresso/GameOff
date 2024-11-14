using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRainer : ObstacleSpawner
{
    [SerializeField] List<GameObject> obstacles;
    [SerializeField] float timeBetweenSpawns;

    public override void SpawnObstacle()
    {
        foreach(GameObject obstacle in obstacles) { 
            obstacle.transform.position = new Vector2(obstacle.transform.position.x, 0); //This is assuming the obstacles are children of the spawner.
            obstacle.SetActive(false);
        }
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        Random.InitState(9999);
        for (int i = Random.Range(0, obstacles.Count); i < obstacles.Count; i = Random.Range(0, obstacles.Count))
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            if (obstacles[i].activeSelf)
            {
                int start = i;
                do
                {
                    i = ++i % obstacles.Count;
                }
                while (i != start && obstacles[i].activeSelf);
            }
            if (obstacles[i].activeSelf) continue;
            obstacles[i].transform.position = new Vector2(obstacles[i].transform.position.x, 0);
            obstacles[i].SetActive(true);
        }
    }

    public override void StopSpawning()
    {
        StopCoroutine(SpawnCoroutine());
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.transform.position = new Vector2(obstacle.transform.position.x, 0);
        }
    }
}
