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

    protected void HealToFull()
    {
        health = maxHealth;
    }

    protected void Die()
    {
        OnDeath?.Invoke();
    }

    public bool GetIsPlayer()
    {
        return isPlayer;
    }

    public void SetInvincibility(bool invincible)
    {
        isInvincible = invincible;
    }
}
