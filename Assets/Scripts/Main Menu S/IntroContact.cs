using System.Collections;
using UnityEngine;

public class IntroContact : MonoBehaviour
{
    public GameObject comp;
    public GameObject Off;
    public float delay = 2f;

    public void StartSequence2()
    {
        StartCoroutine(ExecuteAfterDelay());
    }

    private IEnumerator ExecuteAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        // Disable the comp object
        if (comp != null)
        {
            comp.SetActive(false);
        }

        // Enable the Off object
        if (Off != null)
        {
            Off.SetActive(true);
        }
    }
}
