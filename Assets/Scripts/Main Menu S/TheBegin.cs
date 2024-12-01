using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TheBegin : MonoBehaviour
{
    [SerializeField] private Image panel;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    private void Awake()
    {
        if (panel == null) return;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color initialColor = panel.color;

        while (elapsedTime < fadeDuration)
        {
            float curveValue = fadeCurve.Evaluate(elapsedTime / fadeDuration);
            panel.color = new Color(initialColor.r, initialColor.g, initialColor.b, curveValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        panel.gameObject.SetActive(false);
    }
}
