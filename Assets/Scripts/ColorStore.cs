using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Denotes which type of color your want from <see cref="ColorStore"/>
/// </summary>
public enum ColorTypes { HUDColor, NarratorColor }

/// <summary>
/// ColorStore is a scriptatble object that stores 
/// the colors for <see cref="Colorable"/> objects.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Color Store")]
public class ColorStore : ScriptableObject
{
    [Header("Colors")]
    [SerializeField] public Color narratorColor;
    [SerializeField] public Color hudColor;
    List<Colorable> colorableObjects = new List<Colorable>();

    public void OnDisable()
    {
        colorableObjects.Clear();
    }

    /// <summary>
    /// Subscribes a <see cref="Colorable"/>. Returns the color wanted.
    /// </summary>
    /// <param name="obj">The <see cref="Colorable"/> object.</param>
    /// <param name="wanted">The <see cref="ColorTypes"/> wanted.</param>
    /// <returns>The color of the <see cref="ColorTypes"/> wanted.</returns>
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

    /// <summary>
    /// Tells every colorable to update their color.
    /// </summary>
    public void UpdateColorables()
    {
        //Could probably optimize it to only update colorable whose colors actually changed.
        foreach(Colorable obj in colorableObjects)
        {
            switch(obj.colorType)
            {
                case ColorTypes.HUDColor:
                    obj.UpdateColor(hudColor); 
                    break;
                case ColorTypes.NarratorColor:
                    obj.UpdateColor(narratorColor);
                    break;
            }
        }
    }
}
