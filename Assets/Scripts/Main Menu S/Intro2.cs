using System.Collections;
using UnityEngine;
using TMPro;

public class IntroTwo : MonoBehaviour
{
    public TextMeshProUGUI CMPNAME; // Reference to the TextMeshPro component
    public float fadeDuration = 1.5f; // Duration for fading in/out
    public float holdDuration = 2f; // Duration to hold text fully visible

    private void Start()
    {
        // Start the sequence with timing as specified
        StartCoroutine(PlayIntroSequence());
    }

    private IEnumerator PlayIntroSequence()
    {
        // Wait 2 seconds, then fade in the CMPNAME text
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeInText(CMPNAME));

        // Hold the text fully visible for the specified duration
        yield return new WaitForSeconds(holdDuration);

        // Fade out the CMPNAME text
        yield return StartCoroutine(FadeOutText(CMPNAME));
    }

    private IEnumerator FadeInText(TextMeshProUGUI text)
    {
        float elapsedTime = 0f;
        Color color = text.color;
        color.a = 0; // Start fully transparent
        text.color = color;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            text.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 1;
        text.color = color;
    }

    private IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        float elapsedTime = 0f;
        Color color = text.color;
        color.a = 1; // Start fully opaque
        text.color = color;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            text.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 0;
        text.color = color;
    }
}
