using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefendMiniGame : MonoBehaviour
{
    public static DefendMiniGame instance; // Singleton instance

    [Header("Player Settings")]
    public RectTransform playerImage; // Player image reference
    public float moveSpeed = 300f;    // Movement speed

    [Header("Boundary Settings")]
    public RectTransform gameArea;   // The game area boundary

    [Header("Mini-game Settings")]
    public float gameDuration = 10f; // Duration of the mini-game in seconds
    private bool isGameRunning = false;

    [Header("Lineers")]
    public List<GameObject> Lineers; // List of objects to disable after the mini-game

    [Header("UI Components")]
    public Image timerBarImage; // Timer bar as an image

    private Vector2 inputDirection;
    private float remainingTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        isGameRunning = true;
        remainingTime = gameDuration; // Initialize timer
        UpdateTimerBar(); // Initialize the timer bar UI

        TextOutput.instance.Print("Enemy's Turn: Defend yourself!");
        StartCoroutine(MiniGameTimer());
    }

    private void OnDisable()
    {
        isGameRunning = false;
    }

    void Update()
    {
        if (!isGameRunning) return;

        // Update the timer
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerBar();
        }
        else
        {
            EndMiniGame(true); // Timer ran out, player succeeded
        }

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
        if (playerImage != null)
        {
            Vector3 movement = new Vector3(inputDirection.x, inputDirection.y, 0) * moveSpeed * Time.deltaTime;
            playerImage.localPosition += movement;
        }
    }

    private void ConstrainMovement()
    {
        if (playerImage != null && gameArea != null)
        {
            Vector3 playerPos = playerImage.localPosition;
            Vector3 minBounds = gameArea.rect.min;
            Vector3 maxBounds = gameArea.rect.max;

            // Clamp player position to game area
            playerImage.localPosition = new Vector3(
                Mathf.Clamp(playerPos.x, minBounds.x, maxBounds.x),
                Mathf.Clamp(playerPos.y, minBounds.y, maxBounds.y),
                playerPos.z
            );
        }
    }

    private IEnumerator MiniGameTimer()
    {
        yield return new WaitForSecondsRealtime(gameDuration);

        if (isGameRunning)
        {
            TextOutput.instance.Print("> Player dodged the attack. Zero damage taken.");
            TextOutput.instance.Print("> COUNTER ATTACK ENGAGED +30 Damage next attack");

            EndMiniGame(true); // Mini-game succeeded
        }
    }

    /// <summary>
    /// Ends the mini-game, handling success or failure.
    /// </summary>
    /// <param name="success">True if the player succeeded, false if they failed.</param>
    public void EndMiniGame(bool success)
    {
        isGameRunning = false;

        // Disable Lineers
        foreach (GameObject lineer in Lineers)
        {
            if (lineer != null)
            {
                lineer.SetActive(false);
            }
        }

        if (success)
        {
            Debug.Log("Counter Attack Enabled! +30 damage to the player's next attack.");
            PlayerStats.instance.AddDamage(30); // Add bonus damage
        }
        else
        {
            int halfDamage = Mathf.CeilToInt(Combat.instance.GetEnemyAttack() * 0.5f);
            PlayerStats.instance.TakeDamage(halfDamage); // Apply half-damage
        }

        // Notify Combat that the mini-game has completed
        Combat.instance.MiniGameDefendCompleted();
        // Disable the mini-game object
        gameObject.SetActive(false);
    }

    private void UpdateTimerBar()
    {
        if (timerBarImage != null)
        {
            float normalizedTime = remainingTime / gameDuration;
            timerBarImage.rectTransform.localScale = new Vector3(normalizedTime, 1, 1);
        }
    }
}
