using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] List<Minigame> minigames = new List<Minigame>();
    public static MinigameManager instance;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
    }

    public void PlayMinigame(int index, MinigameCaller caller)
    {
        if (index < 0) {
            Debug.LogError($"{name}: index < 0");
            return;
        }
        if(index >= minigames.Count) {
            Debug.LogError($"{name}: index is out of bounds. Either caller is using an incorrect index or we are missing a game!");
            return;
        }
        minigames[index].StartMinigame(caller);
    }

}
