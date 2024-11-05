using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Colorable is an object whose color can be 
/// changed by the player or something else in 
/// the game.
/// </summary>
public class Colorable : MonoBehaviour
{
    [Header("Color Store")]
    [SerializeField] ColorStore colorStore;

    [Header("Colorable Parts")]
    [SerializeField] List<Image> images = new List<Image>();
    [SerializeField] List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    public ColorType colorType;

    /// <summary>
    /// Subscribe to the color score and get my color.
    /// </summary>
    private void Awake()
    {
        UpdateColor(colorStore.GetColor(this, colorType));
    }

    /// <summary>
    /// Update the color of my images and text to the provided color.
    /// </summary>
    /// <param name="color"><see cref="Color"/> to update to.</param>
    public void UpdateColor(Color color)
    {
        for(int i = 0; i < images.Count; i++)
            images[i].color = color;
        for(int i = 0; i < texts.Count; i++)
            texts[i].color = color;
    }
}
