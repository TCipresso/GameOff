using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FountainEncounter is a Noncombat <see cref="Encounter"/>. It will fully heal 
/// the player if they chose to bathe in it.
/// </summary>
[CreateAssetMenu(menuName ="Scriptable Objects/Encounters/Fountain Encounter")]
public class FountainEncounter : Encounter
{
    [Header("Fountain Flavor")]
    [TextArea(3, 10)]
    [SerializeField] string batheString;
    [TextArea(3, 10)]
    [SerializeField] string ignoreString;

    public override string GetDescription()
    {
        return base.GetDescription();
    }

    public override bool IsEncounterKeyword(string[] tokens)
    {
        if(base.IsEncounterKeyword(tokens)) return true;
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "bathe":
                case "ignore":
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
                case "bathe":
                    Debug.Log("Accepted Fountain");
                    return AcceptFountain();
                case "ignore":
                    Debug.Log("Ignored Fountain");
                    return DenyFountain();
            }
        }

        return $"Keyword not recognized.";
    }

    /// <summary>
    /// Heals the player to full.
    /// </summary>
    /// <returns>Description of the player bathing.</returns>
    private string AcceptFountain()
    {
        PlayerStats.instance.Heal();
        LeaveEncounter();
        return batheString;
    }

    /// <summary>
    /// Leave the encounter.
    /// </summary>
    /// <returns>Description of the player leaving.</returns>
    private string DenyFountain()
    {
        LeaveEncounter();
        return ignoreString;
    }
}
