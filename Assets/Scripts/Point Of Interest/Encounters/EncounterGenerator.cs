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
    [Header("Starting Room")]
    [SerializeField] List<PointOfInterest> startingRooms = new List<PointOfInterest>();

    [Header("Encounters")]
    [SerializeField] List<Encounter> combatEncounters;
    [SerializeField] List<Encounter> nonCombatEncounters;
    private int maxNonCombatEncounters;
    private int maxCombatEncounters;
    private int nonCombatEncountersRemaining;
    private int combatEncountersRemaining;

    [Header("Generation Parameters")]
    [Tooltip("Noncombat Encounters (x) : Combat Encounters (y)")]
    [SerializeField] Vector2 nCToCRatio = new Vector2(1, 2);
    [Range(0, 100)]
    [SerializeField] int chanceToIgnoreRatio = 10;

    private void Awake()
    {
        GenerateEncounters();
    }

    /// <summary>
    /// Generate floor's encounters based on this object's startingPOI.
    /// </summary>
    public void GenerateEncounters()
    {
        maxNonCombatEncounters = (int)nCToCRatio.x;
        maxCombatEncounters = (int)nCToCRatio.y;
        nonCombatEncountersRemaining = maxNonCombatEncounters;
        combatEncountersRemaining = maxCombatEncounters;
        
        for(int i = 0; i < startingRooms.Count; i++)
        {
            IterativeDFS(startingRooms[i], i);
        }
    }

    /// <summary>
    /// Generate floor's encounters based on provided root. Deprecated.
    /// </summary>
    /// <param name="root"><see cref="PointOfInterest"/> to start graph navigation.</param>
    public void GenerateEncounters(PointOfInterest root)
    {
        GenerateEncounters();
    }

    private void IterativeDFS(PointOfInterest root, int floorIndex)
    {
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
                if (destination == current) throw new System.Exception($"{current.name} -> {destination.name}: Destination is self.");
                if (!visited.Contains(destination) && !stack.Contains(destination)) stack.Push(destination);
            }
            
            //Must be the ending room. Add a route to the starting room of the next floor.
            //Here as to avoid nesting previous loop in an else statement.
            if(routes.Count == 0 && floorIndex < startingRooms.Count - 1)
            {
                current.AddRoute(new Route("down", startingRooms[floorIndex + 1]));
            }

            if (current.AllowEncounterChange()) AddEncounter(current);
            else Debug.Log($"{current.name} does not allow encounter changes");
        }

        Debug.Log($"Visited {visited.Count} nodes.");
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
        root.SetEncounter(nonCombatEncounters[Random.Range(0, nonCombatEncounters.Count)]);
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
