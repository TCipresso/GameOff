using UnityEngine;
using System.Collections.Generic;

public class POIManager : MonoBehaviour
{
    public List<POI> poiList = new List<POI>();
    public Dictionary<POI, List<POI>> routes = new Dictionary<POI, List<POI>>();

    private POI currentPOI;

    void Start()
    {
        if (poiList.Count > 0)
        {
            currentPOI = poiList[0];
            DisplayPOI(currentPOI);
        }
    }

    public void DisplayPOI(POI poi)
    {
        Debug.Log("Current POI: " + poi.poiName);
        Debug.Log("Description: " + poi.description);
    }

    public void MoveToPOI(POI targetPOI)
    {
        if (routes.ContainsKey(currentPOI) && routes[currentPOI].Contains(targetPOI))
        {
            currentPOI = targetPOI;
            DisplayPOI(currentPOI);
        }
        else
        {
            Debug.Log("No route to the target POI from the current location.");
        }
    }

    public void AddRoute(POI fromPOI, POI toPOI)
    {
        if (!routes.ContainsKey(fromPOI))
        {
            routes[fromPOI] = new List<POI>();
        }
        routes[fromPOI].Add(toPOI);
    }
}
