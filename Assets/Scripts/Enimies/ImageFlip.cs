using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Flips a UI Image component at random intervals.
/// </summary>
public class ImageFlip : MonoBehaviour
{
    [Header("Flip Settings")]
    [SerializeField] private float minInterval = 1f;
    [SerializeField] private float maxInterval = 5f;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform not found on the GameObject. Please ensure this is attached to a UI Image object.");
            enabled = false;
            return;
        }

        StartCoroutine(FlipImageAtRandomIntervals());
    }

    /// <summary>
    /// Flips the UI Image horizontally at random intervals.
    /// </summary>
    private IEnumerator FlipImageAtRandomIntervals()
    {
        while (true)
        {
            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            if (rectTransform != null)
            {

                rectTransform.localScale = new Vector3(
                    rectTransform.localScale.x * -1,
                    rectTransform.localScale.y,
                    rectTransform.localScale.z
                );
            }
        }
    }
}
