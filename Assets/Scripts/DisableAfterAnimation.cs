using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Literally just for use within an animation to 
/// disable GameObject with animation.
/// </summary>
public class DisableAfterAnimation : MonoBehaviour
{
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
