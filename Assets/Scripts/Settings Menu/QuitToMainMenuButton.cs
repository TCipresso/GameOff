using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMainMenuButton : MonoBehaviour
{
    [SerializeField] string targetScene;

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(targetScene);
    }
}
