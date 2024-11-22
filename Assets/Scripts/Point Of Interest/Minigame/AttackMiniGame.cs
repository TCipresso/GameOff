using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AttackMiniGame : MonoBehaviour
{
    [Header("Arrow Prefabs")]
    public GameObject upArrowPrefab;
    public GameObject downArrowPrefab;
    public GameObject leftArrowPrefab;
    public GameObject rightArrowPrefab;

    [Header("Visual Variants")]
    public Sprite hollowUpSprite, filledUpSprite, failureUpSprite;
    public Sprite hollowDownSprite, filledDownSprite, failureDownSprite;
    public Sprite hollowLeftSprite, filledLeftSprite, failureLeftSprite;
    public Sprite hollowRightSprite, filledRightSprite, failureRightSprite;

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

    private void Start()
    {
        arrowPrefabs = new GameObject[] { upArrowPrefab, downArrowPrefab, leftArrowPrefab, rightArrowPrefab };
        GenerateSequence();
    }

    private void Update()
    {
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

        // Check player input
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
        sequence.Clear();
        spawnedArrows.Clear();
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
            // Correct input: Change arrow to filled
            UpdateArrowVisual(currentProgress, true);
            currentProgress++;

            if (currentProgress >= sequence.Count)
            {
                Debug.Log("Sequence Completed!");
                GenerateSequence(); // Start a new sequence
            }
        }
        else
        {
            // Incorrect input: Flash red and reset sequence
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

    private System.Collections.IEnumerator HandleFailure()
    {
        // Show red flash
        foreach (GameObject arrow in spawnedArrows)
        {
            Image arrowImage = arrow.GetComponent<Image>();
            switch (sequence[spawnedArrows.IndexOf(arrow)])
            {
                case "Up": arrowImage.sprite = failureUpSprite; break;
                case "Down": arrowImage.sprite = failureDownSprite; break;
                case "Left": arrowImage.sprite = failureLeftSprite; break;
                case "Right": arrowImage.sprite = failureRightSprite; break;
            }
        }

        // Wait briefly to show the red flash
        yield return new WaitForSeconds(0.5f);

        // Reset to hollow arrows
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
        }

        currentProgress = 0; // Reset progress
    }
}
