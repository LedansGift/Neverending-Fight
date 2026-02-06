using System;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField]
    private bool isPlayer = false;

    [SerializeField]
    private int maxHealth = 100;
    protected int health;
    public Action OnTakeDamage;
    public Action OnDeath;

    public abstract void TakeDamage(int damageAmount);

    protected void SetMaxHealth()
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
}
