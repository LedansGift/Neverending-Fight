using UnityEngine;

public class HomingProjectile : Projectile
{
    private float homingStrength = 1f;
    private Transform homingTarget;

    public override void ActivateProjectile(
        int damage,
        float speed,
        Transform target = null,
        float homingStrength = 1f
    )
    {
        base.ActivateProjectile(damage, speed, target);

        homingTarget = target;
        this.homingStrength = homingStrength;
    }

    protected override void MoveProjectile()
    {
        base.MoveProjectile();

        if (homingTarget)
        {
            HomeToTarget();
        }
    }

    //Based on solution at https://www.reddit.com/r/Unity3D/comments/cj7niq/rotating_an_object_to_face_player_on_only_y_axis/

    private void HomeToTarget()
    {
        Vector3 targetDirection = transform.position - homingTarget.position;
        float radians = Mathf.Atan2(targetDirection.x, targetDirection.z);
        float degrees = radians * Mathf.Rad2Deg;

        float rotationAmount = Mathf.Min(homingStrength * Time.deltaTime, 1f);
        Quaternion targetRotation = Quaternion.Euler(0f, degrees, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationAmount);
    }
}
