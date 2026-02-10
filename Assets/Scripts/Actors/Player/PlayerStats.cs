using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float movementSpeed = 7.5f;

    [SerializeField]
    private float dashTime = 1f;

    [SerializeField]
    private float dashSpeed = 20f;

    [SerializeField]
    private float dashRecharge = 1.5f;

    [Header("Glaive")]
    [SerializeField]
    private int glaiveDamage = 5;

    [SerializeField]
    private float glaiveAttackRange = 5f;

    [SerializeField]
    [Range(0, 1)]
    private float glaiveAttackArc = 0.5f;

    [SerializeField]
    private float glaiveAttackCooldown = 1.25f;

    [SerializeField]
    private float glaiveRhythmStart = 0.5f;

    [SerializeField]
    private float glaiveRhythmDuration = 0.25f;

    [Header("Bow")]
    [SerializeField]
    private int bowDamage = 4;

    [SerializeField]
    private int bowPerfectDamage = 6;

    [SerializeField]
    private float bowProjectileSpeed = 10f;

    [SerializeField]
    private float bowChargeTime = 1f;

    [SerializeField]
    private float bowShootCooldown = 0.5f;

    [SerializeField]
    private float bowPerfectChargeTime = 0.7f;

    [SerializeField]
    private float bowPerfectChargeDuration = 0.15f;

    [Header("Tome")]
    [SerializeField]
    private int tomeMaxDamage = 15;

    [SerializeField]
    private float tomeMaxRadius = 10f;

    [SerializeField]
    private float tomeMaxMovementModifier = 0.4f;

    [SerializeField]
    private float tomeChargeTime = 2.5f;

    [SerializeField]
    private float tomeAttackCooldown = 0.5f;

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public float GetDashTime()
    {
        return dashTime;
    }

    public float GetDashSpeedModifier()
    {
        return dashSpeed;
    }

    public float GetDashRechargeTime()
    {
        return dashRecharge;
    }

    public int GetGlaiveDamage()
    {
        return glaiveDamage;
    }

    public float GetGlaiveAttackRange()
    {
        return glaiveAttackRange;
    }

    public float GetGlaiveAttackArc()
    {
        return glaiveAttackArc;
    }

    public float GetGlaiveAttackCooldown()
    {
        return glaiveAttackCooldown;
    }

    public float GetGlaiveRhythmStart()
    {
        return glaiveRhythmStart;
    }

    public float GetGlaiveRhythmDuration()
    {
        return glaiveRhythmDuration;
    }

    public int GetBowDamage()
    {
        return bowDamage;
    }

    public int GetPerfectBowDamage()
    {
        return bowPerfectDamage;
    }

    public float GetBowProjectileSpeed()
    {
        return bowProjectileSpeed;
    }

    public float GetBowChargeTime()
    {
        return bowChargeTime;
    }

    public float GetBowShootCooldown()
    {
        return bowShootCooldown;
    }

    public float GetBowPerfectChargeTime()
    {
        return bowPerfectChargeTime;
    }

    public float GetBowPerfectChargeDuration()
    {
        return bowPerfectChargeDuration;
    }

    public int GetTomeMaxDamage()
    {
        return tomeMaxDamage;
    }

    public float GetTomeMaxRadius()
    {
        return tomeMaxRadius;
    }

    public float GetTomeMaxMovementModifier()
    {
        return tomeMaxMovementModifier;
    }

    public float GetTomeChargeTime()
    {
        return tomeChargeTime;
    }

    public float GetTomeAttackCooldown()
    {
        return tomeAttackCooldown;
    }
}
