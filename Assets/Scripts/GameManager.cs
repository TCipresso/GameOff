using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PointOfInterest currentPOI;
    public static GameManager instance { private set; get; }
    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
    }

    public string GetCurrentPOIDesc()
    {
        return currentPOI.GetDescription();
    }

    public Sprite GetCurrentPOIImage()
    {
        return currentPOI.GetImage();
    }
}
