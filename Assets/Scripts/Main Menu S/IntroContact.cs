using System.Collections;
using UnityEngine;

public class IntroContact : MonoBehaviour
{
    public GameObject comp;
    public GameObject Off;
    public GameObject StartUp;
    public float delay = 2f;
    public float offDisableDelay = 1f; // Duration Off object stays active

    public void StartSequence2()
    {
        StartCoroutine(ExecuteAfterDelay());
    }

    private IEnumerator ExecuteAfterDelay()
    {
        // Use WaitForSecondsRealtime for precise timing
        yield return new WaitForSecondsRealtime(delay);

        if (comp != null)
        {
            comp.SetActive(false);
        }

        if (Off != null)
        {
            Off.SetActive(true);

            // Wait for 1 second in real time before disabling Off
            yield return new WaitForSecondsRealtime(offDisableDelay);

            Off.SetActive(false);
        }

        if (StartUp != null)
        {
            StartUp.SetActive(false);
        }
    }
}
