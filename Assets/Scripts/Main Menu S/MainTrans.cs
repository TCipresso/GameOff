using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTrans : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject liveUI;
    [SerializeField] private GameObject MainMenuMusic;

    public void ToggleUI()
    {
        if (mainMenuUI != null) mainMenuUI.SetActive(false);
        if (liveUI != null) liveUI.SetActive(true);
        if (mainMenuUI != null) MainMenuMusic.SetActive(false);
    }

    public void TransitionToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
