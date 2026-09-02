using System;
using UnityEngine;

public class BossHealth : Health
{
    // [SerializeField]
    // private AudioClip enemyHitSFX;

    // [SerializeField]
    // private AudioClip enemyDeathSFX;
    public static Action OnBossDie;

    public static EventHandler<int> OnInitialiseBossHealth;
    public static EventHandler<int> OnChangeBossHealth;

    private void Awake()
    {
        isInvincible = true;
    }

    private void Start()
    {
        HealToFull();
    }

    public void InitialiseHealth(int bossHealth = -1)
    {
        isInvincible = false;

        if (bossHealth < 0)
        {
            bossHealth = maxHealth;
        }

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
            OnBossDie?.Invoke();
            Die();
        }
        else
        {
            OnTakeDamage?.Invoke();
            damagedSFX?.PlaySFX(transform.position);
        }
    }

    public float GetHealthPercentage()
    {
        return (float)health / (float)maxHealth * 100f;
    }
}
