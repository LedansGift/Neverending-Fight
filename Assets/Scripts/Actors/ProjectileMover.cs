using System;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    private bool moverActive = false;
    private float rotationSpeedModifier = 10f;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected float zRotationSpeed;

    [SerializeField]
    protected Rigidbody projectileRb;

    [SerializeField]
    protected Transform rotatingElement;

    [SerializeField]
    protected Projectile projectile;

    [SerializeField]
    protected AnimationCurve speedOverLifetime;

    private void Awake()
    {
        projectile.OnProjectileActivated += ToggleMover;
        projectile.OnSetProjectileSpeed += UpdateSpeed;
    }

    private void OnDisable()
    {
        projectile.OnProjectileActivated -= ToggleMover;
        projectile.OnSetProjectileSpeed -= UpdateSpeed;
    }

    private void Update()
    {
        if (moverActive && rotatingElement)
        {
            RotateProjectile();
        }
    }

    private void FixedUpdate()
    {
        if (moverActive)
        {
            MoveProjectile();
        }
    }

    protected virtual void MoveProjectile()
    {
        float resolvedSpeed = speedOverLifetime.Evaluate(projectile.GetLifetimeProgress()) * speed;

        projectileRb.MovePosition(
            projectileRb.position
                + transform.forward * resolvedSpeed * Time.fixedDeltaTime * Time.timeScale
        );
    }

    protected virtual void RotateProjectile()
    {
        rotatingElement.eulerAngles += new Vector3(
            0f,
            0f,
            zRotationSpeed * rotationSpeedModifier * Time.deltaTime
        );
    }

    private void ToggleMover(object sender, bool toggle)
    {
        moverActive = toggle;
    }

    private void UpdateSpeed(object sender, float newSpeed)
    {
        speed = newSpeed;
    }
}
