using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public GameObject gameOverScreen;

    [Header("Combat State")]
    private bool combatActive = false;
    private Encounter currentEncounter;

    [Header("Mini Games")]
    public GameObject ddrMinigame;    
    public GameObject defendMinigame; 
    public GameObject Loading;

    [Header("Music")]
    [SerializeField] AudioSource battleMusic;
    [SerializeField] AudioSource regularMusic;

    private bool AttMiniGameComplete = false; 
    private bool DefMiniGameComplete = false; 
    public AudioSource attackSoundSource;

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

    public void SetPlayerTurn()
    {
        if (currentState == CombatState.EndCombat) return; 
        currentState = CombatState.PlayerTurn;

        Debug.Log("Player's Turn: Choose your next action.");
        TextOutput.instance.Print("Player's Turn ACTIONS: 'ATTACK'     'DEFEND'     'INVESTIGATE'");
        UpdateInputFieldState();
    }

    public void InitiateCombat(Encounter encounter)
    {
        Debug.Log("Initializing combat...");
        combatActive = true;
        currentEncounter = encounter;

        regularMusic.Pause();
        battleMusic.Play();

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
        float delay = Mathf.Min(currentEncounter.description.Length * 0.05f, 3.0f);
        Invoke(nameof(DoSpeedCheck), delay);
    }

    private void DoSpeedCheck()
    {
        if (currentState == CombatState.EndCombat) return;

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

        if (CheatCodeManager.instance.TryActivateCheat(input))
        {
            Debug.Log($"Cheat code '{input}' attempted!");
            return;
        }

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
                if (currentState != CombatState.EndCombat)
                {
                    TextOutput.instance.Print("Invalid command. Please type 'attack', 'defend', or 'investigate'");
                }
                return;
        }
    }

    private void PlayerAttack()
    {
        ddrMinigame.SetActive(true);
        AttMiniGameComplete = false;
        currentState = CombatState.WaitForMiniGame;
        UpdateInputFieldState();
        StartCoroutine(WaitForAttackCompletion());
    }

    private void PlayerDefend()
    {
        Debug.Log("Defend mini-game started!");
        defendMinigame.SetActive(true);
        DefMiniGameComplete = false;
        currentState = CombatState.WaitForMiniGame;
        UpdateInputFieldState();
        StartCoroutine(WaitForDefendCompletion());
    }

    private IEnumerator WaitForAttackCompletion()
    {
        yield return new WaitUntil(() => AttMiniGameComplete);
        CompleteAttackAction();
    }

    private IEnumerator WaitForDefendCompletion()
    {
        yield return new WaitUntil(() => DefMiniGameComplete);
        CompleteDefendAction();
    }

    public void MiniGameAttackCompleted()
    {
        Debug.Log("Attack mini-game completed!");
        Loading.SetActive(false);
        AttMiniGameComplete = true;
    }

    public void MiniGameDefendCompleted()
    {
        Debug.Log("Defend mini-game completed!");
        Loading.SetActive(false);
        DefMiniGameComplete = true;
    }

    private void CompleteAttackAction()
    {
        ApplyDamageAndCheckForEnemyDefeat();
        ddrMinigame.SetActive(false);

        if (combatActive)
        {
            SetEnemyTurn();
        }
        else
        {
            Debug.Log("Combat has ended; not transitioning to enemy turn.");
        }
    }


    private void CompleteDefendAction()
    {
        ApplyDefense();
        defendMinigame.SetActive(false);

        if (combatActive)
        {
            SetPlayerTurn();
        }
        else
        {
            Debug.Log("Combat has ended; not transitioning to player's turn.");
        }
    }

    public void SetCombatStateToEnd()
    {
        currentState = CombatState.EndCombat;
        combatActive = false;
        Debug.Log("Combat state set to EndCombat.");
    }



    private void ApplyDamageAndCheckForEnemyDefeat()
    {
        if (currentEnemyScript != null)
        {
            currentEnemyScript.TakeDamage(playerStats.dmg);
            TextOutput.instance.Print($"You attack! The enemy took {playerStats.dmg} damage. Enemy HP: {currentEnemyScript.GetHealth()}");
            PlayAttackSound();
            if (currentEnemyScript.IsDefeated())
            {
                TextOutput.instance.Print("Enemy defeated! You win the encounter!");
                EndCombat(true);
                return;
            }
        }
    }

    private void PlayAttackSound()
    {
        if (attackSoundSource != null)
        {
            attackSoundSource.Play();
        }
    }

    private void ApplyDefense()
    {
        Debug.Log("Defense applied!");
        playerStats.isDefending = true;
    }

    private void SetEnemyTurn()
    {
        currentState = CombatState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {

        UpdateInputFieldState();
        yield return new WaitForSecondsRealtime(3);
        TextOutput.instance.Print("Enemy's Turn");
        playerStats.ResetDamage();
        yield return new WaitForSecondsRealtime(3);

        if (currentEnemyScript != null)
        {
            int damage = playerStats.isDefending ? Mathf.CeilToInt(currentEnemyScript.GetAttack() * 0.5f) : currentEnemyScript.GetAttack();
            playerStats.TakeDamage(damage);
            TextOutput.instance.Print($"Enemy attacks! You took {damage} damage. Your HP: {playerStats.playerHP}");
            playerStats.isDefending = false;
        }

        yield return new WaitForSecondsRealtime(1.5f);
        SetPlayerTurn();
    }

    private void Investigate(Encounter encounter)
    {

        string randomHint = encounter.GetAttackDialogues()[Random.Range(0, encounter.GetAttackDialogues().Count)];
        TextOutput.instance.Print($"You investigate: {randomHint}");
        UpdateInputFieldState();
    }

    public void EndCombat(bool playerWon)
    {
        combatActive = false;
        battleMusic.Stop();
        regularMusic.Play();
        
        TextOutput.instance.Print(playerWon ? "You are victorious!" : "Combat lost!");

        if (!playerWon && gameOverScreen != null)
        {
            Debug.Log("Player has been defeated. Triggering GameOver screen.");
            gameOverScreen.SetActive(true);
        }

        if (currentEnemy != null)
        {
            currentEnemy.SetActive(false);
            currentEnemy = null;
        }

        currentState = CombatState.EndCombat;
        PlayerStats.instance.ResetStats();
        ResetEnemyStats();
        UpdateInputFieldState();
        playerStats.isDefending = false;
        currentEncounter.LeaveEncounter();
        currentEncounter = null;

        playerInputFieldObject.SetActive(true);
    }

    public void ResetEnemyStats()
    {
        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            if (enemyPrefab.TryGetComponent<Enemy>(out Enemy enemyScript))
            {
                enemyScript.ResetStats();
            }
        }
    }


    private IEnumerator ClearTextOutputAfterDelay()
    {
        yield return new WaitForSecondsRealtime(5f);
        TextOutput.instance.Clear();
    }

    public bool IsCombatActive()
    {
        return combatActive;
    }

    public int GetEnemyAttack()
    {
        return currentEnemyScript != null ? currentEnemyScript.GetAttack() : 0;
    }
}
