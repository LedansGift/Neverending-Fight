using UnityEngine;

public class ProjectileReflective : Projectile
{
    protected override void TryDestroyProjectile()
    {
        Vector3 reflectDirection = PlayerIdentifier.PlayerTransform
            .GetComponent<PlayerMovement>()
            .GetPlayerLookDirection();

        ProjectileManager.Instance.SpawnReflectedProjectile(transform.position, reflectDirection);

        base.TryDestroyProjectile();
    }
}
