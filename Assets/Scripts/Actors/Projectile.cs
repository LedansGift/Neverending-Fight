using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool projectileActive = false;
    private float lifetimeTimer = 0f;

    [SerializeField]
    private bool enemyProjectile = false;

    [SerializeField]
    private bool damagingProjectile = true;

    [SerializeField]
    private bool invincibleProjectile = false;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float lifetime = 4f;

    private Health projectileHealth;

    [SerializeField]
    private GameObject projectileVisual;

    [SerializeField]
    private TrailRenderer trail;

    [SerializeField]
    private ParticleSystem hitParticles;

    [SerializeField]
    private Collider projectileCollider;

    public EventHandler<bool> OnProjectileActivated;
    public EventHandler<float> OnSetProjectileSpeed;

    private void Awake()
    {
        projectileHealth = GetComponent<Health>();

        projectileHealth.OnDeath += TryDestroyProjectile;

        projectileHealth.SetInvincibility(invincibleProjectile);

        DeactivateProjectile();
    }

    private void OnDisable()
    {
        projectileHealth.OnDeath -= TryDestroyProjectile;
    }

    public virtual void ActivateProjectile()
    {
        projectileActive = true;
        lifetimeTimer = 0f;

        projectileVisual.SetActive(true);
        projectileCollider.enabled = true;

        if (trail)
        {
            trail.Clear();
            trail.emitting = true;
        }

        OnProjectileActivated?.Invoke(this, true);
    }

    public void DeactivateProjectile()
    {
        if (projectileActive)
        {
            if (hitParticles)
            {
                hitParticles.gameObject.SetActive(false);
                hitParticles.gameObject.SetActive(true);
            }
        }

        projectileActive = false;
        projectileVisual.SetActive(false);
        projectileCollider.enabled = false;

        if (trail)
        {
            trail.emitting = false;
        }

        OnProjectileActivated?.Invoke(this, false);
    }

    public void SetSpeedAndDamage(float speed, int damage)
    {
        this.damage = damage;
        OnSetProjectileSpeed?.Invoke(this, speed);
    }

    private void Update()
    {
        if (!projectileActive)
        {
            return;
        }

        lifetimeTimer += Time.deltaTime;

        if (lifetimeTimer > lifetime)
        {
            DeactivateProjectile();
        }
    }

    private void TryDestroyProjectile()
    {
        DeactivateProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!projectileActive || !damagingProjectile)
        {
            return;
        }

        if (other.TryGetComponent<Health>(out Health health))
        {
            if (enemyProjectile != health.GetIsPlayer())
            {
                return;
            }

            health.TakeDamage(damage);
            DeactivateProjectile();
        }
    }
}
