using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefendMiniGame : MonoBehaviour
{
    [Header("Player Settings")]
    public RectTransform playerImage; // Reference to the player's image (RectTransform)
    public float moveSpeed = 300f;    // Speed of movement

    [Header("Boundary Settings")]
    public RectTransform gameArea; 

    private Vector2 inputDirection;

    void Update()
    {
        HandleInput();
        MovePlayer();
        ConstrainMovement();
    }

    private void HandleInput()
    {
        // Get input direction
        inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MovePlayer()
    {
        // Move the player image based on input
        if (playerImage != null)
        {
            Vector3 movement = new Vector3(inputDirection.x, inputDirection.y, 0) * moveSpeed * Time.deltaTime;
            playerImage.localPosition += movement;
        }
    }

    private void ConstrainMovement()
    {
        // Ensure the player's image stays within the game area
        if (playerImage != null && gameArea != null)
        {
            Vector3 playerPos = playerImage.localPosition;
            Vector3 minBounds = gameArea.rect.min;
            Vector3 maxBounds = gameArea.rect.max;

            // Clamp position to stay within the game area's bounds
            playerImage.localPosition = new Vector3(
                Mathf.Clamp(playerPos.x, minBounds.x, maxBounds.x),
                Mathf.Clamp(playerPos.y, minBounds.y, maxBounds.y),
                playerPos.z
            );
        }
    }
}
