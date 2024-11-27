using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TreasureChestEncounter is a noncombat <see cref="Encounter"/> 
/// which gives the player an item when they open it. Since these items 
/// are used to negate any punishment later on, the player takes a very 
/// slight punishment when opening the chest.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Encounters/Treasure Chest Encounter")]
public class TreasureChestEncounter : Encounter
{
    [Header("Chest Penalties")]
    [SerializeField] int damage = 1;
    [SerializeField] int speedLost = 1;
    [SerializeField] int textSpeedLost = 1;

    [Header("Chest Flavor")]
    [TextArea(3, 10)][SerializeField] string ignoreText;
    [TextArea(3, 10)][SerializeField] string inspectText;

    public override bool IsEncounterKeyword(string[] tokens)
    {
        if(base.IsEncounterKeyword(tokens)) return true;
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "ignore":
                case "inspect":
                    return true;
            }
        }
        return false;
    }

    public override string ParseEncounterKeywords(string[] tokens)
    {
        string baseResponse = base.ParseEncounterKeywords(tokens);
        if(!baseResponse.Equals("Keyword not recognized.")) return baseResponse;
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "ignore":
                    LeaveEncounter();
                    return ignoreText;
                case "inspect":
                    return InspectChest();
            }
        }

        return $"Keyword not recognized.";
    }

    private string InspectChest()
    {
        //No such thing as a free lunch.
        PlayerStats.instance.TakeDamage(damage);
        PlayerStats.instance.DecreaseSpeed(speedLost);
        PlayerStats.instance.DecreaseTypingSpeed(textSpeedLost);
        LeaveEncounter();
        if(Random.Range(0, 2) == 0)
        {
            PlayerInventory.instance.AddApple();
            return $"{inspectText} an <i>apple</i>.";
        }
        else
        {
            PlayerInventory.instance.AddSmoke();
            return $"{inspectText} a <i>smoke</i>.";
        }
    }
}
