using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

/// <summary>
/// TypingTest is a <see cref="PromptType"/> <see cref="Minigame"/> 
/// where the player has to type in multiple words within a set time 
/// for multiple rounds.
/// </summary>
public class TypingTest : PromptType
{
    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] List<float> roundDurations = new List<float>();
    [SerializeField] List<int> wordsPerRound = new List<int>();
    private List<string> curRoundWords = new List<string>();

    public override void StartMinigame(MinigameCaller caller)
    {
        base.StartMinigame(caller);
        timerUIElement.StopTimer();
        StopAllCoroutines();
        progressText.text = $"{curPoints} / {winPoints}";
        NewPrompt();
    }

    /// <summary>
    /// Starts a new round.
    /// </summary>
    protected override void NewPrompt()
    {
        StopAllCoroutines();
        timerUIElement.StopTimer();

        int words = wordsPerRound[curPoints < wordsPerRound.Count ? curPoints : (wordsPerRound.Count - 1)];
        
        curRoundWords.Clear();
        for(int i = 0; i < words; i++)
        {
            curRoundWords.Add(prompts[Random.Range(0, prompts.Count)]);
        }

        StringBuilder showPrompt = new StringBuilder();
        for(int i = 0; i < curRoundWords.Count; i++)
        {
            showPrompt.Append(curRoundWords[i]);
            showPrompt.Append(" ");
        }
        promptArea.text = showPrompt.ToString();

        timerUIElement.StartTimer(roundDurations[curPoints < roundDurations.Count ? curPoints : (roundDurations.Count - 1)]);
        StartCoroutine(Timer(roundDurations[curPoints < roundDurations.Count ? curPoints : (roundDurations.Count - 1)]));
    }

    /// <summary>
    /// Ends the minigame after a duration.
    /// </summary>
    /// <param name="duration">Duration of the round.</param>
    /// <returns>An <see cref="IEnumerator"/></returns>
    IEnumerator Timer(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        EndMinigame();
    }

    /// <summary>
    /// Splits a single token into multiple.
    /// </summary>
    /// <param name="token">The player's text input.</param>
    public override void ReadTextInput(string token)
    {
        ReadTextInput(token.Split(' '));
    }

    /// <summary>
    /// Checks if the player typed all the prompted words correctly.
    /// If so, reward a point.
    /// If curPoints = winPoints, end the game.
    /// Else, start a new round.
    /// </summary>
    /// <param name="tokens">The player's text input.</param>
    public override void ReadTextInput(string[] tokens)
    {
        int j = 0;
        for(int i = 0; i < tokens.Length; i++)
        {
            if (string.IsNullOrEmpty(tokens[i])) continue;
            if (!tokens[i].Equals(curRoundWords[j])) break;
            j++;
        }

        if (j == curRoundWords.Count) { 
            curPoints++;
            progressText.text = $"{curPoints} / {winPoints}";
        }
        if (curPoints >= winPoints) EndMinigame();
        else NewPrompt();
    }
}
