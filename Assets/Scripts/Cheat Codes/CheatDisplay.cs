using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheatDisplay : MonoBehaviour
{
    public TMP_Text cheatsDisplay; // Reference to the TextMeshPro text component
    public float typingSpeed = 0.05f; // Speed for the typewriter effect
    private CheatCodeManager cheatCodeManager;

    private void OnEnable()
    {
        if (cheatCodeManager == null)
        {
            cheatCodeManager = FindObjectOfType<CheatCodeManager>();
        }

        if (cheatCodeManager != null)
        {
            StartCoroutine(TypeOutCheats());
        }
    }

    private void OnDisable()
    {
        cheatsDisplay.text = "";
    }

    private IEnumerator TypeOutCheats()
    {
        cheatsDisplay.text = "";
        List<CheatCode> cheats = cheatCodeManager.GetDiscoveredCheats(); // Updated to GetDiscoveredCheats

        foreach (CheatCode cheat in cheats)
        {
            foreach (char letter in cheat.cheatName + "\n")
            {
                cheatsDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
    }
}
