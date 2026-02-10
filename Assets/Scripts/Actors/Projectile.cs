using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool projectileActive = false;
    private float lifetimeTimer = 0f;

    [SerializeField]
    private bool enemyProjectile = false;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float lifetime = 4f;

    [SerializeField]
    private GameObject projectileVisual;

    [SerializeField]
    private Rigidbody projectileRb;

    private void Awake()
    {
        DeactivateProjectile();
    }

    public void ActivateProjectile(int damage, float speed)
    {
        projectileActive = true;
        lifetimeTimer = 0f;

        this.damage = damage;
        this.speed = speed;

        projectileVisual.SetActive(true);
    }

    private void DeactivateProjectile()
    {
        projectileActive = false;
        projectileVisual.SetActive(false);
    }

    private void MoveProjectile()
    {
        projectileRb.MovePosition(
            projectileRb.position + transform.forward * speed * Time.fixedDeltaTime
        );
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

    private void FixedUpdate()
    {
        if (projectileActive)
        {
            MoveProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
