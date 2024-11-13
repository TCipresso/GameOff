using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A point of interest is the current location state of the game.
/// It defines the location and interactions available.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Point of Interest")]
public class PointOfInterest : ScriptableObject
{
    [TextArea(3, 10)]
    [SerializeField] List<Route> routes = new List<Route>();
    [SerializeField] Sprite image;
    [SerializeField] Encounter encounter; 

    /// <summary>
    /// Get the description of the current POI if it has one.
    /// </summary>
    /// <returns>The description (or lack of) of the POI.</returns>
    public string GetDescription()
    {
        return "POI are being redone";
        //if (description.Length == 0) return "There is no discription of this place.";
        //return description;
    }

    /// <summary>
    /// Gets the Image of the current POI if it has one.
    /// </summary>
    /// <returns>The Image of the POI if present or the global default image.</returns>
    public Sprite GetImage()
    {
        if (image == null) return null; //We should define a global default image.
        return image;
    }

    /// <summary>
    /// Move through a route.
    /// </summary>
    /// <param name="tokens">Tokens defining what route to take.</param>
    /// <returns>The <see cref="PointOfInterest"/> if the route is vaild, null otherwise.</returns>
    public PointOfInterest Move(string[] tokens)
    {
        for(int i = 0; i < routes.Count; i++)
        {
            //I could extend this and have it check multiple tokens for more detailed directions. Just lmk.
            if (tokens[1].Equals(routes[i].GetDirection().ToLower().Trim()) && routes[i].CanTravel()) return routes[i].GetDestination();
        }
        return null;
    }
}
