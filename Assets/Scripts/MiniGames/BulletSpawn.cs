using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab; // Prefab of the bullet to spawn
    public float bulletSpeed = 5f;  // Speed of the bullet
    public float fireRate = 1f;    // Time interval between shots

    private Coroutine spawnCoroutine;

    private void OnEnable()
    {
        StartSpawning();
    }

    private void OnDisable()
    {
        StopSpawning();
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnBullets());
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnBullets()
    {
        while (true)
        {
            SpawnBullet();
            yield return new WaitForSecondsRealtime(fireRate); // Use real-time delays for consistency
        }
    }

    private void SpawnBullet()
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = transform.up * bulletSpeed; // Moves forward in the direction the spawner is facing
            }
        }
    }
}
