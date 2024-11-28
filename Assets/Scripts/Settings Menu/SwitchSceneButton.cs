using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneButton : MonoBehaviour
{
    [SerializeField] string targetScene;

    public void SwitchScene()
    {
        SceneManager.LoadScene(targetScene);
    }
}
