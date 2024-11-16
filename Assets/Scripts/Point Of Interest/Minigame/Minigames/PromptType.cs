using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class PromptType : Minigame
{
    [SerializeField] float duration = 10f;
    [SerializeField] int curPoints = 0;
    [SerializeField] int winPoints = 3;
    [SerializeField] TextMeshProUGUI promptArea;
    [SerializeField] MinigameTextInput inputArea;
    [SerializeField] List<string> prompts = new List<string>();
    private string curPrompt = string.Empty;
    private int curPromptIndex = 0;

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
        StartCoroutine(Timer());
    }

    private void NewPrompt()
    {
        int prev = curPromptIndex;
        curPromptIndex = Random.Range(0, prompts.Count);
        if(curPromptIndex == prev) curPromptIndex = (curPromptIndex + 1) % prompts.Count;
        
        curPrompt = prompts[curPromptIndex];
        promptArea.text = curPrompt;
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        EndMinigame();
    }

    public override void EndMinigame()
    {
        StopAllCoroutines();
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

    public virtual void ReadTextInput(string token)
    {
        if (!string.IsNullOrEmpty(token) && curPrompt.Equals(token.Trim()))
        {
            curPoints++;
            NewPrompt();
        }
    }

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
