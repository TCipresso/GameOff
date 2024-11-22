using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterGenerator : MonoBehaviour
{
    [Header("Test Start")]
    [SerializeField] public PointOfInterest startingPOI;

    [Header("Encounters")]
    [SerializeField] List<Encounter> combatEncounters;
    [SerializeField] List<Encounter> nonCombatEncounters;
    [SerializeField] List<Encounter> bossEncounters;
    [SerializeField] int bossIndex;

    [Header("Generation Parameters")]
    [Tooltip("Noncombat Encounters (x) : Combat Encounters (y)")]
    [SerializeField] Vector2 nCToCRatio = new Vector2(1, 2);
    [Range(0, 100)]
    [SerializeField] int chanceToIgnoreRatio = 10;

    public void GenerateEncounters(PointOfInterest root)
    {
        IterativeDFS(root);
    }

    private void IterativeDFS(PointOfInterest root)
    {
        bool atStart = true;
        int maxNonCombatEncounters = (int) nCToCRatio.x;
        int maxCombatEncounters = (int) nCToCRatio.y;
        int nonCombatEncountersRemaining = maxNonCombatEncounters;
        int combatEncountersRemaining = maxCombatEncounters;

        Stack<PointOfInterest> stack = new Stack<PointOfInterest>();
        List<PointOfInterest> visited = new List<PointOfInterest>();
        stack.Push(root);

        while (stack.Count > 0)
        {
            PointOfInterest current = stack.Pop();
            visited.Add(current);

            List<Route> routes = current.GetRoutes();
            foreach (Route route in routes)
            {
                PointOfInterest destination = route.GetDestination();
                if (!visited.Contains(destination) && !stack.Contains(destination)) stack.Push(destination);
            }

            if (!atStart) {
                //In pathway room.
                if (routes.Count > 0)
                    AddEncounter(current, ref nonCombatEncountersRemaining, maxNonCombatEncounters, ref combatEncountersRemaining, maxCombatEncounters);
                
                //In the boss room. Makes it where the ending room always has a boss.
                else
                { 
                    Debug.Log($"{current.name} is the boss room.");
                    current.SetEncounter(bossEncounters[bossIndex]);
                }
            }

            //In starting room. Makes it where starting room is always empty so the player doesn't start in combat and has time to settle in.
            else {
                Debug.Log($"{current.name} is the starting room.");
                current.SetEncounter(null);
                atStart = false;
                nonCombatEncountersRemaining--;
            }
        }

        Debug.Log($"Visited {visited.Count} nodes.");
    }

    private void AddEncounter(PointOfInterest root, ref int nonCombatEncountersRemaining, int maxNonCombatEncounters, ref int combatEncountersRemaining, int maxCombatEncounters)
    {
        //I noticed that there would be a pattern if we kept to the ratio all the time,
        //so there's a random chance that the ratio is ignored.
        if(Random.Range(0, 100) + 1 <= chanceToIgnoreRatio)
        {
            switch(Random.Range(0, 2))
            {
                default:
                case 0:
                    SetNoncombatEncounter(root);
                    break;
                case 1:
                    SetCombatEncounter(root);
                    break;
            }
            return;
        }

        if (combatEncountersRemaining <= 0)
        {
            nonCombatEncountersRemaining = maxNonCombatEncounters;
            combatEncountersRemaining = maxCombatEncounters;
        }

        if (nonCombatEncountersRemaining > 0 && nonCombatEncounters.Count > 0)
        {
            SetNoncombatEncounter(root);
            nonCombatEncountersRemaining--;
        }
        else if(combatEncounters.Count > 0)
        {
            SetCombatEncounter(root);
            combatEncountersRemaining--;
        }
        else
        {
            Debug.LogWarning("Both noncombatEncounters and combatEncounters are empty!");
            root.SetEncounter(null);
        }
    }

    private void SetNoncombatEncounter(PointOfInterest root)
    {
        Debug.Log($"{root.name} is a noncombat encounter");
        int chosenEncounter = 0;
        if (nonCombatEncounters.Count > 0) chosenEncounter = Random.Range(0, nonCombatEncounters.Count + 1); //Chance for a plainly empty room.
        if (nonCombatEncounters.Count == 0 || chosenEncounter >= nonCombatEncounters.Count) root.SetEncounter(null); 
        else root.SetEncounter(nonCombatEncounters[chosenEncounter]);
    }

    private void SetCombatEncounter(PointOfInterest root)
    {
        Debug.Log($"{root.name} is a combat encounter");
        root.SetEncounter(combatEncounters[Random.Range(0, combatEncounters.Count)]);
    }

    IEnumerator GenerateEncountersCoroutine(PointOfInterest startPOI)
    {
        yield return null;
    }
}
