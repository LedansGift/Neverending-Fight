using System;
using UnityEngine;

public class BossHealth : Health
{
    // [SerializeField]
    // private AudioClip enemyHitSFX;

    // [SerializeField]
    // private AudioClip enemyDeathSFX;

    public static EventHandler<int> OnInitialiseBossHealth;
    public static EventHandler<int> OnChangeBossHealth;

    private void Start()
    {
        HealToFull();
    }

    public void InitialiseHealth(int bossHealth)
    {
        SetMaxHealth(bossHealth);
        OnInitialiseBossHealth?.Invoke(this, health);
    }

    public override void TakeDamage(int damage, bool arenaWideDamage = false)
    {
        if (isInvincible)
        {
            return;
        }

        health = Mathf.Max(0, health - damage);

        OnChangeBossHealth?.Invoke(this, health);

        if (health == 0f)
        {
            //AudioManager.PlaySFX(enemyDeathSFX, 1f, 0, transform.position);

            Die();
        }
        else
        {
            OnTakeDamage?.Invoke();
            //AudioManager.PlaySFX(enemyHitSFX, 1f, 0, transform.position);
        }
    }
}
