using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PenultimateEncounter is a noncombat <see cref="Encounter"/>.
/// It pretends that it is a deadend and forces the player to use a 
/// special case of noclip to proceed. Meant to be used for the second
/// to last room in the game hence the name.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Encounters/Penultimate Encounter")]
public class PenultimateEncounter : Encounter
{
    [Header("Penultimate Encounter Flavor")]
    [TextArea(3, 10)][SerializeField] string noclipResponse;
    [TextArea(3, 10)][SerializeField] List<string> descriptions;
    [SerializeField] int descriptionIndex = -1; //Serialized for debugging.
    [SerializeField] bool isSetUp = false; //Serialized for debugging.

    private void OnDisable()
    {
        isSetUp = false;
    }

    public override string GetDescription()
    {
        if(!isSetUp)
        {
            isSetUp = true;
            descriptionIndex = -1;
        }
        descriptionIndex++;
        descriptionIndex %= descriptions.Count;
        return descriptions[descriptionIndex];
    }

    public override List<string> GetEncounterKeywords()
    {
        if (Random.Range(0, 1000) < 999) return null;
        return base.GetEncounterKeywords();
    }

    public override bool IsEncounterKeyword(string[] tokens)
    {
        if (base.IsEncounterKeyword(tokens)) return true;
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "noclip":
                    return true;
            }
        }
        return false;
    }

    public override string ParseEncounterKeywords(string[] tokens)
    {
        string baseRespone = base.ParseEncounterKeywords(tokens);
        if(!baseRespone.Equals("Keyword not recognized.")) return baseRespone;
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "noclip":
                    LeaveEncounter();
                    return noclipResponse;
            }
        }

        return $"Keyword not recognized.";
    }

    public override void LeaveEncounter()
    {
        isSetUp = false;
        base.LeaveEncounter();
    }
}
