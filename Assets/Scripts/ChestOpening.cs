using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpening : MonoBehaviour
{
    [SerializeField] GameObject openChest;

    private void OnDisable()
    {
        openChest.SetActive(true);
    }
}
