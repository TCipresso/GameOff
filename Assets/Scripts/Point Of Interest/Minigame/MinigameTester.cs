using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTester : MonoBehaviour, MinigameCaller
{
    [SerializeField] Minigame test;
    public void CompleteMinigame(MinigameStatus gameResult)
    {
        Debug.Log(gameResult.ToString());
    }

    public void StartMinigame()
    {
        test.StartMinigame(this);
    }

}
