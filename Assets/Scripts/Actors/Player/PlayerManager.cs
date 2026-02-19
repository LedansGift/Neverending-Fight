using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private bool playerActive = false;
    private int playerRetries = 2;

    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerAttacker playerAttacker;

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
        playerHealth.OnDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        BattleManager.OnPlayerToggle -= TogglePlayer;
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
            return;
        }

        playerRetries--;
        OnNewPlayerRetries?.Invoke(this, playerRetries);

        TogglePlayer(this, true);

        // Else reset to beginning of phase (event invoke, below handled by various script)
        //Gradual time stop
        //Clock graphic rewinds
        //Reset everything necessary
        //Toggle Player on by battle manager
        //Revive player by battle manager
    }
}
