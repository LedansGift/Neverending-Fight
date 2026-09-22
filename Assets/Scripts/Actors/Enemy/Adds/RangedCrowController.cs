using System.Collections;
using UnityEngine;

public class RangedCrowController : ProjectileEntityController
{
    private int crowProjectileInitialPool = 50;

    [SerializeField]
    private float projectileFireCooldown = 4f;

    [SerializeField]
    private Projectile crowProjectilePrefab;

    [SerializeField]
    private Transform projectileSpawnPoint;

    private void Start()
    {
        if (
            !ProjectileManager.Instance.CheckAreProjectilesInitialised(
                crowProjectilePrefab.gameObject,
                out int amount
            )
        )
        {
            ProjectileManager.Instance.InitialiseProjectileSet(
                crowProjectilePrefab.gameObject,
                crowProjectileInitialPool
            );
        }
    }

    protected override IEnumerator StartEntityAction()
    {
        yield return base.StartEntityAction();

        StartCoroutine(FireCrowProjectile());
    }

    private IEnumerator FireCrowProjectile()
    {
        ProjectileManager.Instance.SpawnProjectile(
            crowProjectilePrefab.gameObject,
            projectileSpawnPoint.position,
            projectileSpawnPoint.forward
        );

        yield return new WaitForSeconds(projectileFireCooldown);

        StartCoroutine(FireCrowProjectile());
    }
}
