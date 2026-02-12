using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerHealth : Health
{
    private bool invincible = false;
    private float invincibilityDuration = 2f;

    //private float impulseStrength = 1f;

    private PlayerStats playerStats;

    // [SerializeField]
    // private AudioClip playerDamageSFX;

    // [SerializeField]
    // private AudioClip playerDeathSFX;

    [SerializeField]
    private CinemachineImpulseSource impulseSource;

    public static EventHandler<int> OnChangePlayerHealth;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        SetMaxHealth(playerStats.GetHealth());
        OnChangePlayerHealth?.Invoke(this, health);
    }

    private IEnumerator DamageInvincibility()
    {
        invincible = true;

        yield return new WaitForSeconds(invincibilityDuration);

        invincible = false;
    }

    public override void TakeDamage(int damage)
    {
        if (invincible)
        {
            return;
        }

        //impulseSource.GenerateImpulse(impulseStrength);

        health = Mathf.Max(0, health - damage);

        OnChangePlayerHealth?.Invoke(this, health);

        if (health == 0)
        {
            //AudioManager.PlaySFX(playerDeathSFX, 1f, 0, transform.position);
            Die();
        }
        else
        {
            //AudioManager.PlaySFX(playerDamageSFX, 1f, 0, transform.position);
            StartCoroutine(DamageInvincibility());
        }
    }
}
