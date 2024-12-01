using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LiveUI : MonoBehaviour
{
    [System.Serializable]
    public class UIElement
    {
        public GameObject element;
        public float delayBeforeEnable = 1f;
    }

    [SerializeField] private List<UIElement> uiElements;

    private void OnEnable()
    {
        StartCoroutine(EnableUIElements());
    }

    private IEnumerator EnableUIElements()
    {
        foreach (var uiElement in uiElements)
        {
            yield return new WaitForSecondsRealtime(uiElement.delayBeforeEnable);

            if (uiElement.element != null)
            {
                uiElement.element.SetActive(true);
            }
        }

        SceneManager.LoadScene("SampleScene");
    }
}
