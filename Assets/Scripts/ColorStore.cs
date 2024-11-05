using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// Unsubscribes all colorable objects. 
    /// Mainly for uses when leaving play mode to clear references.
    /// </summary>
    public void OnDisable()
    {
        colorableObjects.Clear();
    }

    /// <summary>
    /// Subscribes a <see cref="Colorable"/>. Returns the color wanted.
    /// </summary>
    /// <param name="obj">The <see cref="Colorable"/> object.</param>
    /// <param name="wanted">The <see cref="ColorType"/> wanted.</param>
    /// <returns>The color of the <see cref="ColorType"/> wanted.</returns>
    public Color GetColor(Colorable obj, ColorType wanted)
    {
        colorableObjects.Add(obj);
        return GetColor(wanted);
    }

    /// <summary>
    /// Returns the color wanted.
    /// </summary>
    /// <param name="wanted">The <see cref="ColorType"> wanted.</param>
    /// <returns>The color of the <see cref="ColorType"> wanted.</returns>
    public Color GetColor(ColorType wanted)
    {
        switch (wanted)
        {
            default:
            case ColorType.HUDCOLOR:
                return hudColor;
            case ColorType.NARRATORCOLOR:
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
                case ColorType.HUDCOLOR:
                    obj.UpdateColor(hudColor); 
                    break;
                case ColorType.NARRATORCOLOR:
                    obj.UpdateColor(narratorColor);
                    break;
            }
        }
    }
}
