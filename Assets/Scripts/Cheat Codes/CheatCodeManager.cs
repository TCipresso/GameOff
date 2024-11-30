using System.Collections.Generic;
using UnityEngine;

public class CheatCodeManager : MonoBehaviour, MinigameCaller
{
    public static CheatCodeManager instance;

    [Header("Cheat Code Management")]
    public List<CheatCode> discoveredCheatCodes = new List<CheatCode>();
    public List<CheatCode> undiscoveredCheatCodes = new List<CheatCode>();

    [Header("UI")]
    public GameObject cheatsUI;
    public GameObject CheatsPanel;
    private bool isDisplaying = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip successCheatSound;

    private CheatCode currentCheatCode;

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

    public bool TryActivateCheat(string cheatName)
    {
        foreach (CheatCode cheat in discoveredCheatCodes)
        {
            if (cheat.cheatName.Equals(cheatName, System.StringComparison.OrdinalIgnoreCase))
            {
                if (cheat.GetRemainingCharges() > 0)
                {
                    Debug.Log($"Cheat '{cheatName}' found in discovered cheats and activated.");
                    ActivateCheat(cheat);
                    return true;
                }
                else
                {
                    TextOutput.instance.Print($"Nice try, cheater, Try something else.");
                    return false;
                }
            }
        }

        foreach (CheatCode cheat in undiscoveredCheatCodes)
        {
            if (cheat.cheatName.Equals(cheatName, System.StringComparison.OrdinalIgnoreCase))
            {
                if (cheat.GetRemainingCharges() > 0)
                {
                    Debug.Log($"Cheat '{cheatName}' found in undiscovered cheats. Moving to discovered and activating.");
                    undiscoveredCheatCodes.Remove(cheat);
                    AddCheatToDiscovered(cheat);
                    ActivateCheat(cheat);
                    return true;
                }
                else
                {
                    Debug.Log($"Nice try, cheater! No charges left for cheat '{cheatName}'.");
                    return false;
                }
            }
        }

        Debug.Log($"Cheat '{cheatName}' not found in any cheat lists.");
        return false;
    }


    private void ActivateCheat(CheatCode cheat)
    {
        Debug.Log($"Activating cheat: {cheat.cheatName}");

        currentCheatCode = cheat;

        if (cheat.minigameIndex >= 0 && cheat.minigameIndex < MinigameManager.instance.minigames.Count)
        {
            MinigameManager.instance.PlayMinigame(cheat.minigameIndex, this);
            PlayCheatSuccessSound();
        }
        else
        {
            Debug.LogWarning($"No valid minigame associated with cheat: {cheat.cheatName}");
        }
    }

    public void StartMinigame()
    {
        Debug.Log("StartMinigame called in CheatCodeManager.");
    }

    public void CompleteMinigame(MinigameStatus gameResult)
    {
        if (gameResult == MinigameStatus.WIN && currentCheatCode != null)
        {
            Debug.Log($"Executing effect of cheat: {currentCheatCode.cheatName}");
            currentCheatCode.ExecuteEffect();
        }
        else
        {
            Debug.Log($"Minigame ended with status: {gameResult}. No effect applied.");
        }

        currentCheatCode = null;
    }

    private void PlayCheatSuccessSound()
    {
        if (audioSource != null && successCheatSound != null)
        {
            audioSource.PlayOneShot(successCheatSound);
        }
    }

    public List<CheatCode> GetDiscoveredCheats()
    {
        return discoveredCheatCodes;
    }

    public void ToggleCheatDisplay()
    {
        isDisplaying = !isDisplaying;
        cheatsUI.SetActive(isDisplaying);
        CheatsPanel.SetActive(isDisplaying);
    }

    /// <summary>
    /// USE THIS FOR FORSIGHT
    /// USE THIS TO DIRECTLY ADD A CHEATCODE OF YOUR CHOOSING. 
    /// </summary>
    /// <param name="cheat"></param>
    public void AddCheatToDiscovered(CheatCode cheat)
    {
        if (!discoveredCheatCodes.Contains(cheat))
        {
            discoveredCheatCodes.Add(cheat);
            TextOutput.instance.Print($"Cheat code '{cheat.cheatName}' has been discovered.");
        }
    }

    /// <summary>
    /// USE THIS FOR CAMPFIRE
    /// Adds one charge to all discovered cheats
    /// </summary>
    public void AddCheatCharge()
    {
        foreach (CheatCode cheat in discoveredCheatCodes)
        {
            if (cheat.GetRemainingCharges() < cheat.maxUses)
            {
                cheat.AddCharge(1);
                Debug.Log($"Recharged 1 charge for cheat '{cheat.cheatName}'. Current charges: {cheat.GetRemainingCharges()}.");
            }
            else
            {
                Debug.Log($"Cheat '{cheat.cheatName}' is already at maximum charges ({cheat.maxUses}).");
            }
        }
    }

    /// <summary>
    /// USE THIS FOR THE CHEST
    /// Adds a random cheat code from the undiscovered list to the discovered list.
    /// </summary>
    public void GiveRandCheat()
    {
        if (undiscoveredCheatCodes.Count == 0)
        {
            Debug.Log("No more undiscovered cheat codes to reveal.");
            return;
        }

        int randomIndex = Random.Range(0, undiscoveredCheatCodes.Count);
        CheatCode randomCheat = undiscoveredCheatCodes[randomIndex];
        undiscoveredCheatCodes.RemoveAt(randomIndex);
        AddCheatToDiscovered(randomCheat);
        Debug.Log($"New cheat discovered: {randomCheat.cheatName}");
    }


}
