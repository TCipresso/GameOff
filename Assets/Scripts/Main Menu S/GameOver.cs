using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    [Header("References")]
    public GameObject oldAudio;
    public GameObject mainGame;
    public GameObject GameManager;

    private void OnEnable()
    {
        if (oldAudio != null) oldAudio.SetActive(false);
        if (mainGame != null) mainGame.SetActive(false);
        if (GameManager != null) GameManager.SetActive(false);

        Debug.Log("GameOver triggered: OldAudio and MainGame have been disabled.");
    }

    /// <summary>
    /// Loads the Main Menu scene.
    /// </summary>
    public void MainMenu()
    {
        Debug.Log("Returning to Main Menu...");
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Restarts the current scene.
    /// </summary>
    public void Restart()
    {
        Debug.Log("Restarting current scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
