using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SkibidiRizzler is a <see cref="PromptType"/> <see cref="Minigame"/> 
/// that prompts the player to type the most brainrot sentence they 
/// can imagine
/// </summary>
public class SkibidiRizzler : PromptType
{
    [TextArea(3, 10)]
    [SerializeField] string brainrotPrompt;
    
    public override void StartMinigame(MinigameCaller caller)
    {
        base.StartMinigame(caller);
        StopAllCoroutines();
        StartCoroutine(Timer());
    }

    /// <summary>
    /// No new prompts are needed in this minigame.
    /// </summary>
    protected override void NewPrompt()
    {
        promptArea.text = brainrotPrompt;
    }

    /// <summary>
    /// Ends the minigame after a duration.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/></returns>
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        caller.CompleteMinigame(MinigameStatus.LOST);
        MinigameResultUI.instance.ShowResult(MinigameStatus.LOST);
        EndMinigame();
    }

    public override void EndMinigame()
    {
        StopAllCoroutines();
        timerUIElement.StopTimer();
        promptArea.gameObject.SetActive(false);
        inputArea.DeactivateInput();
        InputParser.instance.ActivateInput();
    }

    /// <summary>
    /// Splits the single token into multiple.
    /// </summary>
    /// <param name="token">The text input from the player</param>
    public override void ReadTextInput(string token)
    {
        ReadTextInput(token.Split(' '));
    }

    /// <summary>
    /// Checks if any of the tokens are in the dictionary.
    /// </summary>
    /// <param name="tokens">The text input from the player.</param>
    public override void ReadTextInput(string[] tokens)
    {
        foreach(string token in tokens)
        {
            if (prompts.Contains(token.ToLower()))
            {
                caller.CompleteMinigame(MinigameStatus.WIN);
                MinigameResultUI.instance.ShowResult(MinigameStatus.FISTBUMP);
                EndMinigame();
            }
        }
    }
}
