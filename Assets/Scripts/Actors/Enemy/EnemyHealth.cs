using UnityEngine;

public class EnemyHealth : Health
{
    // [SerializeField]
    // private AudioClip enemyHitSFX;

    [SerializeField]
    private VFXPlayer enemyHitVFX;

    // [SerializeField]
    // private AudioClip enemyDeathSFX;
    [SerializeField]
    private VFXPlayer enemyDeathVFX;

    private void Start()
    {
        HealToFull();
    }

    public override void TakeDamage(int damage, bool arenaWideDamage = false)
    {
        if (isInvincible)
        {
            return;
        }

        health = Mathf.Max(0, health - damage);

        if (health == 0f)
        {
            //AudioManager.PlaySFX(enemyDeathSFX, 1f, 0, transform.position);

            if (enemyDeathVFX)
            {
                enemyDeathVFX.PlayVFX(transform);
            }

            Die();
        }
        else
        {
            //AudioManager.PlaySFX(enemyHitSFX, 1f, 0, transform.position);

            if (enemyHitVFX)
            {
                enemyHitVFX.PlayVFX(transform);
            }

            OnTakeDamage?.Invoke();
        }
    }
}
