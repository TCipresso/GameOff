using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Required for TextMeshPro components

/// <summary>
/// Handles the turn-based combat system.
/// </summary>
public class Combat : MonoBehaviour
{
    public static Combat instance;

    [Header("Enemy Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    private GameObject currentEnemy;
    private Enemy currentEnemyScript;

    [Header("Player Settings")]
    [SerializeField] private PlayerStats playerStats;

    [Header("UI Components")]
    [SerializeField] private GameObject playerInputFieldObject;

    [Header("Combat State")]
    private bool combatActive = false;
    private Encounter currentEncounter;

    [Header("Mini Game")]
    public GameObject ddrMinigame;
    private bool miniGameComplete = false;

    private enum CombatState
    {
        Intro,
        SpeedCheck,
        PlayerTurn,
        EnemyTurn,
        WaitForMiniGame,
        EndCombat
    }

    private CombatState currentState;

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

    private void UpdateInputFieldState()
    {
        if (playerInputFieldObject != null)
        {
            playerInputFieldObject.SetActive(currentState == CombatState.PlayerTurn);
        }
    }

    public void InitiateCombat(Encounter encounter)
    {
        Debug.Log("Initializing combat...");
        combatActive = true;
        currentEncounter = encounter;

        int enemyIndex = encounter.GetEnemyIndex();
        if (enemyIndex >= 0 && enemyIndex < enemyPrefabs.Count)
        {
            currentEnemy = enemyPrefabs[enemyIndex];
            currentEnemy.SetActive(true);
            currentEnemyScript = currentEnemy.GetComponent<Enemy>();

            if (currentEnemyScript == null)
            {
                Debug.LogError("Enemy GameObject is missing an Enemy script.");
                combatActive = false;
                return;
            }

            Debug.Log($"Engaged in combat with: {currentEnemy.name}");
        }
        else
        {
            Debug.LogError("Invalid enemy index for encounter.");
            combatActive = false;
            return;
        }

        currentState = CombatState.Intro;
        UpdateInputFieldState();
        TextOutput.instance.Print(currentEncounter.description, OutputCarrot.QUESTION);
        float delay = Mathf.Min(currentEncounter.description.Length * 0.05f, 3.0f);
        Invoke(nameof(DoSpeedCheck), delay);
    }

    private void DoSpeedCheck()
    {
        if (playerStats.speed >= currentEnemyScript.speed)
        {
            currentState = CombatState.PlayerTurn;
            TextOutput.instance.Print("Player's Turn: Type 'attack', 'defend', or 'investigate'");
        }
        else
        {
            currentState = CombatState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
        UpdateInputFieldState();
    }

    public void HandlePlayerInput(string input)
    {
        if (!combatActive || currentState != CombatState.PlayerTurn) return;

        switch (input.ToLower())
        {
            case "attack":
                PlayerAttack();
                break;
            case "defend":
                PlayerDefend();
                break;
            case "investigate":
                Investigate(currentEncounter);
                break;
            default:
                TextOutput.instance.Print("Invalid command. Please type 'attack', 'defend', or 'investigate'");
                return;
        }

        //if (currentEnemyScript != null && !currentEnemyScript.IsDefeated())
        //{
          //  currentState = CombatState.EnemyTurn;
           // StartCoroutine(EnemyTurn());
     //   }
    }

    private void PlayerAttack()
    {
        ddrMinigame.SetActive(true);
        currentState = CombatState.WaitForMiniGame;
        UpdateInputFieldState();
        StartCoroutine(WaitForMiniGameCompletion());
    }

    private IEnumerator WaitForMiniGameCompletion()
    {
        yield return new WaitUntil(() => miniGameComplete);
        ApplyDamageAndCheckForEnemyDefeat();
    }

    public void MiniGameCompleted()
    {
        Debug.Log("Combat has received the mini-game completion signal.");
        miniGameComplete = true;
    }

    private void ApplyDamageAndCheckForEnemyDefeat()
    {
        if (currentEnemyScript != null)
        {
            currentEnemyScript.TakeDamage(playerStats.dmg);
            TextOutput.instance.Print($"You attack! The enemy took {playerStats.dmg} damage. Enemy HP: {currentEnemyScript.GetHealth()}");

            if (currentEnemyScript.IsDefeated())
            {
                TextOutput.instance.Print("Enemy defeated! You win the encounter!");
                EndCombat(true);
            }
            else
            {
                currentState = CombatState.EnemyTurn;
                StartCoroutine(EnemyTurn());
            }
        }
        miniGameComplete = false;  // Reset the flag for the next usage
    }

    private void PlayerDefend()
    {
        TextOutput.instance.Print("You defend! The enemy's next attack will deal reduced damage.");
        playerStats.isDefending = true;
        UpdateInputFieldState();
    }

    private void Investigate(Encounter encounter)
    {
        List<string> dialogues = encounter.GetAttackDialogues();
        string randomHint = dialogues[Random.Range(0, dialogues.Count)];
        TextOutput.instance.Print($"You investigate: {randomHint}");
        UpdateInputFieldState();
    }

    private IEnumerator EnemyTurn()
    {
        UpdateInputFieldState();
        yield return new WaitForSeconds(1);
        TextOutput.instance.Print("Enemy's Turn");
        UpdateInputFieldState();

        if (currentEnemyScript != null)
        {
            int enemyAttack = currentEnemyScript.GetAttack();
            int damage = playerStats.isDefending ? Mathf.CeilToInt(enemyAttack * 0.5f) : enemyAttack;

            playerStats.TakeDamage(damage);
            TextOutput.instance.Print($"Enemy attacks! You took {damage} damage. Your HP: {playerStats.playerHP}");

            playerStats.isDefending = false;

            if (playerStats.playerHP <= 0)
            {
                TextOutput.instance.Print("You were defeated! Game over.");
                currentState = CombatState.EndCombat;
                EndCombat(false);
                yield break;
            }
        }
        UpdateInputFieldState();
        yield return new WaitForSeconds(1.5f);
        currentState = CombatState.PlayerTurn;
        TextOutput.instance.Print("Player's Turn ACTIONS: 'ATTACK'     'DEFEND'     'INVESTIGATE'");
        UpdateInputFieldState();
    }

    private void EndCombat(bool playerWon)
    {
        combatActive = false;
        TextOutput.instance.Print(playerWon ? "You are victorious!" : "Combat lost!");

        if (currentEnemy != null)
        {
            currentEnemy.SetActive(false);
            currentEnemy = null;
        }

        currentState = CombatState.EndCombat;
        UpdateInputFieldState();
        playerStats.isDefending = false;
        currentEncounter = null;
    }

    public bool IsCombatActive()
    {
        return combatActive;
    }
}
