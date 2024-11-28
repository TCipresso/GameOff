using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AttackMiniGame : MonoBehaviour
{
    [Header("Player Stats")]
    public PlayerStats playerStats;

    [Header("Arrow Prefabs")]
    public GameObject upArrowPrefab;
    public GameObject downArrowPrefab;
    public GameObject leftArrowPrefab;
    public GameObject rightArrowPrefab;

    [Header("Visual Variants")]
    public Sprite hollowUpSprite, filledUpSprite;
    public Sprite hollowDownSprite, filledDownSprite;
    public Sprite hollowLeftSprite, filledLeftSprite;
    public Sprite hollowRightSprite, filledRightSprite;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color failureColor = Color.red;

    [Header("Sounds")]
    public AudioClip[] successSounds;
    public AudioClip failureSound;
    public AudioSource audioSource;

    [Header("UI Components")]
    public Image timerBarImage;
    public float miniGameDuration = 10f;

    [Header("Sequence Settings")]
    public Transform spawnPoint;
    public int sequenceLength = 5;
    public float spawnInterval = 0.5f;

    private float spawnTimer = 0f;
    private int arrowsSpawned = 0;

    private List<GameObject> spawnedArrows = new List<GameObject>();
    private List<string> sequence = new List<string>();
    private int currentProgress = 0;

    private GameObject[] arrowPrefabs;
    private string[] arrowDirections = { "Up", "Down", "Left", "Right" };

    private float remainingTime;
    private int completedSequences = 0;

    private void OnEnable()
    {
        arrowPrefabs = new GameObject[] { upArrowPrefab, downArrowPrefab, leftArrowPrefab, rightArrowPrefab };
        GenerateSequence();
        StartMiniGameTimer();
        completedSequences = 0;
    }

    private void Update()
    {
        // Update timer
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerBar();
        }
        else
        {
            EndMiniGame();
            return;
        }

        // Spawn sequence arrows
        if (arrowsSpawned < sequenceLength)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                SpawnArrow(sequence[arrowsSpawned]);
                spawnTimer = 0f;
                arrowsSpawned++;
            }
        }

        HandleInput();
    }

    private void SpawnArrow(string direction)
    {
        int index = System.Array.IndexOf(arrowDirections, direction);
        if (index >= 0 && index < arrowPrefabs.Length)
        {
            GameObject arrow = Instantiate(arrowPrefabs[index], spawnPoint.position, Quaternion.identity, spawnPoint);
            spawnedArrows.Add(arrow);
        }
    }

    private void GenerateSequence()
    {
        ClearPreviousSequence();
        sequence.Clear();
        arrowsSpawned = 0;
        currentProgress = 0;

        for (int i = 0; i < sequenceLength; i++)
        {
            int randomIndex = Random.Range(0, arrowDirections.Length);
            sequence.Add(arrowDirections[randomIndex]);
        }

        Debug.Log("Sequence Generated: " + string.Join(", ", sequence));
    }

    private void HandleInput()
    {
        if (sequence.Count == 0 || currentProgress >= sequence.Count) return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            CheckInput("Up");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            CheckInput("Left");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            CheckInput("Down");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            CheckInput("Right");
        }
    }

    private void CheckInput(string input)
    {
        if (input == sequence[currentProgress])
        {
            UpdateArrowVisual(currentProgress, true);
            PlaySuccessSound(currentProgress);
            currentProgress++;

            if (currentProgress >= sequence.Count)
            {
                Debug.Log("Sequence Completed!");
                completedSequences++;
                playerStats.AddDamage(5); // Add bonus damage for the completed sequence
                GenerateSequence(); // Start a new sequence
            }
        }
        else
        {
            PlayFailureSound();
            StartCoroutine(HandleFailure());
        }
    }

    private void UpdateArrowVisual(int index, bool isCorrect)
    {
        GameObject arrow = spawnedArrows[index];
        Image arrowImage = arrow.GetComponent<Image>();

        if (isCorrect)
        {
            switch (sequence[index])
            {
                case "Up": arrowImage.sprite = filledUpSprite; break;
                case "Down": arrowImage.sprite = filledDownSprite; break;
                case "Left": arrowImage.sprite = filledLeftSprite; break;
                case "Right": arrowImage.sprite = filledRightSprite; break;
            }
        }
    }

    private IEnumerator HandleFailure()
    {
        foreach (GameObject arrow in spawnedArrows)
        {
            Image arrowImage = arrow.GetComponent<Image>();
            arrowImage.color = failureColor;
        }

        yield return new WaitForSeconds(0.5f);

        foreach (GameObject arrow in spawnedArrows)
        {
            Image arrowImage = arrow.GetComponent<Image>();
            switch (sequence[spawnedArrows.IndexOf(arrow)])
            {
                case "Up": arrowImage.sprite = hollowUpSprite; break;
                case "Down": arrowImage.sprite = hollowDownSprite; break;
                case "Left": arrowImage.sprite = hollowLeftSprite; break;
                case "Right": arrowImage.sprite = hollowRightSprite; break;
            }
            arrowImage.color = normalColor;
        }

        currentProgress = 0;
    }

    private void ClearPreviousSequence()
    {
        foreach (GameObject arrow in spawnedArrows)
        {
            Destroy(arrow);
        }

        spawnedArrows.Clear();
    }

    private void StartMiniGameTimer()
    {
        remainingTime = miniGameDuration;
        UpdateTimerBar();
    }

    private void UpdateTimerBar()
    {
        if (timerBarImage != null)
        {
            float normalizedTime = remainingTime / miniGameDuration;
            timerBarImage.rectTransform.localScale = new Vector3(normalizedTime, 1, 1);
        }
    }

    private void EndMiniGame()
    {
        Debug.Log($"Mini-game ended. Completed sequences: {completedSequences}");

        //playerStats.ResetDamage(); //do not touch this Tommy

        Combat.instance.MiniGameAttackCompleted();
        gameObject.SetActive(false); // Disable the mini-game
    }

    private void PlaySuccessSound(int index)
    {
        if (index < successSounds.Length && audioSource != null)
        {
            audioSource.PlayOneShot(successSounds[index]);
        }
    }

    private void PlayFailureSound()
    {
        if (failureSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(failureSound);
        }
    }
}
