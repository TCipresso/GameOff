using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMainMenuButton : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}
