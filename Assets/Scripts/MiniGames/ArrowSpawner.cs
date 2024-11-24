using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject arrowsContainer;
    private float spawnDelay; 

    private float timer = 0f;

    private void Start()
    {
        spawnDelay = Random.Range(1f, 3f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnDelay)
        {
            SpawnArrow();
            timer = 0;
            spawnDelay = Random.Range(2f, 3.5f);
        }
    }

    void SpawnArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowsContainer.transform);
    }
}
