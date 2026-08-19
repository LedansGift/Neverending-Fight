using UnityEngine;

public class HomingProjectileMover : ProjectileMover
{
    [SerializeField]
    private float homingStrength = 1f;
    private Transform homingTarget;

    private void Start()
    {
        homingTarget = PlayerIdentifier.PlayerTransform;
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
        Vector3 targetDirection = homingTarget.position - transform.position;
        float radians = Mathf.Atan2(targetDirection.x, targetDirection.z);
        float degrees = radians * Mathf.Rad2Deg;

        float rotationAmount = homingStrength * Time.fixedDeltaTime;
        //float rotationAmount = Mathf.Min(homingStrength * Time.fixedDeltaTime, 1f);
        Quaternion targetRotation = Quaternion.Euler(0f, degrees, 0f);
        projectileRb.MoveRotation(
            //Quaternion.Slerp(transform.rotation, targetRotation, rotationAmount)
            Quaternion.RotateTowards(transform.rotation, targetRotation, rotationAmount)
        );
    }
}
