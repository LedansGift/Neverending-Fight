using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField]
    private BossHealth bossHealth;

    [SerializeField]
    private BossCombatManager bossCombatManager;

    private void Start()
    {
        bossHealth.InitialiseHealth(100);
        bossCombatManager.StartBossCombat();
    }
}
