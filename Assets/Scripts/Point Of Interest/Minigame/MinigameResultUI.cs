using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MinigameResultUI is a singleton UI element to show the resulting <see cref="MinigameStatus"/> of a <see cref="Minigame"/>
/// </summary>
public class MinigameResultUI : MonoBehaviour
{
    public static MinigameResultUI instance { get; private set; }
    [SerializeField] GameObject win;
    [SerializeField] GameObject lost;
    [SerializeField] float duration = 2f;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
    }

    /// <summary>
    /// Turns off UI element if active. Then turns on the proper UI element
    /// based on <see cref="MinigameStatus"/>.
    /// </summary>
    /// <param name="minigameStatus">The resulting <see cref="MinigameStatus"/> you want to show</param>
    public void ShowResult(MinigameStatus minigameStatus)
    {
        HideResults();
        switch (minigameStatus)
        {
            case MinigameStatus.WIN:
                win.SetActive(true);
                StartCoroutine(Timer());
                break;
            case MinigameStatus.LOST:
                lost.SetActive(true);
                StartCoroutine(Timer());
                break;
            default:
                Debug.LogWarning("Unkown MinigameStatus sent to MinigameResultUI");
                break;
        }
    }

    /// <summary>
    /// Turns off the UI element
    /// </summary>
    public void HideResults()
    {
        StopAllCoroutines();
        if (win.activeSelf) win.SetActive(false);
        if (lost.activeSelf) lost.SetActive(false);
    }

    /// <summary>
    /// Turn off UI element after duration.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/></returns>
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        HideResults();
    }
}
