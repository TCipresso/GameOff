using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// YapPat is a <see cref="PromptType"/> <see cref="Minigame"/>
/// where the player types x words within y time.
/// </summary>
public class YapPat : PromptType
{
    private List<string> playerTokens = new List<string>();

    public override void StartMinigame(MinigameCaller caller)
    {
        base.StartMinigame(caller);
        StartCoroutine(CountWords());
    }

    /// <summary>
    /// Shows the current count to the player.
    /// </summary>
    protected override void NewPrompt()
    {
        promptArea.text = $"{curPoints} / {winPoints}";
    }

    /// <summary>
    /// Splits a single token into an array of tokens.
    /// </summary>
    /// <param name="token">The player's text input.</param>
    public override void ReadTextInput(string token)
    {
        ReadTextInput(token.Split(' '));
    }

    /// <summary>
    /// Adds every token into a string list.
    /// </summary>
    /// <param name="tokens">The player's text input.</param>
    public override void ReadTextInput(string[] tokens)
    {
        foreach(string token in tokens)
        {
            if (!string.IsNullOrEmpty(token)) playerTokens.Add(token);
        }
    }

    /// <summary>
    /// Counts the number of words in the list and empties it.
    /// Shows the current count to the player.
    /// Ends the game once enough words were entered.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/></returns>
    IEnumerator CountWords()
    {
        while(curPoints < winPoints)
        {
            yield return null;
            Debug.Log("Counting");
            if (playerTokens.Count == 0) continue;

            playerTokens.RemoveAt(0);
            curPoints++;
            promptArea.text = $"{curPoints} / {winPoints}";
        }
        EndMinigame();
    }

    public override void EndMinigame()
    {
        base.EndMinigame();
        playerTokens.Clear();
    }
}
