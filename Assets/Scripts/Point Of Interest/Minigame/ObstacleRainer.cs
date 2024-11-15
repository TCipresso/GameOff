using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ObstacleRainer is an <see cref="ObstacleSpawner"/> that spawns a 
/// line of staggered <see cref="Obstacle"/>s.
/// </summary>
public class ObstacleRainer : ObstacleSpawner
{
    [SerializeField] List<GameObject> obstacles;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] bool vertical = true;

    public override void SpawnObstacle()
    {
        foreach(GameObject obstacle in obstacles) { 
            if (vertical) obstacle.transform.position = new Vector2(obstacle.transform.position.x, transform.position.y);
            else obstacle.transform.position = new Vector2(transform.position.x, obstacle.transform.position.y);

            if (obstacle.TryGetComponent<Obstacle>(out Obstacle cur))
            {
                cur.reporter = this;
            }
            obstacle.SetActive(false);
        }
        StartCoroutine(SpawnCoroutine());
    }

    /// <summary>
    /// Choose a random inactive <see cref="Obstacle"/> to drop.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/></returns>
    IEnumerator SpawnCoroutine()
    {
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

            if (vertical) obstacles[i].transform.position = new Vector2(obstacles[i].transform.position.x, transform.position.y);
            else obstacles[i].transform.position = new Vector2(transform.position.x, obstacles[i].transform.position.y);
            obstacles[i].SetActive(true);
        }
    }

    public override void StopSpawning()
    {
        StopAllCoroutines();
        foreach (GameObject obstacle in obstacles)
        {
            if (vertical) obstacle.transform.position = new Vector2(obstacle.transform.position.x, transform.position.y);
            else obstacle.transform.position = new Vector2(transform.position.x, obstacle.transform.position.y);
            obstacle.SetActive(false);
        }
    }

    public override void ReportObstacleHit()
    {
        reporter.ReportObstacleHit();
    }
}
