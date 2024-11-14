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
            obstacle.transform.position = new Vector2(obstacle.transform.position.x, transform.position.y);
            if(obstacle.TryGetComponent<Obstacle>(out Obstacle cur))
            {
                cur.reporter = this;
            }
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
            obstacles[i].transform.position = new Vector2(obstacles[i].transform.position.x, transform.position.y);
            obstacles[i].SetActive(true);
        }
    }

    public override void StopSpawning()
    {
        StopAllCoroutines();
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.transform.position = new Vector2(obstacle.transform.position.x, transform.position.y);
            obstacle.SetActive(false);
        }
    }

    public override void Report()
    {
        reporter.Report();
    }
}
