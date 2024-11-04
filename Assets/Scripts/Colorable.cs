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
    public ColorTypes colorType;

    private void Awake()
    {
        UpdateColor(colorStore.GetColor(this, colorType));
    }

    public void UpdateColor(Color color)
    {
        foreach(Image image in images)
            image.color = color;
        foreach(TextMeshProUGUI text in texts)
            text.color = color;
    }
}
