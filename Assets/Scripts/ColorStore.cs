using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorTypes { HUDColor, NarratorColor }

/// <summary>
/// ColorStore is a scriptatble object that stores 
/// the colors for <see cref="Colorable"/> objects.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Color Store")]
public class ColorStore : ScriptableObject
{
    [Header("Colors")]
    [SerializeField] Color narratorColor;
    [SerializeField] Color hudColor;
    List<Colorable> colorableObjects = new List<Colorable>();

    public Color GetColor(Colorable obj, ColorTypes wanted)
    {
        colorableObjects.Add(obj);
        switch (wanted)
        {
            default:
            case ColorTypes.HUDColor:
                return hudColor;
            case ColorTypes.NarratorColor:
                return narratorColor;
        }
    }

    public void UpdateColors()
    {
        //TODO: Go through each colorable and tell them to change.
    }
}
