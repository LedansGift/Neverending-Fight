using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private bool playerActive = false;

    private Collider playerCollider;

    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerAttacker playerAttacker;
    private PlayerTimepiece playerTimepiece;

    public EventHandler<bool> OnPlayerActive;

    private void Awake()
    {
        playerCollider = GetComponent<Collider>();
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerTimepiece = GetComponent<PlayerTimepiece>();
    }

    private void Start()
    {
        TogglePlayer(null, false);
    }

    private void OnEnable()
    {
        BattleManager.OnPlayerToggle += TogglePlayer;
        BattleManager.OnWeakenPlayer += WeakenPlayer;
        PlayerGlaive.OnGlaiveSpecial += TryToggleCollider;
        RestartManager.OnResetPhase += ResetPlayer;
        BossFormManager.OnPhaseFinished += HandlePlayerFinishPhase;
        BossFormManager.OnNewPhaseStart += HandlePlayerPhaseStart;
        playerHealth.OnDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        BattleManager.OnPlayerToggle -= TogglePlayer;
        BattleManager.OnWeakenPlayer -= WeakenPlayer;
        PlayerGlaive.OnGlaiveSpecial -= TryToggleCollider;
        RestartManager.OnResetPhase -= ResetPlayer;
        BossFormManager.OnPhaseFinished -= HandlePlayerFinishPhase;
        BossFormManager.OnNewPhaseStart -= HandlePlayerPhaseStart;
        playerHealth.OnDeath -= HandlePlayerDeath;
    }

    public void TogglePlayer(object sender, bool toggle)
    {
        // if (playerActive == toggle)
        // {
        //     return;
        // }

        playerActive = toggle;

        playerCollider.enabled = playerActive;
        playerMovement.ToggleCanMove(playerActive);
        playerAttacker.ToggleCanAttack(playerActive);
        playerMovement.SetWeaponModifier();

        OnPlayerActive?.Invoke(this, playerActive);

        UIFadeController.ToggleUI(playerActive);
    }

    public void SetMouseTarget(Transform targetTransform)
    {
        playerMovement.SetMouseTarget(targetTransform);
        playerAttacker.SetMouseTarget(targetTransform);
    }

    public void WeakenPlayer()
    {
        playerHealth.SetWeakenedHealth();
    }

    private void HandlePlayerDeath()
    {
        if (!playerActive)
        {
            return;
        }

        TogglePlayer(this, false);

        playerTimepiece.DecrementRetries();
    }

    private void TryToggleCollider(object sender, bool specialActive)
    {
        if (!playerActive)
        {
            return;
        }

        playerCollider.enabled = !specialActive;
    }

    private void ResetPlayer()
    {
        playerAttacker.ResetWeapons();
        playerHealth.RevivePlayer();
        playerMovement.ResetToSavedPosition();
        TogglePlayer(this, true);
    }

    private void HandlePlayerFinishPhase()
    {
        TogglePlayer(this, false);
    }

    private void HandlePlayerPhaseStart()
    {
        Debug.Log("Player Phase Started");

        playerMovement.SaveCurrentPosition();
        playerTimepiece.ResetPlayerRetries();
        ResetPlayer();

        //playerMovement.TryResolveGroundCheck();
    }
}
