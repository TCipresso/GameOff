using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpening : MonoBehaviour
{
    public static ChestOpening instance {  get; private set; }
    [SerializeField] GameObject openChest;
    [SerializeField] private bool wantToOpen = false;

    private void Awake()
    {
        instance = this;
    }

    private void OnDisable()
    {
        if(wantToOpen) openChest.SetActive(true);
        wantToOpen = false;
    }

    public void WantToOpen()
    {
        wantToOpen = true;
    }

    public void DoNotWantToOpen()
    {
        wantToOpen = false;
    }
}
