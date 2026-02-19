using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerHealth : Health
{
    private bool isDead = false;
    private bool invincible = false;
    private float invincibilityDuration = 1f;

    //private float impulseStrength = 1f;

    private PlayerStats playerStats;

    // [SerializeField]
    // private AudioClip playerDamageSFX;

    // [SerializeField]
    // private AudioClip playerDeathSFX;

    private Coroutine iFrameCoroutine;

    [SerializeField]
    private CinemachineImpulseSource impulseSource;

    public static EventHandler<int> OnChangePlayerHealth;

    private void Start()
    {
        invincible = false;
        playerStats = GetComponent<PlayerStats>();
        SetMaxHealth(playerStats.GetHealth());
        OnChangePlayerHealth?.Invoke(this, health);
    }

    private void OnEnable()
    {
        PlayerGlaive.OnGlaiveSpecial += SetJumpInvincibility;
    }

    private void OnDisable()
    {
        PlayerGlaive.OnGlaiveSpecial -= SetJumpInvincibility;
    }

    private IEnumerator DamageInvincibility()
    {
        invincible = true;

        yield return new WaitForSeconds(invincibilityDuration);

        invincible = false;
    }

    public override void TakeDamage(int damage, bool arenaWideDamage = false)
    {
        if (isDead || (invincible && !arenaWideDamage))
        {
            return;
        }

        //impulseSource.GenerateImpulse(impulseStrength);

        health = Mathf.Max(0, health - damage);

        OnChangePlayerHealth?.Invoke(this, health);

        if (health == 0)
        {
            //AudioManager.PlaySFX(playerDeathSFX, 1f, 0, transform.position);
            isDead = true;
            Die();
        }
        else
        {
            //AudioManager.PlaySFX(playerDamageSFX, 1f, 0, transform.position);
            iFrameCoroutine = StartCoroutine(DamageInvincibility());
        }
    }

    public void RevivePlayer()
    {
        isDead = false;
        HealToFull();

        if (iFrameCoroutine != null)
        {
            StopCoroutine(iFrameCoroutine);
        }

        invincible = false;
    }

    private void SetJumpInvincibility(object sender, bool toggle)
    {
        if (iFrameCoroutine != null)
        {
            StopCoroutine(iFrameCoroutine);
        }

        invincible = toggle;
    }
}
