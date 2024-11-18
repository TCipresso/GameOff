using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

/// <summary>
/// PromptType is a <see cref="Minigame"/> that takes typed input
/// from the player. It can also be used as a base class for 
/// future prompt type minigames.
/// </summary>
public class PromptType : Minigame
{
    [Header("Game Statistics")]
    [SerializeField] protected float duration = 10f;
    [SerializeField] protected int curPoints = 0;
    [SerializeField] protected int winPoints = 3;
    [SerializeField] protected List<string> prompts = new List<string>();
    protected string curPrompt = string.Empty;
    protected int curPromptIndex = 0;

    [Header("UI Elements")]
    [SerializeField] protected TextMeshProUGUI promptArea;
    [SerializeField] protected MinigameTextInput inputArea;
    

    public override void StartMinigame(MinigameCaller caller)
    {
        StopAllCoroutines();
        base.StartMinigame(caller);
        curPoints = 0;

        promptArea.gameObject.SetActive(true);
        promptArea.text = "";
        InputParser.instance.DeactivateInput();
        inputArea.promptMinigame = this;
        inputArea.ActivateInput();

        NewPrompt();
        timerUIElement.StartTimer(duration);
        StartCoroutine(Timer());
    }

    /// <summary>
    /// Gets a new prompt from the list and presents it to the player.
    /// Tries to avoid duplicate prompts.
    /// </summary>
    protected virtual void NewPrompt()
    {
        int prev = curPromptIndex;
        curPromptIndex = Random.Range(0, prompts.Count);
        if(curPromptIndex == prev) curPromptIndex = (curPromptIndex + 1) % prompts.Count;
        
        curPrompt = prompts[curPromptIndex];
        promptArea.text = curPrompt;
    }

    /// <summary>
    /// Ends the game after a duration.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/></returns>
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        EndMinigame();
    }

    public override void EndMinigame()
    {
        StopAllCoroutines();
        timerUIElement.StopTimer();

        promptArea.text = "";
        promptArea.gameObject.SetActive(false);
        inputArea.DeactivateInput();
        InputParser.instance.ActivateInput();

        if (curPoints >= winPoints) {
            caller.CompleteMinigame(MinigameStatus.WIN);
            if (showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.WIN);
        }
        else {
            caller.CompleteMinigame(MinigameStatus.LOST);
            if (showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.LOST);
        }
    }

    public override void ReportObstacleHit()
    {
        throw new System.NotImplementedException("No obstacles in this minigame!");
    }

    /// <summary>
    /// Checks if the token is equal to the prompt. 
    /// Gain a point and get a new prompt if it is.
    /// </summary>
    /// <param name="token">The player's text input.</param>
    public virtual void ReadTextInput(string token)
    {
        if (!string.IsNullOrEmpty(token) && curPrompt.Equals(token.Trim()))
        {
            curPoints++;
            NewPrompt();
        }
    }

    /// <summary>
    /// Makes all tokens into a single string
    /// and then calls <see cref="ReadTextInput(string)"/>
    /// </summary>
    /// <param name="tokens">An array of the player's text input.</param>
    public virtual void ReadTextInput(string[] tokens)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string token in tokens)
        {
            stringBuilder.Append(token);
            stringBuilder.Append(" ");
        }
        ReadTextInput(stringBuilder.ToString());
    }
}
