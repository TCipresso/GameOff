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
    [SerializeField] public Color[] colors;
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
        int index = (int)wanted;
        if (index < 0 || index > colors.Length)
            return colors[0];
        return colors[index];
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
                default:
                case ColorType.HUDCOLOR:
                    obj.UpdateColor(colors[0]); 
                    break;
                case ColorType.NARRATORCOLOR:
                    obj.UpdateColor(colors[1]);
                    break;
                case ColorType.INPUTCOLOR:
                    obj.UpdateColor(colors[2]);
                    break;
                case ColorType.STORYCOLOR:
                    obj.UpdateColor(colors[3]);
                    break;
            }
        }
    }
}
