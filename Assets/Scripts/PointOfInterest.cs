using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Routes
{
    [SerializeField] string direction;
    [SerializeField] PointOfInterest destination;
    //[SerializeField] CheatsIDunnoWhateverWeEndUpCallingThis requiredCheats;

    public string GetDirection()
    {
        return direction;
    }

    public PointOfInterest GetDestination()
    {
        return destination;
    }

    public bool CanTravel()
    {
        /*False conditions*/

        return true;
    }
}

[CreateAssetMenu(menuName = "Scriptable Objects/Point of Interest")]
public class PointOfInterest : ScriptableObject
{
    [TextArea(3, 10)]
    [SerializeField] string description = "";
    [SerializeField] Sprite image;
    [SerializeField] List<Routes> routes = new List<Routes>();

    public string GetDescription()
    {
        if (description.Length == 0) return "There is no discription of this place.";
        return description;
    }

    public Sprite GetImage()
    {
        if (image == null) return null; //We should define a global default image.
        return image;
    }

    public PointOfInterest Move(string[] tokens)
    {
        for(int i = 0; i < routes.Count; i++)
        {
            if (tokens[1].Equals(routes[i].GetDirection().ToLower().Trim()) && routes[i].CanTravel()) return routes[i].GetDestination();
        }
        return null;
    }
}
