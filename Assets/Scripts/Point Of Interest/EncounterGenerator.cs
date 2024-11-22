using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterGenerator : MonoBehaviour
{
    [SerializeField] public PointOfInterest startingPOI;

    public void GenerateEncounters(PointOfInterest root)
    {
        IterativeDFS(root);
    }

    private void IterativeDFS(PointOfInterest root)
    {
        Stack<PointOfInterest> stack = new Stack<PointOfInterest>();
        List<PointOfInterest> visited = new List<PointOfInterest>();
        stack.Push(root);

        while (stack.Count > 0)
        {
            PointOfInterest current = stack.Pop();
            visited.Add(current);

            Debug.Log(current.name);
            List<Route> routes = current.GetRoutes();
            foreach (Route route in routes)
            {
                PointOfInterest destination = route.GetDestination();
                if (!visited.Contains(destination) && !stack.Contains(destination)) stack.Push(destination);
            }
        }

        Debug.Log($"Visited {visited.Count} nodes.");
    }

    IEnumerator GenerateEncountersCoroutine(PointOfInterest startPOI)
    {
        yield return null;
    }
}
