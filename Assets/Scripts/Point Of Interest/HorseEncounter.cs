using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Encounters/Horse Encounter")]
public class HorseEncounter : Encounter
{
    [Header("Horse Statistics")]
    [Range(0, 100)]
    [SerializeField] int baseEvilHorseChance = 10;
    [SerializeField] int evilHorseChance = 0; //Serialized on for debugging.
    [SerializeField] int evilHorseChanceIncrease = 10;
    [SerializeField] bool increaseChancePerGoodHorse = true;
    [SerializeField] bool forceEvilHorse = false;
    [SerializeField] bool isEvil = false;

    [Header("Good Horse Outcomes")]
    [SerializeField] int healthBoostAmount = 5;
    [SerializeField] int damageBoostAmount = 1;
    [SerializeField] int speedBoostAmount = 3;
    [SerializeField] int typingSpeedBoostAmount = 2;

    [Header("Horse Flavor")]
    [TextArea(3, 10)]
    [SerializeField] List<string> petResponse = new List<string>();
    [TextArea(3, 10)]
    [SerializeField] string appleResponse;
    [TextArea(3, 10)]
    [SerializeField] string evilHorseAccept;
    [TextArea(3, 10)]
    [SerializeField] string evilHorseDeny;
    [TextArea(3, 10)]
    [SerializeField] string goodHorseAccept;
    [TextArea(3, 10)]
    [SerializeField] string goodHorseDeny;

    public override string GetDescription()
    {
        if(evilHorseChance == 0) evilHorseChance = baseEvilHorseChance;
        if(forceEvilHorse || Random.Range(0, 100) + 1 <= evilHorseChance)
        {
            isEvil = true;
            evilHorseChance = baseEvilHorseChance;
            return description;
        }
        isEvil = false;
        if (increaseChancePerGoodHorse) evilHorseChance += evilHorseChanceIncrease;
        if (evilHorseChance > 100) evilHorseChance = 100;
        return description;
    }

    public void SetForceEvilHorse(bool isEvil = true)
    {
        forceEvilHorse = isEvil;
    }

    public override bool IsEncounterKeyword(string[] tokens)
    {
        if (base.IsEncounterKeyword(tokens)) return true;
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "pet":
                case "accept":
                case "deny":
                case "apple":
                    return true;
            }
        }
        return false;
    }

    public override string ParseEncounterKeywords(string[] tokens)
    {
        string baseResult = base.ParseEncounterKeywords(tokens);
        if (!baseResult.Equals("Keyword not recognized.")) return baseResult;

        foreach (string token in tokens)
        {
            switch (token)
            {
                case "pet":
                    Debug.Log("Petting Horse");
                    return PetHorse();
                case "accept":
                    Debug.Log("Accepting horse's offer");
                    return AcceptHorse();
                case "deny":
                    Debug.Log("Declining horse's offer");
                    return DenyHorse();
                case "apple":
                    Debug.Log("Giving Apple to horse");
                    return GiveApple();
            }
        }

        return $"Keyword not recognized.";
    }

    private string PetHorse()
    {
        int response = evilHorseChance / 10;
        if (response < 0) response = 0;
        else if(response >= petResponse.Count) response = petResponse.Count - 1;
        return petResponse[response];
    }

    private string AcceptHorse()
    {
        if(isEvil)
        {
            Combat.instance.InitiateCombat(this);
            return evilHorseAccept;
        }

        PlayerStats.instance.maxHP += healthBoostAmount;
        PlayerStats.instance.playerHP += healthBoostAmount;
        PlayerStats.instance.dmg += damageBoostAmount;
        PlayerStats.instance.speed += speedBoostAmount;
        PlayerStats.instance.baseTypingSpeed += typingSpeedBoostAmount;
        GameManager.instance.LeaveEnounter();
        return goodHorseAccept;
    }

    private string DenyHorse()
    {
        GameManager.instance.LeaveEnounter();
        if (isEvil)
        {
            return evilHorseDeny;
        }
        return goodHorseDeny;
    }

    private string GiveApple()
    {
        /*
         * if(haveApple) {
         * apples--;
         */
        isEvil = false;
        return appleResponse;
        //}
        //return "You have no apples to give.";
    }
}
