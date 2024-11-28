using System.Collections;
using UnityEngine;

public class LineAnimator : MonoBehaviour
{
    public GameObject[] lineAnimations; // Array of line GameObjects with attached animations
    public GameObject loadingScreen;
    public GameObject ddrMinigame;
    public float delayBetweenAnimations = 1.0f; // Delay between animations, adjust as needed

    void OnEnable()
    {
        StartCoroutine(ActivateLineAnimationsSequentially());
    }

    void OnDisable()
    {
        // Disable all objects in the lineAnimations array
        foreach (GameObject line in lineAnimations)
        {
            if (line != null)
            {
                line.SetActive(false);
            }
        }
    }

    IEnumerator ActivateLineAnimationsSequentially()
    {
        foreach (GameObject line in lineAnimations)
        {
            line.SetActive(true); // Enable the line, which should start its animation
            yield return new WaitForSecondsRealtime(delayBetweenAnimations); // Wait for the animation to complete
        }

        ShowLoadingScreen();
    }

    void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(StartMiniGameAfterDelay());
    }

    IEnumerator StartMiniGameAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2); // Delay before starting the mini-game
        loadingScreen.SetActive(false);
        ddrMinigame.SetActive(true);
    }
}
