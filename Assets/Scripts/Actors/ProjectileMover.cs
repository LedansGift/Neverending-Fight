using System;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    private bool moverActive = false;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float zRotationSpeed;

    [SerializeField]
    private Rigidbody projectileRb;

    [SerializeField]
    private Transform rotatingElement;

    [SerializeField]
    private Projectile projectile;

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
        projectileRb.MovePosition(
            projectileRb.position + transform.forward * speed * Time.fixedDeltaTime * Time.timeScale
        );
    }

    protected virtual void RotateProjectile()
    {
        rotatingElement.eulerAngles += new Vector3(0f, 0f, zRotationSpeed * Time.deltaTime);
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
