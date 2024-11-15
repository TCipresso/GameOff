using System.Collections.Generic;
using UnityEngine;

public class CheatCodeManager : MonoBehaviour, MinigameCaller
{
    public static CheatCodeManager instance;
    public List<CheatCode> discoveredCheatCodes = new List<CheatCode>();
    public List<CheatCode> undiscoveredCheatCodes = new List<CheatCode>();
    public GameObject cheatsUI;
    private bool isDisplaying = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        cheatsUI.SetActive(false);
    }

    /// <summary>
    /// Adds a cheat to the discovered cheats if it’s not already present.
    /// </summary>
    public void AddCheatToDiscovered(CheatCode cheat)
    {
        if (!discoveredCheatCodes.Contains(cheat))
        {
            discoveredCheatCodes.Add(cheat);
            Debug.Log($"Cheat code '{cheat.cheatName}' has been discovered and added to discovered cheats.");
        }
    }

    /// <summary>
    /// Checks if a cheat exists in the undiscovered list, moves it to discovered if found, and activates it.
    /// </summary>
    /// <param name="cheatName">The name of the cheat code to activate</param>
    /// <returns>True if the cheat was activated, false otherwise</returns>
    public bool TryActivateCheat(string cheatName)
    {
        // First, check in the discovered list
        foreach (CheatCode cheat in discoveredCheatCodes)
        {
            if (cheat.cheatName.Equals(cheatName, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"Cheat '{cheatName}' found in discovered cheats and activated.");
                ActivateCheat(cheat);
                return true;
            }
        }

        // Then, check in the undiscovered list
        foreach (CheatCode cheat in undiscoveredCheatCodes)
        {
            if (cheat.cheatName.Equals(cheatName, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"Cheat '{cheatName}' found in undiscovered cheats. Moving to discovered and activating.");
                undiscoveredCheatCodes.Remove(cheat);
                AddCheatToDiscovered(cheat);
                ActivateCheat(cheat);
                return true;
            }
        }

        Debug.Log($"Cheat '{cheatName}' not found in any cheat lists.");
        return false;
    }

    /// <summary>
    /// Activates the specified cheat.
    /// </summary>
    private void ActivateCheat(CheatCode cheat)
    {
        Debug.Log($"Activating cheat: {cheat.cheatName}");

        if (cheat.minigameIndex >= 0 && cheat.minigameIndex < MinigameManager.instance.minigames.Count)
        {
            MinigameManager.instance.PlayMinigame(cheat.minigameIndex, this);
        }
        else
        {
            Debug.LogWarning($"No valid minigame associated with cheat: {cheat.cheatName}");
        }
    }

    //I JUST PUT THIS SHIT HERE TO SHUT THE DAMN INTERFACE UP

    /// <summary>
    /// Required implementation of StartMinigame() from MinigameCaller.
    /// </summary>
    public void StartMinigame()
    {
        Debug.Log("StartMinigame called in CheatCodeManager.");
        // You can add code here if you need CheatCodeManager to do anything when a minigame starts
    }

    /// <summary>
    /// Required implementation of CompleteMinigame from MinigameCaller.
    /// </summary>
    public void CompleteMinigame(MinigameStatus gameResult)
    {
        Debug.Log($"Minigame completed with result: {gameResult}");
        // Implement behavior when a minigame is completed, if needed
    }

    /// <summary>
    /// Returns the list of discovered cheats for display purposes.
    /// </summary>
    public List<CheatCode> GetDiscoveredCheats()
    {
        return discoveredCheatCodes;
    }

    /// <summary>
    /// Toggles the cheat display UI.
    /// </summary>
    public void ToggleCheatDisplay()
    {
        isDisplaying = !isDisplaying;
        cheatsUI.SetActive(isDisplaying);
    }
}
