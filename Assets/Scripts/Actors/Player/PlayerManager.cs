using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private bool playerActive = false;

    private PlayerMovement playerMovement;
    private PlayerAttacker playerAttacker;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttacker = GetComponent<PlayerAttacker>();

        TogglePlayer(null, false);
    }

    private void OnEnable()
    {
        BattleManager.OnPlayerToggle += TogglePlayer;
    }

    private void OnDisable()
    {
        BattleManager.OnPlayerToggle -= TogglePlayer;
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
}
