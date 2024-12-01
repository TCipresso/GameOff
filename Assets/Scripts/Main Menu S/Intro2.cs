using System.Collections;
using UnityEngine;
using TMPro;

public class IntroTwo : MonoBehaviour
{
    public TextMeshProUGUI CMPNAME;
    public float fadeDuration = 1.5f;
    public float holdDuration = 2f;

    private void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    private IEnumerator PlayIntroSequence()
    {
        yield return new WaitForSecondsRealtime(2f);
        yield return StartCoroutine(FadeInText(CMPNAME));
        yield return new WaitForSecondsRealtime(holdDuration);
        yield return StartCoroutine(FadeOutText(CMPNAME));
    }

    private IEnumerator FadeInText(TextMeshProUGUI text)
    {
        float elapsedTime = 0f;
        Color color = text.color;
        color.a = 0;
        text.color = color;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            text.color = color;
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        color.a = 1;
        text.color = color;
    }

    private IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        float elapsedTime = 0f;
        Color color = text.color;
        color.a = 1;
        text.color = color;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            text.color = color;
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        color.a = 0;
        text.color = color;
    }
}
