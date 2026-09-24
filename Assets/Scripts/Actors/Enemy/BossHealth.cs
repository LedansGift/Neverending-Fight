using System;
using UnityEngine;

public class BossHealth : Health
{
    public EventHandler<int> OnIncomingDamage;
    public static Action OnBossDie;
    public static EventHandler<int> OnInitialiseBossHealth;
    public static EventHandler<int> OnChangeBossHealth;

    protected virtual void Awake()
    {
        isInvincible = true;
    }

    private void Start()
    {
        HealToFull();
    }

    public virtual void InitialiseHealth(int bossHealth = -1)
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
        OnIncomingDamage?.Invoke(this, damage);

        if (isInvincible)
        {
            return;
        }

        int minimumHealthThreshold = 0;

        if (isUnkillable)
        {
            minimumHealthThreshold = 1;
        }

        health = Mathf.Max(minimumHealthThreshold, health - damage);

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
