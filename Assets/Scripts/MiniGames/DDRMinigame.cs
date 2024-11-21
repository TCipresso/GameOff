using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDRMinigame : MonoBehaviour
{
    public float gameDuration = 10f;
    private List<KeyCode> currentSequence;
    private int sequenceIndex;
    private int currentRound = 0;
    private float timer;
    private bool gameActive;

    void OnEnable()
    {
        StartGame();
    }

    void Update()
    {
        if (!gameActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            EndGame();
            return;
        }

        if (Input.anyKeyDown)
        {
            CheckInput();
        }
    }

    void StartGame()
    {
        timer = gameDuration;
        gameActive = true;
        currentRound = 0;
        StartNextRound();
    }

    void StartNextRound()
    {
        currentRound++;
        GenerateSequence(currentRound + 3); // Start with 4 arrows and increase
        sequenceIndex = 0;
    }

    void GenerateSequence(int length)
    {
        currentSequence = new List<KeyCode>();
        KeyCode[] possibleKeys = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
        for (int i = 0; i < length; i++)
        {
            KeyCode randomKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
            currentSequence.Add(randomKey);
            Debug.Log(randomKey); // Log or show this to the player somehow
        }
    }

    void CheckInput()
    {
        foreach (KeyCode key in currentSequence)
        {
            if (Input.GetKeyDown(key))
            {
                sequenceIndex++;
                if (sequenceIndex >= currentSequence.Count)
                {
                    StartNextRound(); // Correct sequence, go to next round
                }
                return; // Exit the loop and wait for the next input
            }
        }

        // If no correct input, restart the round
        StartNextRound();
    }

    void EndGame()
    {
        gameActive = false;
        Debug.Log("Game Over. Total Rounds Cleared: " + (currentRound - 1));
        Combat.instance.MiniGameCompleted(); // Inform the combat system
    }
}
