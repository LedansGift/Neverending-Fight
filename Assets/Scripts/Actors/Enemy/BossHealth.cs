using System;
using UnityEngine;

public class BossHealth : Health
{
    protected bool healthActive = false;
    public EventHandler<int> OnIncomingDamage;
    public static Action OnBossDie;
    public static EventHandler<int> OnInitialiseBossHealth;
    public static EventHandler<int> OnChangeBossHealth;

    protected virtual void Awake()
    {
        healthActive = false;
        isInvincible = true;
    }

    private void Start()
    {
        HealToFull();
    }

    public virtual void InitialiseHealth(int bossHealth = -1)
    {
        isInvincible = false;
        healthActive = true;

        if (bossHealth < 0)
        {
            bossHealth = maxHealth;
        }

        SetMaxHealth(bossHealth);
        OnInitialiseBossHealth?.Invoke(this, health);
    }

    public void DeactivateHealth()
    {
        healthActive = false;
    }

    public override bool TakeDamage(int damage, bool arenaWideDamage = false)
    {
        if (!healthActive)
        {
            return false;
        }

        OnIncomingDamage?.Invoke(this, damage);

        if (isInvincible)
        {
            return false;
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

        return true;
    }

    public float GetHealthPercentage()
    {
        return (float)health / (float)maxHealth * 100f;
    }
}
