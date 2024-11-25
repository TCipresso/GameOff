using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wait is to be used by <see cref="Waiter"/>s. Allows classes
/// that cannot use coroutines such as <see cref="ScriptableObject"/>s
/// to wait.
/// </summary>
public class Wait : MonoBehaviour
{
    public static Wait instance { get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    /// <summary>
    /// Waits for the desired seconds.
    /// </summary>
    /// <param name="duration">Number of seconds to wait.</param>
    /// <param name="waiter">Caller.</param>
    /// <returns>Reference to started coroutine.</returns>
    public Coroutine WaitForSeconds(float duration, Waiter waiter)
    {
        return StartCoroutine(WaitForSecondsCoroutine(duration, waiter));
    }

    IEnumerator WaitForSecondsCoroutine(float duration, Waiter waiter)
    {
        yield return new WaitForSecondsRealtime(duration);
        waiter.WaitComplete();
    }

    /// <summary>
    /// Waits for the desired frames.
    /// </summary>
    /// <param name="frames">Number of frames to wait.</param>
    /// <param name="waiter">Caller.</param>
    /// <returns>Reference to started coroutine.</returns>
    public Coroutine WaitForFrames(int frames, Waiter waiter)
    {
        return StartCoroutine(WaitForFramesCoroutine(frames, waiter));
    }

    IEnumerator WaitForFramesCoroutine(int frames, Waiter waiter)
    {
        for(int i = 0; i < frames; i++)
        {
            yield return null;
        }
        waiter.WaitComplete();
    }

    /// <summary>
    /// Stops the wait of a coroutine.
    /// </summary>
    /// <param name="coroutineInstance">Coroutine instance of wanted waiting period.</param>
    public void StopWait(Coroutine coroutineInstance)
    {
        StopCoroutine(coroutineInstance);
    }
}
