using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Routes
{
    [SerializeField] string direction;
    [SerializeField] PointOfInterest destination;
    //[SerializeField] CheatsIDunnoWhateverWeEndUpCallingThis requiredCheats;
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
}
