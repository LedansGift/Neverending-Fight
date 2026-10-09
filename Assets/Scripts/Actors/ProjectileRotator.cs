using UnityEngine;

public class ProjectileRotator : ProjectileMover
{
    private float rotationSpeedModifier = 10f;

    protected override void MoveProjectile()
    {
        float resolvedSpeed = speedOverLifetime.Evaluate(projectile.GetLifetimeProgress()) * speed;

        Quaternion newRotation = Quaternion.Euler(
            projectileRb.rotation.eulerAngles
                + transform.up
                    * resolvedSpeed
                    * rotationSpeedModifier
                    * Time.fixedDeltaTime
                    * Time.timeScale
        );

        projectileRb.MoveRotation(newRotation);
    }
}
