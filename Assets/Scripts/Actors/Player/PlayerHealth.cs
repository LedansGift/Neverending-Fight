using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerHealth : Health
{
    private bool isDead = false;
    private bool attackFailTracker = false;
    private float iFrameDuration = 1f;

    private float impulseStrength = 1f;
    private float deathImpulseStrength = 3f;

    private PlayerStats playerStats;
    private PlayerMovement playerMovement;

    private Coroutine iFrameCoroutine;

    [SerializeField]
    private CinemachineImpulseSource impulseSource;

    public static EventHandler<int> OnInitialisePlayerHealth;
    public static EventHandler<int> OnChangePlayerHealth;

    private void Start()
    {
        SetInvincibility(false);
        playerStats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<PlayerMovement>();
        SetMaxHealth(playerStats.GetHealth());

        OnInitialisePlayerHealth?.Invoke(this, playerStats.GetHealth());
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
        SetInvincibility(true);

        yield return new WaitForSeconds(iFrameDuration);

        SetInvincibility(false);
    }

    public void TakeTickDamage(int damage)
    {
        if (isDead || isInvincible)
        {
            return;
        }

        health = Mathf.Max(0, health - damage);

        OnChangePlayerHealth?.Invoke(this, health);

        if (health == 0)
        {
            isDead = true;
            Die();
        }
    }

    public override void TakeDamage(int damage, bool arenaWideDamage = false)
    {
        if (isDead || (isInvincible && !arenaWideDamage))
        {
            return;
        }

        health = Mathf.Max(0, health - damage);

        if (!arenaWideDamage)
        {
            attackFailTracker = true;
        }

        OnChangePlayerHealth?.Invoke(this, health);

        if (health == 0)
        {
            impulseSource.GenerateImpulse(deathImpulseStrength);
            isDead = true;
            Die();
        }
        else
        {
            damagedSFX?.PlaySFX(transform.position);

            impulseSource.GenerateImpulse(impulseStrength);
            iFrameCoroutine = StartCoroutine(DamageInvincibility());
        }
    }

    //KNOCKBACKS NEED TO HAPPEN BEFORE DAMAGE

    public override void Knockback(
        Vector3 knockbackDirection,
        float knockbackStrength,
        bool forceKnockback = false
    )
    {
        if (isDead || (isInvincible && !forceKnockback))
        {
            return;
        }

        playerMovement.ApplyKnockback(knockbackDirection, knockbackStrength);
    }

    public void Heal(int healAmount)
    {
        if (isDead)
        {
            return;
        }

        health = Mathf.Min(health + healAmount, maxHealth);

        OnChangePlayerHealth?.Invoke(this, health);
    }

    public void RevivePlayer()
    {
        isDead = false;
        ResetAttackFailStatus();
        HealToFull();

        OnChangePlayerHealth?.Invoke(this, health);

        if (iFrameCoroutine != null)
        {
            StopCoroutine(iFrameCoroutine);
        }

        SetInvincibility(false);
    }

    public bool GetAttackFailStatus()
    {
        return attackFailTracker;
    }

    public void ResetAttackFailStatus()
    {
        attackFailTracker = false;
    }

    private void SetJumpInvincibility(object sender, bool toggle)
    {
        if (iFrameCoroutine != null)
        {
            StopCoroutine(iFrameCoroutine);
        }

        SetInvincibility(toggle);
    }
}
