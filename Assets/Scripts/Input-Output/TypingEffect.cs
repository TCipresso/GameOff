using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Adds a typing effect to any TextMeshProUGUI text box, mimicking an old computer.
/// </summary>
public class TypingEffect : MonoBehaviour
{
    [Header("Typing Settings")]
    [SerializeField] private float typingSpeed = 0.05f; // Speed of typing
    [SerializeField] private AudioClip typingSound; // Sound effect for typing
    [SerializeField] private int charsPerSound = 3; // Play sound every X characters

    [Header("References")]
    [SerializeField] private AudioSource audioSource; // Audio source for playing sounds
    [SerializeField] private TextMeshProUGUI textBox; // Target text box for the typing effect

    /// <summary>
    /// Types out a single string with typing effect.
    /// </summary>
    /// <param name="text">The text to type out.</param>
    public IEnumerator TypeText(string text)
    {
        textBox.text = ""; // Clear the text box before starting
        int charCount = 0;

        foreach (char c in text)
        {
            textBox.text += c; // Add character to text box
            charCount++;

            // Play sound every 'charsPerSound' characters
            if (charCount % charsPerSound == 0 && typingSound != null)
            {
                audioSource.PlayOneShot(typingSound);
            }

            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    /// <summary>
    /// Types out a list of strings with a typing effect.
    /// </summary>
    /// <param name="lines">The lines to type out one by one.</param>
    public IEnumerator TypeText(List<string> lines)
    {
        foreach (var line in lines)
        {
            yield return StartCoroutine(TypeText(line)); // Type each line

            // Add a small delay between lines
            textBox.text += "\n";
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
