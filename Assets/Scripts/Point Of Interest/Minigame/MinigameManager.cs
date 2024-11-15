using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MinigameManager is a class that aggregates <see cref="Minigame"/>
/// </summary>
public class MinigameManager : MonoBehaviour
{
    [SerializeField] public List<Minigame> minigames = new List<Minigame>();
    public static MinigameManager instance;

    /// <summary>
    /// Singleton
    /// </summary>
    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
    }

    /// <summary>
    /// Play the <see cref="Minigame"/> stored at index.
    /// </summary>
    /// <param name="index">Index of the <see cref="Minigame"/></param>
    /// <param name="caller">The <see cref="MinigameCaller"/> who wants to start the game</param>
    public void PlayMinigame(int index, MinigameCaller caller)
    {
        if (index < 0)
        {
            Debug.LogError($"{name}: index < 0");
            return;
        }
        if (index >= minigames.Count)
        {
            Debug.LogError($"{name}: index is out of bounds. Either caller is using an incorrect index or we are missing a game!");
            return;
        }

        Debug.Log($"Playing minigame at index {index}: {minigames[index].name}");

        minigames[index].StartMinigame(caller);
    }
}
