using System.Collections.Generic;
using UnityEngine;

public class CheatCodeManager : MonoBehaviour
{
    public List<CheatCode> collectedCheatCodes = new List<CheatCode>();
    public GameObject cheatsUI;
    private bool isDisplaying = false;

    private void Start()
    {
        cheatsUI.SetActive(false);
    }

    public void AddCheat(CheatCode newCheatCode)
    {
        if (!collectedCheatCodes.Contains(newCheatCode))
        {
            collectedCheatCodes.Add(newCheatCode);
        }
    }

    public void ToggleCheatDisplay()
    {
        isDisplaying = !isDisplaying;
        cheatsUI.SetActive(isDisplaying);
    }

    public List<CheatCode> GetCollectedCheats()
    {
        return collectedCheatCodes;
    }
}
