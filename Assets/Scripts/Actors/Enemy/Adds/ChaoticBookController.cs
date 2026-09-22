using System.Collections;
using UnityEngine;

public class ChaoticBookController : ProjectileEntityController
{
    private int chaoticProjectileInitialPool = 200;

    private float SPAWN_POSITION_VARIANCE = 0.1f;
    private float SPAWN_ROTATION_VARIANCE = 0.2f;

    [SerializeField]
    private float projectileFireCooldown = 0.075f;

    [SerializeField]
    private Projectile chaoticProjectilePrefab;

    [SerializeField]
    private Transform projectileSpawnPoint;

    private void Start()
    {
        if (
            !ProjectileManager.Instance.CheckAreProjectilesInitialised(
                chaoticProjectilePrefab.gameObject,
                out int amount
            )
        )
        {
            ProjectileManager.Instance.InitialiseProjectileSet(
                chaoticProjectilePrefab.gameObject,
                chaoticProjectileInitialPool
            );
        }
    }

    protected override IEnumerator StartEntityAction()
    {
        yield return base.StartEntityAction();

        StartCoroutine(FireChaoticProjectile());
    }

    private IEnumerator FireChaoticProjectile()
    {
        Vector3 spawnPosition =
            projectileSpawnPoint.position
            + new Vector3(
                Random.Range(-SPAWN_POSITION_VARIANCE, SPAWN_POSITION_VARIANCE),
                0f,
                Random.Range(-SPAWN_POSITION_VARIANCE, SPAWN_POSITION_VARIANCE)
            );

        Vector3 spawnForward =
            projectileSpawnPoint.forward
            + new Vector3(
                Random.Range(-SPAWN_ROTATION_VARIANCE, SPAWN_ROTATION_VARIANCE),
                0f,
                Random.Range(-SPAWN_ROTATION_VARIANCE, SPAWN_ROTATION_VARIANCE)
            );

        ProjectileManager.Instance.SpawnProjectile(
            chaoticProjectilePrefab.gameObject,
            spawnPosition,
            spawnForward
        );

        yield return new WaitForSeconds(projectileFireCooldown);

        StartCoroutine(FireChaoticProjectile());
    }
}
