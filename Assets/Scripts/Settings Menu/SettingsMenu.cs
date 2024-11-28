using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] List<GameObject> visibleObjects = new List<GameObject>();
    bool isOpen = false;

    public void OpenMenu()
    {
        isOpen = true;
        for(int i = 0; i < visibleObjects.Count; i++)
        {
            visibleObjects[i].SetActive(true);
        }
    }

    public void CloseMenu()
    {
        isOpen = false;
        for(int i = 0; i < visibleObjects.Count; i++)
        {
            visibleObjects[i].SetActive(false);
        }
    }

    public void ToggleMenu()
    {
        if (isOpen) CloseMenu();
        else OpenMenu();
    }
}
