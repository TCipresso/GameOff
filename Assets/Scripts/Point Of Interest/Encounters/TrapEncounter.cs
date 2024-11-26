using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TrapEncounter is a noncombat <see cref="Encounter"/>.
/// Saving throw type encounter. Player must pass minigame in order to proceed. Take damage, lose speed.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Encounters/Trap Encounter")]
public class TrapEncounter : Encounter, Waiter
{
    [Header("Trap Stats")]
    [Tooltip("Gives the player some time to read before the trap starts.")]
    [SerializeField] float trapDelay = 3f;
    [SerializeField] int speedLoss;
    [SerializeField] int damage;

    [Header("Trap Flavor")]
    [TextArea(3, 10)]
    [SerializeField] string failureResponse;
    [TextArea(3, 10)]
    [SerializeField] string successResponse;
    bool isSetUp = false;

    private void OnDisable()
    {
        isSetUp = false;
    }

    public override string GetDescription()
    {
        if (isSetUp) return base.GetDescription();
        
        isSetUp = true;
        Wait.instance.WaitForSeconds(trapDelay, this);
        return base.GetDescription();
    }

    public override void LeaveEncounter()
    {
        isSetUp = false;
        base.LeaveEncounter();
    }

    public override void CompleteMinigame(MinigameStatus gameResult)
    {
        switch (gameResult)
        {
            case MinigameStatus.MISSBUMP:
            case MinigameStatus.LOST:
                TrapFail();
                break;
            case MinigameStatus.FISTBUMP:
            case MinigameStatus.WIN:
                TrapSuccess();
                break;
            default:
                TextOutput.instance.Print($"Unknown game status {gameResult}");
                break;
        }
    }

    /// <summary>
    /// Failed escaping trap. Lose speed and take damage.
    /// </summary>
    private void TrapFail()
    {
        PlayerStats.instance.TakeDamage(damage);
        PlayerStats.instance.DecreaseSpeed(speedLoss);
        TextOutput.instance.Print(failureResponse);
        LeaveEncounter();
    }

    /// <summary>
    /// Succeeded escaping trap. Lose nothing.
    /// </summary>
    private void TrapSuccess()
    {
        TextOutput.instance.Print(successResponse);
        LeaveEncounter();
    }

    public void WaitComplete()
    {
        PlayerStats.instance.TakeDamage(0);
        StartMinigame();
    }
}
