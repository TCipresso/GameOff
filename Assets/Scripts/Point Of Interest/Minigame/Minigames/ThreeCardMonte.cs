using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ThreeCardMonte is a <see cref="Minigame"/> where the player 
/// has to track a card. If they select the correct card after shuffling,
/// they win. Else, they lose.
/// </summary>
public class ThreeCardMonte : Minigame
{
    [SerializeField] bool[] winner = new bool[3];
    [SerializeField] int shuffles = 10;
    [SerializeField] GameObject selectionBlocker;
    [SerializeField] GameObject hint;
    [SerializeField] List<GameObject> buttons = new List<GameObject>();

    public override void StartMinigame(MinigameCaller caller)
    {
        base.StartMinigame(caller);
        selectionBlocker.SetActive(true);

        for(int i = 0; i < 3; i++)
        {
            if (i == 1) winner[i] = true;
            else winner[i] = false;
        }

        foreach(GameObject button in buttons)
        {
            button.SetActive(true);
        }
        hint.SetActive(true);

        StartCoroutine(Shuffle());
    }

    /// <summary>
    /// For each shuffle, swap a pair of cards and do a blink animation with them.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/></returns>
    IEnumerator Shuffle()
    {
        yield return new WaitForSeconds(2);
        hint.SetActive(false);

        for(int i = 0; i < shuffles; i++)
        {
            int start = Random.Range(0, 3);
            int end = Random.Range(0, 3);
            
            if (end == start) end = (end + 1) % 3;

            bool temp = winner[start];
            winner[start] = winner[end];
            winner[end] = temp;

            //Do a little blink animation.
            for(int j = 0; j < 2; j++) {
                yield return new WaitForSeconds(.5f);
                buttons[start].SetActive(!buttons[start].activeSelf);
                buttons[end].SetActive(!buttons[end].activeSelf);
            }
        }
        selectionBlocker.SetActive(false);
    }

    /// <summary>
    /// Select a card. If the winner is a the given index,
    /// you win. Lose otherwise.
    /// </summary>
    /// <param name="index">Index of card</param>
    public void SelectCard(int index)
    {
        index = index % 3;
        if (winner[index])
        {
            if (showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.WIN);
            caller.CompleteMinigame(MinigameStatus.WIN);
        }
        else
        {
            if (showResultOnUI) MinigameResultUI.instance.ShowResult(MinigameStatus.LOST);
            caller.CompleteMinigame(MinigameStatus.LOST);
        }
        EndMinigame();
    }

    public override void EndMinigame()
    {
        StopAllCoroutines();
        selectionBlocker.SetActive(false);
        foreach(GameObject button in buttons)
        {
            button.SetActive(false);
        }
        hint.SetActive(false);
    }

    public override void ReportObstacleHit()
    {
        throw new System.NotImplementedException("No obstacles in this minigame!");
    }
}
