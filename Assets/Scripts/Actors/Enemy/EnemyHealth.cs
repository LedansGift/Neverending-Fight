using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField]
    private bool isInvincible = false;

    // [SerializeField]
    // private AudioClip enemyHitSFX;

    // [SerializeField]
    // private AudioClip enemyDeathSFX;

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

            //Die();

            Destroy(gameObject);
        }
        else
        {
            OnTakeDamage?.Invoke();
            //AudioManager.PlaySFX(enemyHitSFX, 1f, 0, transform.position);
        }
    }
}
