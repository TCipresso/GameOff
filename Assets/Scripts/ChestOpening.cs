using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ChestOpening is literally just so I can control the chest open
/// animation without completely redoing everything.
/// </summary>
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

    /// <summary>
    /// Allows playing open animation OnDisable.
    /// </summary>
    public void WantToOpen()
    {
        wantToOpen = true;
    }

    /// <summary>
    /// Disallows playing open animation OnDisable.
    /// </summary>
    public void DoNotWantToOpen()
    {
        wantToOpen = false;
    }
}
