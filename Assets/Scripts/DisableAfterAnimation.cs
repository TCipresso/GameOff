using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterAnimation : MonoBehaviour
{
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
