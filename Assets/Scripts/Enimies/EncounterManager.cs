using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager instance;

    [SerializeField] private List<GameObject> enemyPrefabs; // List of enemy prefabs

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Get the enemy prefab at the specified index.
    /// </summary>
    /// <param name="index">Index of the enemy in the list.</param>
    /// <returns>The enemy prefab or null if out of range.</returns>
    public GameObject GetEnemyPrefab(int index)
    {
        if (index >= 0 && index < enemyPrefabs.Count)
        {
            return enemyPrefabs[index];
        }

        Debug.LogWarning($"No enemy prefab found at index {index}");
        return null;
    }
}
