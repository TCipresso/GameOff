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

    [Header("Audio Settings")]
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip healthChangeClip; // Clip to play on health change

    void Start()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged += HandleHealthChanged;
        }
    }

    void HandleHealthChanged()
    {
        // Play the health change sound
        PlayHealthChangeSound();

        // Start the flicker effect
        StartCoroutine(FlickerEffect());
    }

    /// <summary>
    /// Plays the sound effect when health changes.
    /// </summary>
    private void PlayHealthChangeSound()
    {
        if (audioSource != null && healthChangeClip != null)
        {
            audioSource.PlayOneShot(healthChangeClip);
        }
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
        foreach (var sprite in enemySprites)
        {
            sprite.color = color;
        }
    }

    void ResetUIColor()
    {
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
