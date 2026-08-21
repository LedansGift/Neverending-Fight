using System;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField]
    private bool isPlayer = false;

    [SerializeField]
    protected bool isInvincible = false;

    [SerializeField]
    protected int maxHealth = 100;
    protected int health;
    public Action OnTakeDamage;
    public Action OnDeath;

    public abstract void TakeDamage(int damageAmount, bool arenaWideDamage = false);

    protected void SetMaxHealth(int newHealth, bool healToFull = true)
    {
        maxHealth = newHealth;

        if (healToFull)
        {
            HealToFull();
        }
    }

    public virtual void HealToFull()
    {
        health = maxHealth;
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke();
    }

    public virtual void Knockback(
        Vector3 knockbackDirection,
        float knockbackStrength,
        bool forceKnockback = false
    ) { }

    public bool GetIsPlayer()
    {
        return isPlayer;
    }

    public void SetInvincibility(bool invincible)
    {
        isInvincible = invincible;
    }
}
