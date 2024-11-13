using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderBox : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite defaultImage;
    public static RenderBox instance;

    private void Awake()
    {
        if (instance == null) Destroy(gameObject);
        instance = this;
    }

    public Sprite GetDefaultImage()
    {
        return defaultImage;
    }

    public void UpdateRender(Sprite newImage)
    {
        image.sprite = newImage;
    }
}
