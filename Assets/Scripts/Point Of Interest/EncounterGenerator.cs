using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterGenerator : MonoBehaviour
{
    [SerializeField] public PointOfInterest startingPOI;
    [SerializeField] List<Encounter> combatEncounters;
    [SerializeField] List<Encounter> nonCombatEncounters;
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

            AddEncounter(current, ref nonCombatEncountersRemaining, maxNonCombatEncounters, ref combatEncountersRemaining, maxCombatEncounters);
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
