using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField]
    private BossHealth bossHealth;

    private void Start()
    {
        bossHealth.InitialiseHealth(100);
    }
}
