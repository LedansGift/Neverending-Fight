using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private bool playerActive = false;
    private int playerRetries = 2;

    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerAttacker playerAttacker;

    public static Action OnNoMoreRetries;
    public static EventHandler<int> OnNewPlayerRetries;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttacker = GetComponent<PlayerAttacker>();

        playerRetries = 2;

        TogglePlayer(null, false);
    }

    private void OnEnable()
    {
        BattleManager.OnPlayerToggle += TogglePlayer;
        RestartManager.OnResetPhase += ResetPlayer;
        BossFormManager.OnPhaseFinished += HandlePlayerFinishPhase;
        playerHealth.OnDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        BattleManager.OnPlayerToggle -= TogglePlayer;
        RestartManager.OnResetPhase -= ResetPlayer;
        BossFormManager.OnPhaseFinished -= HandlePlayerFinishPhase;
        playerHealth.OnDeath -= HandlePlayerDeath;
    }

    public void TogglePlayer(object sender, bool toggle)
    {
        if (playerActive == toggle)
        {
            return;
        }

        playerActive = toggle;

        playerMovement.ToggleCanMove(playerActive);
        playerAttacker.ToggleCanAttack(playerActive);
        playerMovement.SetWeaponModifier();
    }

    public void SetMouseTarget(Transform targetTransform)
    {
        playerMovement.SetMouseTarget(targetTransform);
        playerAttacker.SetMouseTarget(targetTransform);
    }

    private void HandlePlayerDeath()
    {
        TogglePlayer(this, false);

        if (playerRetries <= 0)
        {
            Debug.Log("GAME OVER");
            OnNoMoreRetries?.Invoke();
            return;
        }

        playerRetries--;
        OnNewPlayerRetries?.Invoke(this, playerRetries);
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
        //Make player invincible
        //Relinquish playere control
    }
}
