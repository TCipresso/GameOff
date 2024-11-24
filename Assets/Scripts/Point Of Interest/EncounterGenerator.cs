using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// EncounterGenerator navigates a graph of <see cref="PointOfInterest"/>s 
/// and populates each POI with an <see cref="Encounter"/>.
/// </summary>
public class EncounterGenerator : MonoBehaviour
{
    [Header("Test Start")]
    [SerializeField] PointOfInterest startingPOI;

    [Header("Encounters")]
    [SerializeField] List<Encounter> combatEncounters;
    [SerializeField] List<Encounter> nonCombatEncounters;
    [SerializeField] List<Encounter> bossEncounters;
    [SerializeField] int bossIndex;
    private int maxNonCombatEncounters;
    private int maxCombatEncounters;
    private int nonCombatEncountersRemaining;
    private int combatEncountersRemaining;

    [Header("Generation Parameters")]
    [Tooltip("Noncombat Encounters (x) : Combat Encounters (y)")]
    [SerializeField] Vector2 nCToCRatio = new Vector2(1, 2);
    [Range(0, 100)]
    [SerializeField] int chanceToIgnoreRatio = 10;

    /// <summary>
    /// Generate floor's encounters based on this object's startingPOI.
    /// </summary>
    public void GenerateEncounters()
    {
        GenerateEncounters(startingPOI);
    }

    /// <summary>
    /// Generate floor's encounters based on provided root.
    /// </summary>
    /// <param name="root"><see cref="PointOfInterest"/> to start graph navigation.</param>
    public void GenerateEncounters(PointOfInterest root)
    {
        maxNonCombatEncounters = (int)nCToCRatio.x;
        maxCombatEncounters = (int)nCToCRatio.y;
        nonCombatEncountersRemaining = maxNonCombatEncounters;
        combatEncountersRemaining = maxCombatEncounters;
        IterativeDFS(root);
    }

    private void IterativeDFS(PointOfInterest root)
    {
        bool atStart = true;

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

            if (current.AllowEncounterChange()) PopulatePOIEncounter(current, routes.Count, atStart);
            else Debug.Log($"{current.name} does not allow encounter changes");
            if (atStart) atStart = false;
        }

        Debug.Log($"Visited {visited.Count} nodes.");
    }

    /// <summary>
    /// Determine what type of encounter is best for provided root.
    /// </summary>
    /// <param name="root">Current <see cref="PointOfInterest"/></param>
    /// <param name="atStart">Is this the floor's starting root?</param>
    private void PopulatePOIEncounter(PointOfInterest root, bool atStart = false)
    {
        PopulatePOIEncounter(root, root.GetRoutes().Count, atStart);
    }

    /// <summary>
    /// Determine what type of encounter is best for provided root.
    /// </summary>
    /// <param name="root">Current <see cref="PointOfInterest"/></param>
    /// <param name="numRoutes">The number of <see cref="Route"/>s this POI has.</param>
    /// <param name="atStart">Is this the floor's starting root?</param>
    private void PopulatePOIEncounter(PointOfInterest root, int numRoutes, bool atStart)
    {
        if (!atStart)
        {
            //In pathway room.
            if (numRoutes > 0)
                AddEncounter(root);

            //In the boss room. Makes it where the ending room always has a boss.
            else
            {
                Debug.Log($"{root.name} is the boss room.");
                root.SetEncounter(bossEncounters[bossIndex]);
            }
        }

        //In starting room. Makes it where starting room is always empty so the player doesn't start in combat and has time to settle in.
        else
        {
            Debug.Log($"{root.name} is the starting room.");
            root.SetEncounter(null);
            nonCombatEncountersRemaining--;
        }
    }

    /// <summary>
    /// Add an encount to current <see cref="PointOfInterest"/>.
    /// </summary>
    /// <param name="root">Current <see cref="PointOfInterest"/></param>
    private void AddEncounter(PointOfInterest root)
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

    /// <summary>
    /// Set the current <see cref="PointOfInterest"/>'s <see cref="Encounter"/> to a Noncombat Encounter.
    /// </summary>
    /// <param name="root">The current <see cref="PointOfInterest"/></param>
    private void SetNoncombatEncounter(PointOfInterest root)
    {
        Debug.Log($"{root.name} is a noncombat encounter");
        int chosenEncounter = 0;
        if (nonCombatEncounters.Count > 0) chosenEncounter = Random.Range(0, nonCombatEncounters.Count + 1); //Chance for a plainly empty room.
        if (nonCombatEncounters.Count == 0 || chosenEncounter >= nonCombatEncounters.Count) root.SetEncounter(null); 
        else root.SetEncounter(nonCombatEncounters[chosenEncounter]);
    }

    /// <summary>
    /// Set the current <see cref="PointOfInterest"/>'s <see cref="Encounter"/> to a Combat Encounter.
    /// </summary>
    /// <param name="root">The current <see cref="PointOfInterest"/></param>
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
