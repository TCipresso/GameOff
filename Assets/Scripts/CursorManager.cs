using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public Texture2D customCursor;
    public Texture2D hoverCursor;
    public Texture2D redClickCursor;
    public Image uiCursorImage;
    public Sprite customCursorSprite;
    public Sprite hoverCursorSprite;
    public Sprite redClickSprite;
    public Camera mainCamera;

    private bool isHoveringButton = false;

    private void Start()
    {
        Cursor.visible = false;
        ChangeCursor(customCursorSprite);
    }

    private void Update()
    {
        Vector3 cursorWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
        uiCursorImage.transform.position = new Vector3(cursorWorldPosition.x, cursorWorldPosition.y, uiCursorImage.transform.position.z);

        if (isHoveringButton)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ChangeCursor(redClickSprite);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ChangeCursor(hoverCursorSprite);
            }
        }
        else
        {
            ChangeCursor(customCursorSprite);
        }
    }

    public void ChangeCursor(Sprite newCursor)
    {
        uiCursorImage.sprite = newCursor;
    }

    public void OnButtonHoverEnter()
    {
        isHoveringButton = true;
        ChangeCursor(hoverCursorSprite);
    }

    public void OnButtonHoverExit()
    {
        isHoveringButton = false;
        ChangeCursor(customCursorSprite);
    }
}
