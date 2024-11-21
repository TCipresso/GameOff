using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerStats playerStats;

    [Header("Flicker Colors")]
    public Color flickerColorRed = Color.red; 
    public Color flickerColorBlack = Color.black; 
    public Color normalColor = Color.green;   

    [Header("Flicker Settings")]
    public int flickerCount = 3;
    public float flickerSpeed = 0.1f;

    [Header("UI Components")]
    public List<Image> images;
    public List<TextMeshProUGUI> textMeshPros;
    public List<TMP_InputField> inputFields;

    [Header("Enemy Sprites")]
    public List<Image> enemySprites;


    void Start()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged += HandleHealthChanged;
        }
    }

    void HandleHealthChanged()
    {
        StartCoroutine(FlickerEffect());
    }

    IEnumerator FlickerEffect()
    {
        for (int i = 0; i < flickerCount; i++)
        {
            SetAllUIColor(flickerColorBlack);
            yield return new WaitForSeconds(flickerSpeed);
            SetAllUIColor(flickerColorRed);
            yield return new WaitForSeconds(flickerSpeed);
        }
        // Set all UI components back to normal except enemy sprites
        ResetUIColor();

        // Set enemy sprites to red permanently after flickering
        SetEnemyUIColor(flickerColorRed);
    }

    void SetAllUIColor(Color color)
    {
        // This method now includes enemy sprites in the flickering
        foreach (var image in images)
        {
            image.color = color;
        }
        foreach (var textMesh in textMeshPros)
        {
            textMesh.color = color;
        }
        foreach (var inputField in inputFields)
        {
            var bgImage = inputField.GetComponentInChildren<Image>();
            if (bgImage != null)
            {
                bgImage.color = color;
            }
        }
        // Include enemy sprites in the universal color set
        foreach (var sprite in enemySprites)
        {
            sprite.color = color;
        }
    }

    void ResetUIColor()
    {
        // Reset colors for all UI elements to normalColor, except enemy sprites
        foreach (var image in images)
        {
            if (!enemySprites.Contains(image)) // Exclude enemy sprites
                image.color = normalColor;
        }
        foreach (var textMesh in textMeshPros)
        {
            textMesh.color = normalColor;
        }
        foreach (var inputField in inputFields)
        {
            var bgImage = inputField.GetComponentInChildren<Image>();
            if (bgImage != null && !enemySprites.Contains(bgImage))
            {
                bgImage.color = normalColor;
            }
        }
    }

    void SetEnemyUIColor(Color color)
    {
        // Specifically set enemy sprites' color
        foreach (var sprite in enemySprites)
        {
            sprite.color = color;
        }
    }

    void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged -= HandleHealthChanged;
        }
    }
}
